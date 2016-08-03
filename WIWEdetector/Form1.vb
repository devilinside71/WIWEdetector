Imports System.Threading
Public Class Form1
    Private trd As Thread
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources.wiwe_06x01bt_Awa_2
        Call LoadZPLSamples()
        Call LoadPrinters()
    End Sub

    Private Sub TextBoxInfo_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxInfo.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBoxInfo.SelectAll()
        End If
    End Sub

    Private Sub ButtonScan_Click(sender As Object, e As EventArgs) Handles ButtonScan.Click
        ButtonScan.Text = "Keresés..."
        TextBoxInfo.Text = vbNullString
        TextBoxWIWE.Text = vbNullString
        ButtonScan.BackColor = SystemColors.Highlight
        trd = New Thread(AddressOf ThreadTask)
        trd.Priority = ThreadPriorityLevel.Normal
        trd.IsBackground = True
        Console.WriteLine("Scanning Thread starts: " & Now)
        trd.Start()
    End Sub

    Private Sub ButtonArchive_Click(sender As Object, e As EventArgs) Handles ButtonArchive.Click
        Call ArchiveDatabase()
    End Sub

    Private Sub TextBoxInfo_TextChanged(sender As Object, e As EventArgs) Handles TextBoxInfo.TextChanged
        ButtonScan.Focus()
    End Sub

    Private Sub ButtonPrinterTest_Click(sender As Object, e As EventArgs) Handles ButtonPrinterTest.Click
        Call LabelPrinter.PrintZPL("TESZT12345678", 1)
    End Sub


    Private Sub ThreadTask()
        'Must be within this class
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
                strInfoText = strInfoText & "---" & Now & "---" & vbCrLf
                strInfoText = strInfoText & device.DeviceName & vbCrLf
                strInfoText = strInfoText & strMac & vbCrLf
                strInfoText = strInfoText & device.RSSI & vbCrLf

                If IsWiwe(device.DeviceName, strNap) Then

                    strWIWEMac = strMac
                    strWIWEName = device.DeviceName
                    If IsMACInTable("WIWEdevices.s3db", strMac) = False Then
                        intWIWECount = intWIWECount + 1
                        strInfoText = strInfoText & "Új WIWE" & vbCrLf
                    Else
                        strInfoText = strInfoText & "WIWE már az adatbázisban" & vbCrLf
                    End If
                Else
                    strInfoText = strInfoText & "Nem WIWE" & vbCrLf
                End If
            Next
            Console.WriteLine(strInfoText)

            If intWIWECount = 0 Then
                Call SetButtoncolor(SystemColors.Control)
                Call SetWIWEText("Nincs új WIWE")
            End If
            If intWIWECount = 1 Then
                Call SetButtoncolor(Color.MediumSeaGreen)
                Call ProcessNewWIWE(strWIWEName, strWIWEMac)
            End If
            If intWIWECount > 1 Then
                Call SetButtoncolor(Color.Crimson)
                Call SetWIWEText("Több új WIWE eszköz érzékelve")
            End If
            Call SetInfoText(strInfoText)


        Catch ex As Exception
            If ex.Message.Contains("No supported Bluetooth protocol stack found") Then
                Call SetButtoncolor(SystemColors.Control)
                MessageBox.Show("Nincs bekapcsolva a Bluetooth", "FIGYELEM!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
        Call SetButtonText("START")

    End Sub
    ''' <summary>
    ''' Execute database and printing operations
    ''' </summary>
    ''' <param name="wiwe_name">WIWE device name</param>
    ''' <param name="wiwe_mac">WIWE BT MAC address</param>
    Private Sub ProcessNewWIWE(wiwe_name As String, wiwe_mac As String)
        Call SetWIWEText(wiwe_mac)
        Call InsertWIWEData("WIWEdevices.s3db", wiwe_mac, wiwe_name)
        Call PrintZPL(wiwe_mac, 1)
    End Sub
    ''' <summary>
    ''' Determine if device is a WIWE based on device name and chipset manufacturer
    ''' </summary>
    ''' <param name="device_name">WIWE device nama</param>
    ''' <param name="device_nap">Chipset manufacturer NAP code</param>
    ''' <returns>True if device is a WIWE</returns>
    Private Function IsWiwe(device_name As String, device_nap As String) As Boolean
        'Ellenőrzi, hogy WIWE-e
        '1., WIWE a kezdete a névnek
        '2., B0B4 a BT chip gyártója
        Dim res As Boolean

        res = False
        If device_name.StartsWith("WIWE") And device_nap = "B0B4" Then
            res = True
        End If

        Return res
    End Function

    'Threadsafe control handling, must be within this class
    Delegate Sub SetWIWETextCallback([text] As String)
    Delegate Sub SetInfoTextCallback([text] As String)
    Delegate Sub SetButtonTextCallback([text] As String)
    Delegate Sub SetButtonColorCallback([col] As Color)
    Private Sub SetWIWEText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.TextBoxWIWE.InvokeRequired Then
            Dim d As New SetWIWETextCallback(AddressOf SetWIWEText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.TextBoxWIWE.Text = [text]
        End If
    End Sub
    Private Sub SetInfoText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.TextBoxInfo.InvokeRequired Then
            Dim d As New SetInfoTextCallback(AddressOf SetInfoText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.TextBoxInfo.Text = [text]
        End If
    End Sub
    Private Sub SetButtonText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.ButtonScan.InvokeRequired Then
            Dim d As New SetButtonTextCallback(AddressOf SetButtonText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.ButtonScan.Text = [text]
        End If
    End Sub
    Private Sub SetButtoncolor(ByVal [col] As Color)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.ButtonScan.InvokeRequired Then
            Dim d As New SetButtonColorCallback(AddressOf SetButtoncolor)
            Me.Invoke(d, New Object() {[col]})
        Else
            Me.ButtonScan.BackColor = [col]
        End If
    End Sub
End Class
