Imports System.IO

Module Archive
    Public Sub ArchiveDatabase()
        Dim timeStamp As String = DateTime.Now.ToString("yyyyMMddhhmmss")
        Dim dt As DateTime = DateTime.ParseExact(timeStamp, "yyyyMMddhhmmss", Nothing)
        Dim strTimeStamp As String
        Dim newFileName As String
        Dim path As String = Directory.GetCurrentDirectory()

        strTimeStamp = dt.ToString
        strTimeStamp = strTimeStamp.Replace(".", "")
        strTimeStamp = strTimeStamp.Replace(":", "")
        strTimeStamp = strTimeStamp.Replace(" ", "_")

        newFileName = path & "\Arch\WIWEdevices_" & strTimeStamp & ".s3db"

        Console.WriteLine(newFileName)
        File.Copy("WIWEdevices.s3db", newFileName, True)
    End Sub
End Module
