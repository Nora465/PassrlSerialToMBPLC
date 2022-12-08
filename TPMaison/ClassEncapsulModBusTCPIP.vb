Imports System.IO.Ports
Imports System.Net.Sockets

Public Class ClassEncapsulModBusTCPIP
	'TODO refaire les descriptions des méthodes

	Dim _SP As SerialPort

#Region "Structures Public"

	Public Structure DT_MBAPHeader 'DT : DataType
		Dim TransactionID As UInt16 '2 octets : pour identifier la transaction
		Dim ProtocoleID As UInt16 '2 octets : pour identifier le protocole (ici, Modbus => 0)
		Dim Length As UInt16 '2 octets : Taille à partir du UnitID jusqu'à la fin du message
		Dim UnitID As Byte '1 octet : Identifie le numéro de l'esclave 
		'(si passerelle MB TCP vers MB RTU, mettre l'adresse esclave (sinon, mettre 0 (pour atteindre la passerelle))
	End Structure

	'Pour la lecture de mots/bits // écrire UN SEUL bit/mot
	Public Structure DT_BasicPDU
		Dim FunctionCode As Byte
		Dim StartAddr As UInt16 '2 octets : Adresse de Début /OU/ Adresse du Bit/Mot à écrire
		Dim ValueOrNumOfVars As UInt16 '2 octets : Nombre de variables (R ou W) /OU/ ValeurOUEtat (W_One)
		Dim TotalLength As Byte 'Taille total du PDU
	End Structure

	'Pour écrire PLUSIEURS bits/mots (pas encore implémentée)
	Public Structure DT_WMultiplePDU
		Dim FunctionCode As Byte
		Dim StartAddr As UInt16 '2 octets : Adresse de Début /OU/ Adresse du Bit/Mot à écrire
		Dim NumOfVars As UInt16 '2 octets : Nombre de variables (R ou W) /OU/ ValeurOUEtat (W_One)
		Dim LengthOfBytes As Byte '1 octet : Nombre d'octets nécessaires pour contenir l'état des bits/mots
		Dim ValuesOfVars() As Byte 'Bits : Etat des bits (Ordre : 0 1 2 ...) /OU/ Valeur des mots (Big Endian: MSB - LSB)
		Dim TotalLength As Byte 'taille total du PDU
	End Structure

#End Region

	Public Sub New()

	End Sub

#Region "Public Functions"

#Region "Read/Write From Modbus TCP/IP"
	'Testé & validé (06/07/2020 00h12)
	Public Function ReadSomeBits(ByVal AddrFirstBit As UInt16, ByVal NbrOfBits As UInt16) As Byte()
		Dim PDU As DT_BasicPDU = _BuildPDU_RAll_WSingle(1, AddrFirstBit, NbrOfBits)
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function

	'Testé & validé (06/07/2020 00h12)
	Public Function ReadSomeWords(ByVal AddrFirstWord As UInt16, ByVal NbrOfWords As UInt16) As Byte()
		Dim PDU As DT_BasicPDU = _BuildPDU_RAll_WSingle(3, AddrFirstWord, NbrOfWords)
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function

	'Testé & Validé (06/07/2020 00h12)
	Public Function WriteOneBit(ByVal AddrBit As UInt16, ByVal ValueBit As Boolean) As Byte()
		Dim PDU As DT_BasicPDU = _BuildPDU_RAll_WSingle(5, AddrBit, ValueBit)
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function

	'Testé et validé (06/07/2020 00h12)
	Public Function WriteOneWord(ByVal AddrWord As UInt16, ByVal ValueWord As UInt16) As Byte()
		Dim PDU As DT_BasicPDU = _BuildPDU_RAll_WSingle(6, AddrWord, ValueWord)
		'PDU.FunctionCode = &H_00F1
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function

	Public Function WriteMultipleBits(ByVal AddrFirstBit As UInt16, ByVal NbrOfBits As UInt16, ByVal ValueBits As Boolean()) As Byte()
		Throw New NotImplementedException("pas encore implémentée !")
		Dim PDU As DT_WMultiplePDU = _BuildPDU_WMultiple(&H_15, AddrFirstBit, NbrOfBits, ValueBits)
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function

	Public Function WriteMultipleWords(ByVal AddrFirstWord As UInt16, ByVal NbrOfWord As UInt16, ByVal ValueWords As UInt16()) As Byte()
		Throw New NotImplementedException("pas encore implémentée !")
		Dim PDU As DT_WMultiplePDU = _BuildPDU_WMultiple(&H_16, AddrFirstWord, NbrOfWord, ValueWords)
		Dim MBAPHeader As DT_MBAPHeader = _BuildMBAPHeader(PDU)

		Return _BuildModbusTCPTrame(PDU, MBAPHeader)
	End Function
#End Region

