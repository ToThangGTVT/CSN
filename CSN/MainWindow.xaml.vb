﻿Imports xNet
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Windows.Threading
Imports System.Threading

Class MainWindow
    Dim s0 As String
    Dim table()
    Dim title() As String
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
        Dim AX As AxWMPLib.AxWindowsMediaPlayer = TryCast(winform.Child, AxWMPLib.AxWindowsMediaPlayer)
        Dim http As New HttpRequest
        Dim index As String = http.Get("http://chiasenhac.vn").ToString
        Dim pattern As String = "<div class=""text2 text2x"">(.*?)</p>"
        Dim pattern2 As String = "title=""(.*?)"""
        Dim i As Integer
        Dim s1 As String = Regex.Match(index, pattern, RegexOptions.Singleline).ToString
        For Each match In Regex.Matches(index, pattern, RegexOptions.Singleline)
            For Each match2 In Regex.Matches(match.ToString, pattern2, RegexOptions.Singleline)
                i += 1
                ReDim Preserve table(i)
                ReDim Preserve title(i)
                table(i) = match
                lst.Items.Add(match2.Groups(1).Value)
                title(i) = Regex.Match(match2.Groups(1).Value.ToString, "(.*?) -", RegexOptions.Singleline).Groups(1).Value.ToString
            Next
        Next
        hoan_thanh_getsource = True
    End Sub

    Function lay_URL(s As String)
        On Error Resume Next
        Dim url As String
        Dim k
        k = Regex.Match(s0, "a href=""(.*?)""", RegexOptions.Singleline)
        url = k.Groups(1).Value
        Return url
    End Function
    Function lay_URL_DL(s As String)
        On Error Resume Next
        Dim url As String
        Dim k
        k = Regex.Match(s, "<a href=""http://chiasenhac.vn(.*?)"" target=""_blank", RegexOptions.Singleline)
        url = k.Groups(1).Value
        Return "http://chiasenhac.vn" + url
    End Function

    Private Sub lst_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lst.SelectionChanged
        On Error Resume Next
        rtb.Document.Blocks.Clear()
        Dim Stitle As String
        txt_sus.Content = lst.SelectedItem.ToString
        Dim http As New HttpRequest
        Dim k
        Dim lyric As String
        Dim index1 As String
        If nhan_tim_kiem = False Then
            s0 = table(lst.SelectedIndex + 1).ToString
            Stitle = title(lst.SelectedIndex + 1)
            index1 = http.Get(lay_URL(s0)).ToString
        Else
            s0 = ds_url_search(lst.SelectedIndex + 1).ToString
            Stitle = ds_title_search(lst.SelectedIndex + 1)
            index1 = http.Get(s0).ToString
        End If
        Dim s10 As String = "<p class=""genmed"" style=""font-size: 13px; overflow: hidden;"">(.*?)</p>"
        k = Regex.Match(index1, s10, RegexOptions.Singleline)
        lyric = k.Groups(1).Value.ToString.Replace("<br />", "").Replace("<span style=""font-size: 10%; line-height: 1px; color: #EEFFFF;"">" & Stitle & " lyrics on ChiaSeNhac.vn</span>", "").Replace("<span class=""lyric_translate1"">", "").Replace("</span>", "")
        Dim prgap As New Paragraph()
        prgap.Inlines.Add(lyric)
        rtb.Document.Blocks.Add(prgap)
        Dim index2 As String = http.Get(lay_URL_DL(index1)).ToString
        Dim s11 As String = "<a href=""http://data(.*?)"" title"
        Dim k1
        k1 = Regex.Match(index2, s11, RegexOptions.Singleline)
        Dim wcl As New WebClient

        If (Not System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "song/")) Then
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "song/")
        End If
        Dim song As String = AppDomain.CurrentDomain.BaseDirectory + "song/" + lst.SelectedItem.ToString + ".mp3"

        If File.Exists(song) = False Then
            wcl.DownloadFile("http://data" + k1.Groups(1).Value.ToString, AppDomain.CurrentDomain.BaseDirectory + "song/" + lst.SelectedItem.ToString + ".mp3")
        End If


        wmp.URL = song

        Dim Duration As String
        Dim wmpt As WMPLib.WindowsMediaPlayer = New WMPLib.WindowsMediaPlayer
        Dim media As WMPLib.IWMPMedia = wmpt.newMedia(song)

        Dim timerVideoTime = New DispatcherTimer()
        timerVideoTime.Interval = TimeSpan.FromSeconds(0)
        AddHandler timerVideoTime.Tick, AddressOf Me.timeTick
        timerVideoTime.Start()
        If media IsNot Nothing Then
            scrl.Maximum = media.durationString
        End If
        txttotal.Content = media.durationString
        scrl.Maximum = media.duration
    End Sub

    Public Sub timeTick(ByVal o As Object, ByVal sender As EventArgs)
        On Error Resume Next
        txtreal.Content = wmp.Ctlcontrols.currentPositionString
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
            'Dim timerVideoTime = New DispatcherTimer()
            'timerVideoTime.Interval = TimeSpan.FromSeconds(0)
            'AddHandler timerVideoTime.Tick, AddressOf Me.timeTick
            'timerVideoTime.Start()
            MsgBox("OK")
        End If

    End Sub

    Private Sub scrl_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles scrl.ValueChanged
        wmp.Ctlcontrols.currentPosition = scrl.Value
    End Sub
End Class
