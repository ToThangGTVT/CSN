Public Class option_frm
    Public Property cbo_input As MainWindow

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)

    End Sub
    'nút cancel
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        cbo_chat_luong.Items.Add("32 kbps")
        cbo_chat_luong.Items.Add("128 kbps")
        cbo_chat_luong.Items.Add("320 kbps")
        cbo_chat_luong.Items.Add("500 kbps")
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    'nút OK
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
