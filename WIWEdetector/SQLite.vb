Module SQLite
    Public Function IsInSQLDatabase(device_mac As String) As Boolean
        Dim res As Boolean

        res = False

        Return res
    End Function
    Public Sub WriteToSQLDatabase(device_name, device_mac)
        Dim dtmTimeStamp As DateTime

        dtmTimeStamp = Now
        'Debug
        Console.WriteLine("WriteToSQLDatabase:")
        Console.WriteLine(device_name & " - " & device_mac & ": " & dtmTimeStamp)
    End Sub
End Module
