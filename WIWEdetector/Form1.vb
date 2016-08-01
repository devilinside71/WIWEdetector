Imports System.Threading
Public Class Form1
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
        Call DetectBT()
    End Sub

    Private Sub ButtonArchive_Click(sender As Object, e As EventArgs) Handles ButtonArchive.Click
        Call ArchiveDatabase()
    End Sub
End Class
