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
        LabelStatus.Text = "Keresés..."
        TextBoxInfo.Text = vbNullString
        TextBoxWIWE.Text = vbNullString
        'trd = New Thread(AddressOf ThreadTask)
        'trd.IsBackground = True
        'Debug.Print("Thread starts: " & Now)
        'trd.Start()
        Call ThreadTask()
    End Sub

    Private Sub ButtonArchive_Click(sender As Object, e As EventArgs) Handles ButtonArchive.Click
        Call ArchiveDatabase()
    End Sub
    Private Sub ThreadTask()
        'A BT kezelés külön threadben legyen, különben elsőbbséget élvez
        'és nem enged a futás végéig semmit csinálni
        Dim bc As New InTheHand.Net.Sockets.BluetoothClient()
        Dim deviceArray As InTheHand.Net.Sockets.BluetoothDeviceInfo() = bc.DiscoverDevices()
        Dim count As Integer = deviceArray.Length
        Dim sText As String
        Dim fullText As String
        Dim hNap As String
        Dim hSap As String
        Dim hMac As String
        Dim intWIWECount As Integer
        sText = vbNullString
        fullText = vbNullString
        intWIWECount = 0
        Debug.Print("Devices: " & count)
        For i As Integer = 0 To count - 1
            hNap = Hex(deviceArray(i).DeviceAddress.Nap)
            hSap = Hex(deviceArray(i).DeviceAddress.Sap)
            hMac = hNap & hSap

            fullText = GetFullText(deviceArray(i).DeviceName, hNap, hSap)

            Debug.Print(fullText)
            Me.SetScanText(fullText)
            If IsWiwe(deviceArray(i).DeviceName, hNap) Then
                intWIWECount += intWIWECount
            End If
        Next
        Debug.Print("WIWEs found: " & intWIWECount)
        Me.SetLabelText("")

    End Sub

    Private Function GetFullText(device_name As String, device_nap As String, device_sap As String) As String
        Dim res As String
        res = "---" & Now.ToString & "-----------------------" & vbCrLf
        res = res & device_name & vbCrLf
        res = res & device_nap & device_sap & vbCrLf
        Return res
    End Function
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




    'Threadsafe control handling
    Delegate Sub SetTextCallback([text] As String)
    Delegate Sub SetLabelTextCallback([text] As String)
    Private Sub SetScanText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.TextBoxInfo.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetScanText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.TextBoxInfo.Text = [text]
        End If
    End Sub
    Private Sub SetLabelText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.LabelStatus.InvokeRequired Then
            Dim d As New SetLabelTextCallback(AddressOf SetLabelText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.LabelStatus.Text = [text]
        End If
    End Sub
End Class
