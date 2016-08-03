'http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
Module SQLiteTL
    Public Sub ListTableContent(inputFile As String)
        Try
            Dim db = New SQLiteDatabase(inputFile)

            Dim resultTable As DataTable
            Dim query As [String] = "select * from Devices"
            resultTable = db.GetDataTable(query)
            ' The results can be directly applied to a DataGridView control
            '
            '// Or looped through for some other reason
            For Each r As DataRow In resultTable.Rows
                Console.WriteLine(r("DeviceMAC").ToString() & ", " & r("DeviceName").ToString() & ", " & r("TimeStamp").ToString())
            Next
        Catch fail As Exception
            Dim [error] As [String] = "Hiba ListTableContent:" & vbLf & vbLf
            [error] += fail.Message.ToString() + vbLf & vbLf
            Console.WriteLine([error])
            'MessageBox.Show([error])
        End Try

    End Sub
    Public Function IsMACInTable(inputFile As String, device_mac As String) As Boolean
        Dim res As Boolean

        res = False
        Try
            Dim db = New SQLiteDatabase(inputFile)

            Dim resultTable As DataTable
            Dim query As [String] = "select DeviceMAC from Devices"
            resultTable = db.GetDataTable(query)

            For Each r As DataRow In resultTable.Rows
                If r("DeviceMAC").ToString() = device_mac Then
                    res = True
                End If
            Next
        Catch fail As Exception
            Dim [error] As [String] = "Hiba IsMACInTable:" & vbLf & vbLf
            [error] += fail.Message.ToString() + vbLf & vbLf
            Console.WriteLine([error])
            'MessageBox.Show([error])
        End Try
        Console.WriteLine("IsMACInTable Devices table: " & device_mac & ": " & res)
        Return res
    End Function

    Public Sub InsertWIWEData(inputFile As String, device_mac As String, device_name As String)
        Dim db = New SQLiteDatabase(inputFile)
        Dim data As New Dictionary(Of [String], [String])()
        data.Add("DeviceMAC", device_mac)
        data.Add("DeviceName", device_name)
        data.Add("TimeStamp", Now)
        Try
            db.Insert("Devices", data)
            Console.WriteLine("InsertWIWEData Devices table: " & device_name & "- " & device_mac)
        Catch crap As Exception
            MessageBox.Show(crap.Message)
        End Try

    End Sub
End Module
