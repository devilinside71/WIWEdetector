Imports System.IO

Module Archive
    Public Sub ArchiveDatabase()
        Dim timeStamp As String = DateTime.Now.ToString("yyyyMMddhhmmss")
        Dim dt As DateTime = DateTime.ParseExact(timeStamp, "yyyyMMddhhmmss", Nothing)
        Dim strTimeStamp As String
        Dim newFileName As String
        Dim path As String = Directory.GetCurrentDirectory()
        Dim res As Integer

        res = MessageBox.Show("A meglévő adatbázis archiválásra kerül. Üres adatbázist akarsz használni?", "FIGYELEM!", MessageBoxButtons.YesNoCancel)

        If res = DialogResult.Yes Or res = DialogResult.No Then

            strTimeStamp = dt.ToString
            strTimeStamp = strTimeStamp.Replace(".", "")
            strTimeStamp = strTimeStamp.Replace(":", "")
            strTimeStamp = strTimeStamp.Replace(" ", "_")

            newFileName = path & "\Arch\WIWEdevices_" & strTimeStamp & ".s3db"

            Console.WriteLine(newFileName)
            File.Copy("WIWEdevices.s3db", newFileName, True)
            If res = DialogResult.Yes Then
                File.Copy(path & "\Lib\WIWEdevices.s3db", path & "\WIWEdevices.s3db", True)
            End If
        End If
    End Sub
End Module