#Region "Décomposition de la Trame de réponse"
	Public Function déconstructionTrameReponse(ByVal trameReponse As Byte()) As (MBAPHeader As DT_MBAPHeader, DTU As DT_BasicPDU)
		Throw New NotImplementedException("pas encore implémentée")
	End Function

#End Region

#End Region

#Region "Private Functions"

#Region "Construction de la Trame TCP/IP"
	''' <summary>
	''' Construire la trame permettant de communiquer avec l'API sur TCP/IP (protocole ModBusTCP)
	''' </summary>
	''' <param name="PDU"> Le PDU assemblé par <see cref="_BuildPDU_RAll_WSingle"/> (ou autre Fonction) </param>
	''' <param name="MBAPHeader"> Le MBAP Header, créé par <see cref="_BuildMBAPHeader"/> </param>
	''' <returns> La trame (en byte()) </returns>
	''' Private Function _BuildModbusTCPTrame(ByRef PDU As DT_BasicPDU_NEW, ByRef MBAPHeader As DT_MBAPHeader) As Byte()
	Private Function _BuildModbusTCPTrame(ByRef PDU As DT_BasicPDU, ByRef MBAPHeader As DT_MBAPHeader) As Byte()

		'Trame sous forme de Tableau de byte
		Dim RetTrame(PDU.TotalLength + 6) As Byte

		With MBAPHeader
			RetTrame(0) = .TransactionID >> 8
			RetTrame(1) = .TransactionID And &H_00FF
			RetTrame(2) = .ProtocoleID >> 8
			RetTrame(3) = .ProtocoleID And &H_00FF
			RetTrame(4) = .Length >> 8
			RetTrame(5) = .Length And &H_00FF
			RetTrame(6) = .UnitID
		End With

		With PDU
			RetTrame(7) = .FunctionCode
			RetTrame(8) = .StartAddr >> 8
			RetTrame(9) = .StartAddr And &H_00FF
			RetTrame(10) = .ValueOrNumOfVars >> 8
			RetTrame(11) = .ValueOrNumOfVars And &H_00FF
		End With

		Return RetTrame
	End Function

	''' <summary>
	''' Construire la trame permettant de communiquer avec l'API sur TCP/IP (protocole ModBusTCP)
	''' </summary>
	''' <param name="PDU"> Le PDU assemblé par <see cref="_BuildPDU_RAll_WSingle"/> (ou autre Fonction) </param>
	''' <param name="MBAPHeader"> Le MBAP Header, créé par <see cref="_BuildMBAPHeader"/> </param>
	''' <returns> La trame (en byte()) </returns>
	''' Private Function _BuildModbusTCPTrame(ByRef PDU As DT_BasicPDU_NEW, ByRef MBAPHeader As DT_MBAPHeader) As Byte()
	Private Function _BuildModbusTCPTrame(ByRef PDU As DT_WMultiplePDU, ByRef MBAPHeader As DT_MBAPHeader) As Byte()

		'Trame sous forme de Tableau de byte
		Dim RetTrame(PDU.TotalLength + 6) As Byte

		With MBAPHeader
			RetTrame(0) = .TransactionID >> 8
			RetTrame(1) = .TransactionID And &H_00FF
			RetTrame(2) = .ProtocoleID >> 8
			RetTrame(3) = .ProtocoleID And &H_00FF
			RetTrame(4) = .Length >> 8
			RetTrame(5) = .Length And &H_00FF
			RetTrame(6) = .UnitID
		End With

		'???????????
		'With PDU
		'	RetTrame(7) = .FunctionCode
		'	RetTrame(8) = .StartAddr >> 8
		'	RetTrame(9) = .StartAddr And &H_00FF
		'	RetTrame(10) = .ValueOrNumOfVars >> 8
		'	RetTrame(11) = .ValueOrNumOfVars And &H_00FF
		'End With

		Return RetTrame
	End Function
#End Region

#Region "Construction du MBAP Header"

	''' <summary>
	''' Construit la partie de la trame ModBus qui contient le Header
	''' </summary>
	''' <param name="PDU"> Le PDU généré </param>
	''' <returns> Retourne le MBAPHeader sous forme de DT_MBAPHeader </returns>
	'''Private Function _BuildMBAPHeader(ByVal PDU As DT_BasicPDU_NEW) As DT_MBAPHeader
	Private Function _BuildMBAPHeader(ByVal PDU As Object) As DT_MBAPHeader

		Dim RetMBAPHeader As New DT_MBAPHeader

		With RetMBAPHeader
			'nombre aléatoire pour le transaction ID
			.TransactionID = CByte(Math.Floor((Byte.MaxValue - Byte.MinValue + 1) * Rnd()) + Byte.MinValue)
			.ProtocoleID = 0
			.UnitID = &H_FF
			.Length = Len(.UnitID) + PDU.TotalLength
		End With

		Return RetMBAPHeader
	End Function
