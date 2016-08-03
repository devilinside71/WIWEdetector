Module LabelPrinter
    ''' <summary>
    ''' Prints label.
    ''' Replaces variables with parameters
    ''' </summary>
    ''' <param name="device_mac">Device MAC address</param>
    ''' <param name="qty">Quantity of labels</param>
    Public Sub PrintZPL(device_mac As String, qty As Integer)
        Dim s As String
        Dim pd As New PrintDialog()
        Dim res

        s = labelcodes(0)
        s = s.Replace("VONALKOD", device_mac)
        s = s.Replace("LABELQTY", Trim(CStr(qty)))

        'Console.WriteLine(s)
        'Debug.Print(s)
        ' Open the printer dialog box, and then allow the user to select a printer.
        res = ZebraPrint.SendStringToPrinter(printerwinnames(0), s)

    End Sub
End Module
