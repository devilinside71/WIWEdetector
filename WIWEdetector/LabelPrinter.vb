Module LabelPrinter
    Public Sub PrintZPL(device_mac As String, pld As Integer)
        Dim s As String
        Dim pd As New PrintDialog()

        s = labelcodes(0)
        s = s.Replace("VONALKOD", device_mac)
        s = s.Replace("LABELQTY", Trim(CStr(pld)))

        Console.WriteLine(s)

        ' Open the printer dialog box, and then allow the user to select a printer.
        'res = ZebraPrint.SendStringToPrinter(printerwinnames(0), s)

    End Sub
End Module
