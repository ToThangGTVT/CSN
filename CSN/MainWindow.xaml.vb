Imports xNet
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Windows.Threading
Imports Microsoft.WindowsAPICodePack.Taskbar
Imports System.Activities
Imports System.Threading

Class MainWindow
    Dim s0 As String
    Dim table() 'link+tên bài hát+ tên ca sĩ
    Dim table_dexuat() As String
    Dim titleBH(0) As String  'tên bài hát
    Dim titleBH_dexuat(0) As String  'tên bài hát đề xuất
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
    Dim tab_dangchon As Integer = 1

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
        Dim index As String = http.Get("http://chiasenhac.vn").ToString
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
                ReDim Preserve titleBH(ix)
                titleBH(ix) = tenBH
            End If
        Next
        hoan_thanh_getsource = True
    End Sub

    'lấy URL cửa web có trình phát nhạc vơi bài hát được chỉ định
    Function lay_URL(s As String)
        On Error Resume Next
        Dim url As String
        Dim k
        k = Regex.Match(s0, "<a href=""(.*?)"" title=""", RegexOptions.Singleline)
        url = "https://chiasenhac.vn/" + k.Groups(1).Value
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
        Dim chieu_dai_ten_casi As Integer
        Dim ten_tam As String = Nothing
        If Regex.Match(table(i).ToString, ";").ToString <> Nothing Then
            Dim match = Regex.Match(table(i).ToString, "<div class=""author(.*?)</div>")
            Dim match2 As String = Regex.Match(match.ToString, "><a href=""(.*?)</div>", RegexOptions.Singleline).Groups(1).Value
            For Each match3 In Regex.Matches(match2.ToString, ">(.*?)</a>")
                ten_casi = ten_casi + match3.Value.ToString.Replace("</a>", "").Replace(">", "") + "; " '.Substring(2, ten_casi.Length - 6)
            Next
            ten_tam = ten_casi
            chieu_dai_ten_casi = ten_casi.Length
            ten_casi = ten_tam.Substring(0, chieu_dai_ten_casi - 2)
        Else
            ten_casi = Regex.Match(table(i).ToString, "html"">(.*?)</a></div>").Groups(1).Value
            If ten_casi = Nothing Then
                Dim match = Regex.Match(table(i).ToString, "<div class=""author(.*?)</div>")
                Dim match2 As String = Regex.Match(match.ToString, "><a href=""(.*?)</div>", RegexOptions.Singleline).Groups(1).Value
                ten_casi = Regex.Match(table(i).ToString, "ca-si"">(.*?)</a></div>").Groups(1).Value
            End If
        End If
        Return ten_casi
    End Function

    Private Sub lst_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst.SelectionChanged
        tab_dangchon = 2
        'nhan_tim_kiem = True
        nhan_listbox()
    End Sub

    Sub nhan_listbox()
        On Error Resume Next
        rtb.Document.Blocks.Clear()
        Dim Stitle As String = Nothing
        Dim http As New HttpRequest
        Dim k
        Dim objList As Object
        Dim index1 As String
        Select Case tab_dangchon
            Case 1
                objList = lst_BXH
                s0 = table(lst_BXH.SelectedIndex + 1).ToString
                'Stitle = titleBH(lst_BXH.SelectedIndex + 1)
                index1 = http.Get(lay_URL(s0)).ToString
                txt_sus.Content = lst_BXH.SelectedItem.ToString
            Case 2
                objList = lst
                s0 = ds_url_search(lst.SelectedIndex + 1).ToString.Replace("mp3.chiasenhac.vn", "chiasenhac.vn")
                'Stitle = ds_title_search(lst.SelectedIndex + 1)
                index1 = http.Get(s0).ToString
                txt_sus.Content = lst.SelectedItem.ToString
            Case 3
                objList = lst_dexuat
                s0 = table_dexuat(lst_dexuat.SelectedIndex + 1).ToString
                index1 = http.Get(lay_URl_dexuat(s0)).ToString
                txt_sus.Content = lst_dexuat.SelectedItem.ToString
        End Select

        Dim s10 As String = "<div id=""fulllyric"">(.*?)</div>"
        k = Regex.Match(index1, s10, RegexOptions.Singleline)
        Dim lyric As String
        lyric = k.Groups(1).Value.ToString.Replace("<br />", "").Replace("<span style=""font-size: 10%; line-height: 1px; color: #EEFFFF;"">" & Stitle & " lyrics on ChiaSeNhac.vn</span>", "").Replace("<span class=""lyric_translate1"">", "").Replace("</span>", "").Replace("&quot;", """").Trim
        Dim prgap As New Paragraph()
        prgap.Inlines.Add(lyric)
        rtb.Document.Blocks.Add(prgap)
        Dim s11 As String = "{""file"": ""(.*?)"", ""label"": ""32kbps"","
        Dim k1
        k1 = Regex.Match(index1, s11, RegexOptions.Singleline)
        Dim wcl As New WebClient

        play_media(k1.Groups(1).Value.ToString)

        Hienthidexuat(index1)

        'ghi lịch sử phát nhạc
        Dim lich_su() As String = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "history.txt")
        Dim ghi_lich_su_b As Boolean = True
        For i = LBound(lich_su) To UBound(lich_su)
            If objList.SelectedItem.ToString = lich_su(i) Then
                ghi_lich_su_b = False
                Exit For
            End If
        Next
        If ghi_lich_su_b = True Then
            ghi_lich_su(objList.SelectedItem.ToString)
        End If

        cbo_input.Items.Add(objList.SelectedItem.ToString)
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
            Dim current As Integer = Integer.Parse(Replace(wmp.Ctlcontrols.currentPositionString, ":", ""))
            Dim total As Integer = Integer.Parse(Replace(wmp.currentMedia.durationString, ":", ""))
            Dim percent = current / total
            showTaskProgress(percent * 100)
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
        tab_dangchon = 1
        'nhan_tim_kiem = False
        nhan_listbox()
    End Sub

    Sub Hienthidexuat(index As String)
        Dim pattern As String = "<li class=""media align-items-stretch"">(.*?)</li>"
        Dim ix As Integer
        For Each match In Regex.Matches(index, pattern, RegexOptions.Singleline)
            Dim tenBH As String = Regex.Match(match.ToString, "title=""(.*?)"">").Groups(1).Value
            If tenBH <> "" Then
                ix += 1
                ReDim Preserve table_dexuat(ix)
                table_dexuat(ix) = match.ToString
                Dim tenCS As String = lay_ten_CS_dexuat(ix)
                lst_dexuat.Items.Add(tenBH + " - " + tenCS)
                ReDim Preserve titleBH_dexuat(ix)
                titleBH_dexuat(ix) = tenBH
            End If
        Next
        lst_dexuat.Items.RemoveAt(11)
        lst_dexuat.Items.RemoveAt(10)
    End Sub

    Function lay_ten_CS_dexuat(i As Integer) As String
        Dim ten_casi As String = Nothing
        Dim chieu_dai_ten_casi As Integer
        Dim ten_tam As String = Nothing
        If Regex.Match(table_dexuat(i).ToString, ";").ToString <> Nothing Then
            Dim match = Regex.Match(table_dexuat(i).ToString, "<div class=""author(.*?)</div>")
            Dim match2 As String = Regex.Match(match.ToString, "><a href=""(.*?)</div>", RegexOptions.Singleline).Groups(1).Value
            For Each match3 In Regex.Matches(match2.ToString, ">(.*?)</a>")
                ten_casi = ten_casi + match3.Value.ToString.Replace("</a>", "").Replace(">", "") + "; " '.Substring(2, ten_casi.Length - 6)
            Next
            ten_tam = ten_casi
            chieu_dai_ten_casi = ten_casi.Length
            ten_casi = ten_tam.Substring(0, chieu_dai_ten_casi - 2)
        Else
            ten_casi = Regex.Match(table_dexuat(i).ToString, "html"">(.*?)</a></div>").Groups(1).Value
            If ten_casi = Nothing Then
                Dim match = Regex.Match(table_dexuat(i).ToString, "<div class=""author(.*?)</div>")
                Dim match2 As String = Regex.Match(match.ToString, "><a href=""(.*?)</div>", RegexOptions.Singleline).Groups(1).Value
                ten_casi = Regex.Match(table_dexuat(i).ToString, "ca-si"">(.*?)</a></div>").Groups(1).Value
            End If
        End If
        Return ten_casi
    End Function

    Private Sub Lst_dexuat_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst_dexuat.SelectionChanged
        tab_dangchon = 3
        nhan_listbox()
    End Sub

    Function lay_URl_dexuat(index As String)
        Dim pattern As String = "chiasenhac(.*?)html"
        Return "http://" + Regex.Match(index, pattern).Value.ToString
    End Function

    Private Sub showTaskProgress(value As Integer)
        Dim max As Integer = 100
        Dim prog = TaskbarManager.Instance
        prog.SetProgressState(TaskbarProgressBarState.Normal)
        'For i As Integer = 0 To max - 1
        prog.SetProgressValue(value, max)
        'Thread.Sleep(100)
        'Next
        'prog.SetProgressState(TaskbarProgressBarState.NoProgress)
    End Sub
End Class