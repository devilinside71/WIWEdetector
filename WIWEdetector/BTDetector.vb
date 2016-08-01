Module BTDetector
    Sub DetectBT()

        Try
            Dim devices As New List(Of BTDeviceInfo)()
            Dim bc As New InTheHand.Net.Sockets.BluetoothClient()
            Dim array As InTheHand.Net.Sockets.BluetoothDeviceInfo() = bc.DiscoverDevices()
            Dim count As Integer = array.Length
            Dim sText As String
            Dim fullText As String
            Dim hNap As String
            Dim hSap As String
            Dim hMac As String
            Dim intWIWECount As Integer

            sText = vbNullString
            fullText = vbNullString
            intWIWECount = 0
            For i As Integer = 0 To count - 1
                Dim device As New BTDeviceInfo(array(i))
                hNap = Hex(device.Nap)
                hSap = Hex(device.Sap)
                hMac = hNap & hSap
                fullText = fullText & "------------------------" & vbCrLf
                fullText = fullText & device.DeviceName & vbCrLf
                fullText = fullText & hMac & vbCrLf
                fullText = fullText & device.RSSI & vbCrLf

                If IsWiwe(device.DeviceName, hNap) Then
                    intWIWECount = intWIWECount + 1
                    sText = hMac
                    If IsMACInTable("WIWEdevices.s3db", hMac) Then
                        fullText = fullText & "WIWE az adatbázisban" & vbCrLf
                    Else
                        fullText = fullText & "Új WIWE" & vbCrLf
                        Call InsertWIWEData("WIWEdevices.s3db", hMac, device.DeviceName)
                        Call PrintZPL(hMac, 1)
                    End If
                Else
                    fullText = fullText & "nem WIWE" & vbCrLf
                End If
            Next
            If intWIWECount = 0 Then
                Form1.TextBoxWIWE.Text = "Nincs WIWE"
            End If
            If intWIWECount = 1 Then
            End If
            If intWIWECount > 1 Then

            End If
            Form1.TextBoxInfo.Text = fullText
            If sText = vbNullString Then
                Form1.TextBoxWIWE.Text = "Nincs WIWE"
            Else
                Form1.TextBoxWIWE.Text = sText
            End If

        Catch ex As Exception
            If ex.Message.Contains("No supported Bluetooth protocol stack found") Then
                MessageBox.Show("Nincs bekapcsolva a Bluetooth")
            End If
        End Try


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
