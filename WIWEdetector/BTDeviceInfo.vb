Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports InTheHand.Net.Bluetooth
Imports InTheHand.Net.Sockets

Public Class BTDeviceInfo
    Private mstrDeviceName As String
    Private mblnAuthenticated As Boolean
    Private mblnConnected As Boolean
    Private mlngNap As Long
    Private mlngSap As Long
    Private mdtmLastSeen As DateTime
    Private mdtmLastUsed As DateTime
    Private mblnRemembered As Boolean

    Public Property DeviceName() As String
        Get
            Return mstrDeviceName
        End Get
        Set(value As String)
            mstrDeviceName = value
        End Set
    End Property
    Public Property Authenticated As Boolean
        Get
            Return mblnAuthenticated
        End Get
        Set(value As Boolean)
            mblnAuthenticated = value
        End Set
    End Property
    Public Property Connected As Boolean
        Get
            Return mblnConnected
        End Get
        Set(value As Boolean)
            mblnConnected = value
        End Set
    End Property
    Public Property Nap As Long
        Get
            Return mlngNap
        End Get
        Set(value As Long)
            mlngNap = value
        End Set
    End Property
    Public Property Sap As Long
        Get
            Return mlngSap
        End Get
        Set(value As Long)
            mlngSap = value
        End Set
    End Property
    Public Property LastSeen As DateTime
        Get
            Return mdtmLastSeen
        End Get
        Set(value As DateTime)
            mdtmLastSeen = value
        End Set
    End Property
    Public Property LastUsed As DateTime
        Get
            Return mdtmLastUsed
        End Get
        Set(value As DateTime)
            mdtmLastUsed = value
        End Set
    End Property
    Public Property Remembered As Boolean
        Get
            Return mblnRemembered
        End Get
        Set(value As Boolean)
            mblnRemembered = value
        End Set
    End Property

    Public Sub New(device_info As BluetoothDeviceInfo)
        Me.Authenticated = device_info.Authenticated
        Me.Connected = device_info.Connected
        Me.DeviceName = device_info.DeviceName
        Me.LastSeen = device_info.LastSeen
        Me.LastUsed = device_info.LastUsed
        Me.Nap = device_info.DeviceAddress.Nap
        Me.Sap = device_info.DeviceAddress.Sap
        Me.Remembered = device_info.Remembered
    End Sub
    Public Overrides Function ToString() As String
        Return Me.DeviceName
    End Function


End Class
