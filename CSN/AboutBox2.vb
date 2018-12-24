Public NotInheritable Class abo_frm

    Private Sub AboutBox2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txt_version.Text = My.Application.Info.Version.ToString
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://www.facebook.com/tokun.nb"
        Process.Start(webAddress)
    End Sub
End Class
