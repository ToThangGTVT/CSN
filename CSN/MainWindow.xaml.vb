﻿Imports xNet
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Windows.Threading

Class MainWindow
    Dim s0 As String
    Dim table() 'link+tên bài hát+ tên ca sĩ
    Dim titleBH() As String  'tên bài hát
    Dim ti As Single
    Dim timer As DispatcherTimer = New DispatcherTimer()
    Dim nhan_tim_kiem As Boolean = False
    Dim URL_search
    Dim ds_url_search() As String
    Dim ds_title_search() As String
    Dim hoan_thanh_getsource As Boolean = False
    Dim titleForm As String
    Dim repeat As Boolean = False
    Dim thaydoi As Boolean = False

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)
        On Error Resume Next
        'khai báo tệp lịch sử
        If (Not File.Exists(AppDomain.CurrentDomain.BaseDirectory + "history.txt")) Then
            File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "history.txt")
        End If
        'load lịch sử phát nhạc
        Dim lich_su() As String = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "history.txt")
        Dim i As Integer
        For i = LBound(lich_su) To UBound(lich_su)
            cbo_input.Items.Add(lich_su(i))
        Next
        Dim AX As AxWMPLib.AxWindowsMediaPlayer = TryCast(winform.Child, AxWMPLib.AxWindowsMediaPlayer)
        'craw data để lấy danh sách bài hát
        Dim http As New HttpRequest
        Dim index As String = http.Get("http://beta.chiasenhac.vn").ToString
        Dim pattern As String = "<li class=""media (.*?)</li>"
        Dim ix As Integer 'chỉ số phần tử listbox bảng xếp hạng
        For Each match In Regex.Matches(index, pattern, RegexOptions.Singleline)
            Dim tenBH As String = Regex.Match(match.ToString, "title=""(.*?)""><img").Groups(1).Value
            If tenBH <> "" Then
                ix += 1
                ReDim Preserve table(ix)
                table(ix) = match
                Dim tenCS As String = lay_ten_CS(ix)
                lst_BXH.Items.Add(tenBH + " - " + tenCS)

            End If
            titleBH(ix) = tenBH
        Next
        hoan_thanh_getsource = True
    End Sub
    'lấy URL cửa web có trình phát nhạc vơi bài hát được chỉ định
    Function lay_URL(s As String)
        On Error Resume Next
        Dim url As String
        Dim k
        k = Regex.Match(s0, "<a href=""(.*?)"" title=""", RegexOptions.Singleline)
        url = "https://beta.chiasenhac.vn/" + k.Groups(1).Value
        Return url
    End Function
    'lấy URL của web có đường dẫn download vơi bài hát được chỉ định
    Function lay_URL_DL(s As String)
        On Error Resume Next
        Dim url As String
        Dim k
        k = Regex.Match(s, "<a href=""http://chiasenhac.vn(.*?)"" target=""_blank", RegexOptions.Singleline)
        url = k.Groups(1).Value
        Return "http://chiasenhac.vn" + url
    End Function
    Function lay_ten_CS(i As Integer) As String
        Dim ten_casi As String = Nothing
        If Regex.Match(table(i).ToString, ";").ToString <> Nothing Then
            Dim match = Regex.Match(table(i).ToString, "<div class=""author(.*?)</div>")
            Dim match2 As String = Regex.Match(match.ToString, "><a href=""(.*?)</div>", RegexOptions.Singleline).Groups(1).Value
            For Each match3 In Regex.Matches(match2.ToString, ">(.*?)</a>")
                ten_casi = ten_casi + match3.Value.ToString.Replace("</a>", "").Replace(">", "") + " ;"
            Next
            ten_casi = ten_casi.Substring(0, Len(ten_casi) - 1)
            Dim iy As Integer
            Debug.Print(iy)
        Else
            ten_casi = Regex.Match(table(i).ToString, "html"">(.*?)</a></div>").Groups(1).Value
        End If
        Return ten_casi
    End Function
    Private Sub lst_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst.SelectionChanged
        nhan_listbox()
    End Sub
    Sub nhan_listbox()
        On Error Resume Next
        rtb.Document.Blocks.Clear()
        Dim Stitle As String
        txt_sus.Content = lst.SelectedItem.ToString
        Dim http As New HttpRequest
        Dim k

        Dim index1 As String
        If nhan_tim_kiem = False Then
            s0 = table(lst_BXH.SelectedIndex + 1).ToString
            Stitle = Title(lst_BXH.SelectedIndex + 1)
            index1 = http.Get(lay_URL(s0)).ToString
        Else
            s0 = ds_url_search(lst.SelectedIndex + 1).ToString.Replace("mp3.chiasenhac.vn", "beta.chiasenhac.vn")
            Stitle = ds_title_search(lst.SelectedIndex + 1)
            index1 = http.Get(s0).ToString
        End If

        Dim s10 As String = "<div id=""fulllyric"">(.*?)</div>"
        k = Regex.Match(index1, s10, RegexOptions.Singleline)
        Dim lyric As String
        lyric = k.Groups(1).Value.ToString.Replace("<br />", "").Replace("<span style=""font-size: 10%; line-height: 1px; color: #EEFFFF;"">" & Stitle & " lyrics on ChiaSeNhac.vn</span>", "").Replace("<span class=""lyric_translate1"">", "").Replace("</span>", "").Replace("&quot;", """").Trim
        Debug.Print(lyric)
        Dim prgap As New Paragraph()
        prgap.Inlines.Add(lyric)
        rtb.Document.Blocks.Add(prgap)
        Dim s11 As String = "{""file"": ""(.*?)"", ""label"": ""32kbps"","
        Dim k1
        k1 = Regex.Match(index1, s11, RegexOptions.Singleline)
        Dim wcl As New WebClient

        If (Not Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "song/")) Then
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "song/")
        End If
        Dim song As String = AppDomain.CurrentDomain.BaseDirectory + "song/" + lst.SelectedItem.ToString + ".mp3"

        play_media(k1.Groups(1).Value.ToString)

        'ghi lịch sử phát nhạc
        Dim lich_su() As String = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "history.txt")
        Dim ghi_lich_su_b As Boolean = True
        For i = LBound(lich_su) To UBound(lich_su)
            If lst.SelectedItem.ToString = lich_su(i) Then
                ghi_lich_su_b = False
                Exit For
            End If
        Next
        If ghi_lich_su_b = True Then
            ghi_lich_su(lst.SelectedItem.ToString)
        End If

        cbo_input.Items.Add(lst.SelectedItem.ToString)
    End Sub
    Sub play_media(url As String)
        wmp.URL = url
        Dim wmpt As WMPLib.WindowsMediaPlayer = New WMPLib.WindowsMediaPlayer
        Dim media As WMPLib.IWMPMedia = wmpt.newMedia(url)

        Dim timerVideoTime = New DispatcherTimer()
        timerVideoTime.Interval = TimeSpan.FromSeconds(0)
        AddHandler timerVideoTime.Tick, AddressOf Me.timeTick
        timerVideoTime.Start()
    End Sub
    Public Sub timeTick(ByVal o As Object, ByVal sender As EventArgs)
        On Error Resume Next
        txtreal.Content = wmp.Ctlcontrols.currentPositionString
        If wmp.playState = 3 Then
            txttotal.Content = wmp.currentMedia.durationString
        End If
        If txtreal.Content = txttotal.Content And chk.IsChecked = False Then
            lst_BXH.SelectedIndex += 1
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        On Error Resume Next
        wmp.Ctlcontrols.play()
        txt_sus.Content = lst.SelectedItem + " [Playing...]"
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        On Error Resume Next
        wmp.Ctlcontrols.pause()
        txt_sus.Content = lst.SelectedItem + " [Stoped]"
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        On Error Resume Next
        tab_control.SelectedItem = tab_timkiem
        lst.UnselectAll()
        ReDim ds_title_search(0)
        ReDim ds_url_search(0)

        If txts.Text = "" Then
            Exit Sub
        End If

        Dim http As New HttpRequest
        Dim index As String = http.Get("http://search.chiasenhac.vn/search.php?s=" + txts.Text.Replace(" ", "+")).ToString
        Dim pattern As String = "<tr title=""(.*?)</tr>"
        Dim pattern_url As String = "<a href=""(.*?)"" class"
        Dim pattern_tenBH As String = "target=""_top"">(.*?)</a"
        Dim pattern_tenCS As String = "</a></p>(.*?)</p>"
        Dim tenBH
        Dim tenCS

        lst.Items.Clear()
        Dim i As Integer = 0
        For Each match In Regex.Matches(index, pattern, RegexOptions.Singleline)
            URL_search = Regex.Match(match.ToString, pattern_url, RegexOptions.Singleline)
            URL_search = URL_search.Groups(1).Value
            tenBH = Regex.Match(match.ToString, pattern_tenBH, RegexOptions.Singleline)
            tenBH = tenBH.Groups(1).Value
            tenCS = Regex.Match(match.ToString, pattern_tenCS, RegexOptions.Singleline)
            tenCS = tenCS.Groups(1).Value.Replace(vbTab, "").Replace("<p>", "")
            tenCS = Regex.Split(tenCS, "\n|[ ]{2,}", RegexOptions.Singleline)
            tenCS = tenCS(1).ToString
            If tenCS <> "</td>" Then
                i += 1
                lst.Items.Add(tenBH & " - " & tenCS)
                ReDim Preserve ds_url_search(i)
                ds_url_search(i) = URL_search
                ReDim Preserve ds_title_search(i)
                ds_title_search(i) = tenBH
            End If
        Next
        nhan_tim_kiem = True
    End Sub
    Private Sub chk_Checked(sender As Object, e As RoutedEventArgs) Handles chk.Checked
        If chk.IsChecked = True Then
            repeat = True
        End If
    End Sub

    Public Property MyText() As String
        Get
            Return titleForm
        End Get
        Set(ByVal value As String)
            titleForm = value
        End Set
    End Property

    Private Sub wmp_StatusChange(sender As Object, e As EventArgs) Handles wmp.StatusChange
        If wmp.playState = WMPLib.WMPPlayState.wmppsStopped And repeat = True Then
            wmp.Ctlcontrols.play()
        End If
    End Sub
    Sub stt()
        If wmp.playState = WMPLib.WMPPlayState.wmppsStopped Then
            MsgBox("OK")
        End If
    End Sub

    Private Sub scrl_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles scrl.ValueChanged
        wmp.Ctlcontrols.currentPosition = scrl.Value
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim abo As New abo_frm
        abo.ShowDialog()
    End Sub

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub MenuItem_Click_2(sender As Object, e As RoutedEventArgs)
        End
    End Sub

    Private Sub MenuItem_Click_3(sender As Object, e As RoutedEventArgs) 'nút tùy chọn
        Dim tc As New option_frm
        tc.ShowDialog()
    End Sub

    Sub ghi_lich_su(text As String)
        Dim f As StreamWriter
        f = My.Computer.FileSystem.OpenTextFileWriter(AppDomain.CurrentDomain.BaseDirectory + "history.txt", True)
        f.WriteLine(text)
        f.Close()
    End Sub

    Sub xoa_cbo()
        Me.cbo_input.Items.Clear()

    End Sub
    'xóa lịch sử
    Private Sub MenuItem_Click_5(sender As Object, e As RoutedEventArgs)
        If MsgBox("Bạn chắc chắn muốn xóa", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            My.Computer.FileSystem.DeleteFile(AppDomain.CurrentDomain.BaseDirectory + "history.txt")
            cbo_input.Items.Clear()
        End If
    End Sub

    Private Sub MenuItem_Click_4(sender As Object, e As RoutedEventArgs)
        Dim webAddress As String = "https://www.facebook.com/tokun.nb"
        Process.Start(webAddress)
    End Sub

    Private Sub MenuItem_Click_6(sender As Object, e As RoutedEventArgs)
        MsgBox("Bạn tốt vl nhưng tác giả méo cần đâu" + vbNewLine + "ahihi")
    End Sub
    'nút check for update
    Private Sub MenuItem_Click_7(sender As Object, e As RoutedEventArgs)
        Dim f_update As New update_v
        f_update.ShowDialog()
    End Sub

    Private Sub lst_BXH_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst_BXH.SelectionChanged
        nhan_listbox()
    End Sub
End Class