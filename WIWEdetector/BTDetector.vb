Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.ComponentModel
Imports InTheHand.Net.Bluetooth
Imports InTheHand.Net.Sockets

Module BTDetector
    Sub DetectBT()
        Dim devices As New List(Of BTDeviceInfo)()
        Dim bc As New InTheHand.Net.Sockets.BluetoothClient()
        Dim array As InTheHand.Net.Sockets.BluetoothDeviceInfo() = bc.DiscoverDevices()
        Dim count As Integer = array.Length
        Dim sText As String

        sText = vbNullString
        Form1.TextBox1.Text = vbNullString
        For i As Integer = 0 To count - 1
            Dim device As New BTDeviceInfo(array(i))
            Console.WriteLine("------------------------------")
            Console.WriteLine(device)
            Console.WriteLine(Hex(device.Nap))
            Console.WriteLine(Hex(device.Sap))
            If IsWiwe(device.DeviceName, device.Nap) Then
                sText = Hex(device.Nap) & Hex(device.Sap)
            End If
        Next
        If sText = vbNullString Then
            Form1.TextBox1.Text = "Nincs WIWE"
        Else
            Form1.TextBox1.Text = sText
        End If
    End Sub
    Private Function IsWiwe(device_name As String, device_nap As Long) As Boolean
        'Ellenőrzi, hogy WIWE-e
        '1., WIWE a kezdete a névnek
        '2., B0B4 a BT chip gyártója
        Dim res As Boolean

        res = False
        If device_name.StartsWith("WIWE") And Hex(device_nap) = "B0B4" Then
            res = True
        End If

        Return res
    End Function
End Module