#End Region

#Region "Construction des PDU"

	'PDU : ensemble de données représentant le code function et les données qui suivent
	''' <summary>
	''' Construit la partie de la trame ModBus qui contient les données utiles
	''' Cette surcharge permet de lire tout types de variables (Bit ou Mot / Un ou plusieurs)
	''' ou d'écrire une seule variable (Bit ou Mot)
	''' Codes fonction supportés : 01/02/03/04/XX/06
	''' </summary>
	''' <param name="FunctionCode"> Définie le type de donnée à atteindre (Bit/Mot, un/plusieurs) </param>
	''' <param name="AddrDébut"> Définie l'adresse de début (pour lire) et l'adresse du mot à écrire </param>
	''' <param name="NbrALireOU_ValeurReg"> Le nombre de registre à lire OU La valeur d'un registre (RW) </param>
	''' <returns></returns>
	Private Function _BuildPDU_RAll_WSingle(ByVal FunctionCode As Byte,
											ByVal AddrDébut As UInt16,
											ByVal NbrALireOU_ValeurReg As UInt16
											) As DT_BasicPDU
		Dim RetPDU As New DT_BasicPDU

		With RetPDU
			.FunctionCode = FunctionCode
			.StartAddr = AddrDébut
			.ValueOrNumOfVars = NbrALireOU_ValeurReg

			.TotalLength = Len(.FunctionCode) + Len(.StartAddr) + Len(.ValueOrNumOfVars)
		End With

		Return RetPDU
	End Function

	''' <summary>
	''' Construit la partie de la trame ModBus qui contient les données utiles
	''' Cette surcharge permet d'écrire une seule variable (Type booléen)
	''' Codes fonction supportés : 05
	''' </summary>
	''' <param name="FunctionCode"></param>
	''' <param name="AddrDébut"></param>
	''' <param name="BitState"></param>
	''' <returns> PDU basic </returns>
	Private Function _BuildPDU_RAll_WSingle(ByVal FunctionCode As Byte,
											ByVal AddrDébut As UInt16,
											ByVal BitState As Boolean
											) As DT_BasicPDU
		Dim RetPDU As New DT_BasicPDU

		With RetPDU
			If BitState Then .ValueOrNumOfVars = &H_FF00 Else .ValueOrNumOfVars = &H_0000

			.StartAddr = AddrDébut
			.FunctionCode = FunctionCode
			.TotalLength = Len(.FunctionCode) + Len(.StartAddr) + Len(.ValueOrNumOfVars)
		End With

		Return RetPDU
	End Function

	''' <summary>
	''' Construit la partie de la trame ModBus qui contient les données utiles
	''' Cette surcharge permet de d'écrire plusieurs variables (Bit ou Mot) 
	''' Codes fonction supportés : 15(W Plusieurs Coils) / 16(W Plusieurs Mot)
	''' </summary>
	''' <param name="FunctionCode"> Définie le type de donnée à atteindre (Bit/Mot, un/plusieurs) </param>
	''' <param name="AddrDébut"> Définie l'adresse de début (pour lire) et l'adresse du mot à écrire </param>
	''' <param name="NbrVarsAEcrire"> Le nombre de variables à écrire (à partir de AddrDébut) </param>
	''' <param name="WordsValue"> Valeur </param>
	''' <returns> Le PDU (Charge utile) pour la trame Modbus TCP/IP </returns>
	Private Function _BuildPDU_WMultiple(ByVal FunctionCode As Byte,
										 ByVal AddrDébut As UInt16,
										 ByVal NbrVarsAEcrire As UInt16,
										 ByVal WordsValue As UInt16()
										) As DT_WMultiplePDU

		Dim RetPDU As New DT_WMultiplePDU

		'???????????
		'With RetPDU
		'    .Data = {AddrDébut, NbrVarsAEcrire, (NbrVarsAEcrire * 2) << 8}

		'    .FunctionCode = FunctionCode
		'    .TotalLength = Len(.FunctionCode) + Len(.Data(0)) + Len(.Data(1))
		'End With

		Return RetPDU
	End Function

	Private Function _BuildPDU_WMultiple(ByVal FunctionCode As Byte,
										 ByVal AddrDébut As UInt16,
										 ByVal NbrVarsAEcrire As UInt16,
										 ByVal BitsState As Boolean()
										) As DT_WMultiplePDU

		Dim RetPDU As New DT_WMultiplePDU



		'With RetPDU

		'    .Data = {AddrDébut, &H_FF00}

		'    .FunctionCode = FunctionCode
		'    .TotalLength = Len(.FunctionCode) + Len(.Data(0)) + Len(.Data(1))
		'End With

		Return RetPDU
	End Function

#End Region

#End Region
End Class
