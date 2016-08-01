Module BTDetector
    Sub DetectBT()

        Try
            Dim devices As New List(Of BTDeviceInfo)()
            Dim bc As New InTheHand.Net.Sockets.BluetoothClient()
            Dim devicesArray As InTheHand.Net.Sockets.BluetoothDeviceInfo() = bc.DiscoverDevices()
            Dim devicesCount As Integer = devicesArray.Length
            Dim strInfoText As String
            Dim strNap As String
            Dim strSap As String
            Dim strMac As String
            Dim intWIWECount As Integer
            Dim strWIWEMac As String
            Dim strWIWEName As String

            strInfoText = vbNullString
            strWIWEName = vbNullString
            strWIWEMac = vbNullString
            intWIWECount = 0
            For i As Integer = 0 To devicesCount - 1
                Dim device As New BTDeviceInfo(devicesArray(i))
                strNap = Hex(device.Nap)
                strSap = Hex(device.Sap)
                strMac = strNap & strSap
                strInfoText = strInfoText & "---" & Now & "------------------------" & vbCrLf
                strInfoText = strInfoText & device.DeviceName & vbCrLf
                strInfoText = strInfoText & strMac & vbCrLf
                strInfoText = strInfoText & device.RSSI & vbCrLf

                If IsWiwe(device.DeviceName, strNap) Then

                    strWIWEMac = strMac
                    strWIWEName = device.DeviceName
                    'sText = hMac
                    If IsMACInTable("WIWEdevices.s3db", strMac) = False Then
                        intWIWECount = intWIWECount + 1
                        strInfoText = strInfoText & "Új WIWE" & vbCrLf
                    Else
                        strInfoText = strInfoText & "WIWE az adatbázisban" & vbCrLf
                        '    Call InsertWIWEData("WIWEdevices.s3db", hMac, device.DeviceName)
                        '    Call PrintZPL(hMac, 1)
                    End If
                Else
                    strInfoText = strInfoText & "nem WIWE" & vbCrLf
                End If
            Next
            Debug.Print(strInfoText)
            If intWIWECount = 0 Then
                Form1.TextBoxWIWE.Text = "Nincs új WIWE"
            End If
            If intWIWECount = 1 Then
                Call ProcessNewWIWE(strWIWEName, strWIWEMac)
            End If
            If intWIWECount > 1 Then
                Form1.TextBoxWIWE.Text = "Több új WIWE eszköz érzékelve"
            End If
            Form1.TextBoxInfo.Text = strInfoText


        Catch ex As Exception
            If ex.Message.Contains("No supported Bluetooth protocol stack found") Then
                MessageBox.Show("Nincs bekapcsolva a Bluetooth")
            End If
        End Try


    End Sub
    Private Sub ProcessNewWIWE(wiwe_name As String, wiwe_mac As String)
        Form1.TextBoxWIWE.Text = wiwe_mac
        Call InsertWIWEData("WIWEdevices.s3db", wiwe_mac, wiwe_name)
        Call PrintZPL(wiwe_mac, 1)
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
