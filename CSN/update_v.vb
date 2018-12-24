Public Class update_v

    Private Sub update_v_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txt_version.Text = My.Application.Info.Version.ToString
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://github.com/ToThangGTVT/UpdateCSN"
        Process.Start(webAddress)
    End Sub
End Class