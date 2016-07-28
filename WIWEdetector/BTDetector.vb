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
        Dim fullText As String
        Dim hNap As String
        Dim hSap As String
        Dim hMac As String

        Form1.TextBoxWIWE.Text = vbNullString
        Form1.TextBoxInfo.Text = vbNullString
        sText = vbNullString
        fullText = vbNullString
        For i As Integer = 0 To count - 1
            Dim device As New BTDeviceInfo(array(i))
            hNap = Hex(device.Nap)
            hSap = Hex(device.Sap)
            hMac = hNap & hSap
            fullText = fullText & "------------------------" & vbCrLf
            fullText = fullText & device.DeviceName & vbCrLf
            fullText = fullText & hMac & vbCrLf

            If IsWiwe(device.DeviceName, hNap) Then
                sText = hMac
                If IsInSQLDatabase(hMac) Then
                    fullText = fullText & "WIWE az adatbázisban" & vbCrLf
                Else
                    fullText = fullText & "Új WIWE" & vbCrLf
                    Call WriteToSQLDatabase(device.DeviceName, hMac)
                    Call PrintZPL(hMac, 1)
                End If
            Else
                fullText = fullText & "nem WIWE" & vbCrLf
            End If
        Next
        Form1.TextBoxInfo.Text = fullText
        If sText = vbNullString Then
            Form1.TextBoxWIWE.Text = "Nincs WIWE"
        Else
            Form1.TextBoxWIWE.Text = sText
        End If
    End Sub
    Private Function IsWiwe(device_name As String, hdevice_nap As String) As Boolean
        'Ellenőrzi, hogy WIWE-e
        '1., WIWE a kezdete a névnek
        '2., B0B4 a BT chip gyártója
        Dim res As Boolean

        res = False
        If device_name.StartsWith("WIWE") And hdevice_nap = "B0B4" Then
            res = True
        End If

        Return res
    End Function
End Module
