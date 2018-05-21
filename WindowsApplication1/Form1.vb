Imports WindowsApplication1.NativeWifi
Imports System.Text
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.NetworkInformation
Imports System.Windows.Forms

Public Class Form1
    'Dim WLANList As New WLANList
    Dim WifiGroupList As New List(Of WifiGroup)()
    Dim dt As New DataTable
    Dim groupindex = ""
    Dim str_LocationName = ""
    Dim location_Category = ""
    Dim str_roomID = ""





    Private Function BytesToString(ByVal Input As Byte()) As String
        Dim Result As New StringBuilder(Input.Length * 2)
        Dim Part As String
        For Each b As Byte In Input
            Part = Conversion.Hex(b)
            If Part.Length = 1 Then Part = "0" & Part
            Result.Append(Part)
        Next
        Return Result.ToString()
    End Function

    Private Function BytesToMACString(ByVal bInput As Byte(), Optional ByVal sDelimiter As String = "-") As String
        Dim sResult As New System.Text.StringBuilder(bInput.Length * 2)
        Dim sPart As String
        For i = 0 To bInput.Count - 1
            sPart = Conversion.Hex(bInput(i))
            If sPart.Length = 1 Then sPart = "0" & sPart
            If i = 0 Then
                sResult.Append(sPart)
            Else
                sResult.Append(sDelimiter & sPart)
            End If
        Next
        Return sResult.ToString()
    End Function

    '17,1.16 Get ping value and current connected AP
    Private Function GetPing() As Integer
        Dim ping As New Ping
        Dim pingReply = ping.Send("www.google.com")
        Return pingReply.RoundtripTime
    End Function
    Private Sub GetPingTbRow()
        Dim client As New WlanClient()
        Dim strCurrentAP As String
        Dim p = New Process()
        p.StartInfo.FileName = "cmd.exe"
        p.StartInfo.Arguments = ("/c netsh.exe wlan show interfaces | find " & Chr(34) & "BSSID" & Chr(34)) 'parameters.ToString()
        p.StartInfo.UseShellExecute = False
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.RedirectStandardOutput = True
        p.Start()
        p.WaitForExit(30000)
        strCurrentAP = p.StandardOutput.ReadToEnd()
        strCurrentAP = strCurrentAP.Substring(29)
        strCurrentAP = strCurrentAP.Replace(":", "")
        strCurrentAP = strCurrentAP.ToUpper()
        strCurrentAP = strCurrentAP.Replace(vbCr, "").Replace(vbLf, "")

        For Each wlanIface As WlanClient.WlanInterface In client.Interfaces

            Dim wlanBssEntries As Wlan.WlanBssEntry() = wlanIface.GetNetworkBssList()
            '
            For Each network As Wlan.WlanBssEntry In wlanBssEntries
                ' 
                If BytesToString(network.dot11Bssid) = strCurrentAP Then
                    dt.Rows.Add(groupindex, Encoding.UTF8.GetString(network.dot11Ssid.SSID).Trim(ChrW(0)),
                            BytesToString(network.dot11Bssid),
                            ConvertToChannel(network.chCenterFrequency),
                            network.linkQuality.ToString() + "",
                            network.rssi.ToString() + "",
                            "Ping",
                            Date.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            GetPing(),
                            txtStuID.Text.ToUpper(),
                            txtLocation.Text,
                            str_LocationName,
                            cbxFloor.SelectedItem.ToString(),
                            location_Category,
                            str_roomID
                                )
                    Exit Sub
                End If

            Next
        Next

    End Sub


    Private Sub btnGetWLANList_Click(sender As Object, e As EventArgs) Handles btnGetWLANList.Click
        lstvAvailableNetworkList.Items.Clear()

        Dim client As New WlanClient()


        For Each wlanIface As WlanClient.WlanInterface In client.Interfaces

            Dim wlanAvailableNetworks As Wlan.WlanAvailableNetwork() = wlanIface.GetAvailableNetworkList(0)


            For Each network As Wlan.WlanAvailableNetwork In wlanAvailableNetworks
                ' 
                Dim listItemWiFi As New ListViewItem()
                ' 
                With listItemWiFi
                    .Text = Encoding.UTF8.GetString(network.dot11Ssid.SSID).Trim(ChrW(0))
                    '
                    .SubItems.Add(network.wlanSignalQuality.ToString() + "%")
                    ' 
                    .SubItems.Add(network.dot11BssType.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.dot11DefaultAuthAlgorithm.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.dot11DefaultCipherAlgorithm.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.morePhyTypes.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.numberOfBssids.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.networkConnectable.ToString().Trim(CChar(ChrW(0))))
                    ' 
                    .SubItems.Add(network.flags.ToString().Trim(CChar(ChrW(0))))
                    .SubItems.Add(network.profileName.ToString().Trim(CChar(ChrW(0))))
                    .SubItems.Add(network.securityEnabled.ToString().Trim(CChar(ChrW(0))))
                    .SubItems.Add(network.wlanNotConnectableReason.ToString().Trim(CChar(ChrW(0))))

                    'Wlan.WlanGetNetworkBssList

                End With

                With lstvAvailableNetworkList.Columns
                    .Clear()
                    .Add("SSID")
                    .Add("SignalQuality")
                    .Add("dot11BssType")
                    .Add("dot11DefaultAuthAlgorithm")
                    .Add("dot11DefaultCipherAlgorithm")
                    .Add("morePhyTypes")
                    .Add("numberOfBssids")
                    .Add("networkConnectable")
                    .Add("flags")
                    .Add("profileName")
                    .Add("securityEnabled")
                    .Add("wlanNotConnectableReason")
                End With


                lstvAvailableNetworkList.Items.Add(listItemWiFi)
            Next
        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        lstvAvailableNetworkList.Clear()

        Dim client As New WlanClient()

        For Each wlanIface As WlanClient.WlanInterface In client.Interfaces

            Dim wlanBssEntries As Wlan.WlanBssEntry() = wlanIface.GetNetworkBssList()

            '

            For Each network As Wlan.WlanBssEntry In wlanBssEntries
                ' 
                Dim listItemWiFi As New ListViewItem()
                ' 
                With listItemWiFi
                    .Text = System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).Trim(ChrW(0))

                    '
                    .SubItems.Add(network.beaconPeriod.ToString() + "")
                    .SubItems.Add(network.capabilityInformation.ToString())
                    .SubItems.Add(network.chCenterFrequency.ToString() + "Hz")

                    .SubItems.Add(ConvertToChannel(network.chCenterFrequency))

                    '.SubItems.Add(network.dot11Bssid.Count.ToString())
                    .SubItems.Add(BytesToString(network.dot11Bssid))
                    .SubItems.Add(network.dot11BssPhyType.ToString() + "")
                    .SubItems.Add(network.dot11BssType.ToString() + "")

                    .SubItems.Add(network.hostTimestamp.ToString() + "")
                    .SubItems.Add(network.ieOffset.ToString() + "")
                    .SubItems.Add(network.ieSize.ToString() + "")
                    .SubItems.Add(network.inRegDomain.ToString() + "")
                    .SubItems.Add(network.linkQuality.ToString() + "")
                    .SubItems.Add(network.phyId.ToString() + "")
                    .SubItems.Add(network.rssi.ToString() + "")

                    .SubItems.Add(network.timestamp.ToString() + "")
                    '.SubItems.Add(network.wlanRateSet.GetRateInMbps(CInt(network.wlanRateSet.Rates.)) + "")
                    .SubItems.Add(convertRatesToMbps(network.wlanRateSet.Rates))

                End With

                With lstvAvailableNetworkList.Columns
                    .Clear()
                    .Add("SSID")
                    .Add("beaconPeriod")
                    .Add("capabilityInformation")
                    .Add("chCenterFrequency")
                    .Add("channel")
                    .Add("MAC")
                    '.Add("dot11Bssid")

                    .Add("dot11BssPhyType")
                    .Add("dot11BssType")

                    .Add("hostTimestamp")
                    .Add("ieOffset")
                    .Add("ieSize")
                    .Add("inRegDomain")
                    .Add("linkQuality")
                    .Add("phyId")
                    .Add("rssi")

                    .Add("timestamp")
                    .Add("wlanRateSet")

                End With

                lstvAvailableNetworkList.Items.Add(listItemWiFi)
            Next
        Next
        ''''' Dim wlaniface2 As 

        ' txtStuID.Text = wlaniface2.CurrentConnection.isState


    End Sub




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '17.1.8 stuID limit (must fill with the txtStuId)
        If txtStuID.Text.Length <> 8 Then
            MsgBox("學號長度有誤，請重新確認學號是否正確", MsgBoxStyle.Critical, "提示")
            Return
        End If
        '17.1.21 add location info
        If txtLocation.Text.Length = 0 Then
            MsgBox("請輸入檢測地點", MsgBoxStyle.Critical, "提示")
            Return
        End If

        Me.Text = "0%"

        cbListIndex.Items.Clear()
        WifiGroupList.Clear()
        ''170720 Add locationName Prop. and loation Category
        If cbxLocationName.SelectedItem.ToString() = "菲" Then
            str_LocationName = "菲華樓"
        Else
            str_LocationName = "大" + cbxLocationName.SelectedItem.ToString() + "館"
        End If
        If rdoClassroom.Checked = True Then
            location_Category = "room"
            str_roomID = IIf(cbxNo_extpant.SelectedItem = 0,
                             cbxFloor.SelectedItem.ToString() + cbxID.SelectedItem.ToString(),
                             cbxFloor.SelectedItem.ToString() + "-" + cbxNo_extpant.SelectedItem.ToString()
                             )
        Else
            location_Category = "aisle"
        End If

        dt = GetTable()
        groupindex = Date.Now.ToString("yyyyMMddHHmmss")
        Button3.Enabled = False
        Button2.Enabled = False
        GetBssList()
        ProgressBar1.Value = 0

        Timer1.Interval = 10000 'Every 10s will search once
        Timer1.Start()

        Timer2.Interval = NumericUpDown1.Value * 1000 + 2000
        Timer2.Start()

        If Timer2.Interval - Timer1.Interval < 0 Then
            ProgressBar1.Maximum = 10000
        Else
            ProgressBar1.Maximum = Timer2.Interval - 2000
        End If




    End Sub



    Private Sub GetBssList()


        lstvAvailableNetworkList.Clear()

        Dim client As New WlanClient()

        Dim wifiGroup As New WifiGroup()

        With lstvAvailableNetworkList.Columns
            .Clear()
            .Add("SSID")
            .Add("beaconPeriod")
            .Add("capabilityInformation")
            .Add("chCenterFrequency")

            .Add("MAC")
            '.Add("dot11Bssid")

            .Add("dot11BssPhyType")
            .Add("dot11BssType")

            .Add("hostTimestamp")
            .Add("ieOffset")
            .Add("ieSize")
            .Add("inRegDomain")
            .Add("linkQuality")
            .Add("phyId")
            .Add("rssi")

            .Add("timestamp")
            .Add("wlanRateSet")

        End With

        For Each wlanIface As WlanClient.WlanInterface In client.Interfaces

            Dim wlanBssEntries As Wlan.WlanBssEntry() = wlanIface.GetNetworkBssList()

            '

            For Each network As Wlan.WlanBssEntry In wlanBssEntries
                ' 
                Dim listItemWiFi As New ListViewItem()

                ' Add to List 16/11/04

                wifiGroup.AddWifi(Encoding.UTF8.GetString(network.dot11Ssid.SSID).Trim(ChrW(0)),
                                  BytesToString(network.dot11Bssid),
                                  ConvertToChannel(network.chCenterFrequency),
                                  network.linkQuality.ToString() + "",
                                  network.rssi.ToString() + ""
                )

                ''table add
                dt.Rows.Add(groupindex, Encoding.UTF8.GetString(network.dot11Ssid.SSID).Trim(ChrW(0)),
                            BytesToString(network.dot11Bssid),
                            ConvertToChannel(network.chCenterFrequency),
                            network.linkQuality.ToString() + "",
                            network.rssi.ToString() + "",
                            "Raw",
                            Date.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            DBNull.Value,
                            txtStuID.Text.ToUpper(),
                            txtLocation.Text,
                            str_LocationName,
                            cbxFloor.SelectedItem.ToString(),
                            location_Category,
                            str_roomID
                            )


                With listItemWiFi
                    .Text = Encoding.UTF8.GetString(network.dot11Ssid.SSID).Trim(ChrW(0))

                    '
                    .SubItems.Add(network.beaconPeriod.ToString() + "")
                    .SubItems.Add(network.capabilityInformation.ToString())
                    .SubItems.Add(network.chCenterFrequency.ToString() + "Hz")

                    '.SubItems.Add(network.dot11Bssid.Count.ToString())
                    .SubItems.Add(BytesToString(network.dot11Bssid))
                    .SubItems.Add(network.dot11BssPhyType.ToString() + "")
                    .SubItems.Add(network.dot11BssType.ToString() + "")

                    .SubItems.Add(network.hostTimestamp.ToString() + "")
                    .SubItems.Add(network.ieOffset.ToString() + "")
                    .SubItems.Add(network.ieSize.ToString() + "")
                    .SubItems.Add(network.inRegDomain.ToString() + "")
                    .SubItems.Add(network.linkQuality.ToString() + "")
                    .SubItems.Add(network.phyId.ToString() + "")
                    .SubItems.Add(network.rssi.ToString() + "")

                    .SubItems.Add(network.timestamp.ToString() + "")
                    '.SubItems.Add(network.wlanRateSet.GetRateInMbps(CInt(network.wlanRateSet.Rates.)) + "")
                    .SubItems.Add(network.wlanRateSet.Rates.Count.ToString() + "")



                End With



                lstvAvailableNetworkList.Items.Add(listItemWiFi)
            Next
        Next
        WifiGroupList.Add(wifiGroup)
        cbListIndex.Items.Add(WifiGroupList.Count)

    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If ProgressBar1.Value <> Timer2.Interval Then

            ProgressBar1.Value += Timer1.Interval
            Me.Text = ((ProgressBar1.Value / ProgressBar1.Maximum) * 100).ToString("0.##") + "%"

        End If
        GetBssList()



    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Timer1.Stop()
        Timer2.Stop()
        Button3.Enabled = True
        Button2.Enabled = True
        MessageBox.Show("蒐集完畢，請連上網路後按下SaveToDatabase")



    End Sub

    Public Function convertFrequencyToChannel(freq As Integer) As Integer

        If freq >= 2412 AndAlso freq <= 2484 Then
            Return (freq - 2412) / 5 + 1
        ElseIf freq >= 5170 AndAlso freq <= 5825 Then
            Return (freq - 5170) / 5 + 34
        Else
            Return -1
        End If
    End Function


    Public Shared Function ConvertToChannel(frequency As UInteger) As UInteger
        Dim retVal As UInteger = 0
        ' 2.4 GHz
        If (frequency > 2400000) AndAlso (frequency < 2484000) Then
            retVal = (frequency - 2407000) / 5000
        End If

        If (frequency >= 2484000) AndAlso (frequency <= 2495000) Then
            retVal = 14
        End If

        ' 5 GHz
        If (frequency > 5000000) AndAlso (frequency < 5900000) Then
            retVal = (frequency - 5000000) / 5000
        End If

        Return retVal
    End Function


    Public Function convertRatesToMbps(RateSets As UShort()) As String

        Dim Len As Integer = RateSets.Length - 1

        Dim Result As Double() = New Double(Len) {}

        For i As Integer = 0 To RateSets.Length - 1

            Result(i) = (RateSets(i) And &H7FFF) * 0.5

        Next

        Return Result.Max()

    End Function

    Private Sub cbListIndex_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbListIndex.SelectedIndexChanged

        Dim Lists = WifiGroupList.Item(cbListIndex.SelectedIndex)
        GroupListView.Clear()

        With GroupListView.Columns
            .Clear()
            .Add("SSID")
            .Add("MAC")
            .Add("Channel")
            .Add("linkQuality")
            .Add("rssi")
        End With
        '


        For Each list In Lists.GetWifiData

            Dim listItemWiFi As New ListViewItem()

            With listItemWiFi

                .Text = list.ssid.Trim(ChrW(0))
                .SubItems.Add(list.Mac)
                .SubItems.Add(list.channel)
                .SubItems.Add(list.linkquality)
                .SubItems.Add(list.rssi)

            End With
            GroupListView.Items.Add(listItemWiFi)

        Next



    End Sub

    Function GetTable() As DataTable
        ' Create new DataTable instance.
        Dim table As New DataTable

        ' Create four typed columns in the DataTable.
        table.Columns.Add("GroupIndex", GetType(String))
        table.Columns.Add("ssid", GetType(String))
        table.Columns.Add("mac", GetType(String))
        table.Columns.Add("channel", GetType(Integer))
        table.Columns.Add("linkquality", GetType(Integer))
        table.Columns.Add("rssi", GetType(Integer))
        table.Columns.Add("status", GetType(String))
        table.Columns.Add("date", GetType(String))
        table.Columns.Add("ping", GetType(Integer))
        table.Columns.Add("studentID", GetType(String))
        table.Columns.Add("location_code", GetType(String))
        table.Columns.Add("location", GetType(String))
        table.Columns.Add("floor", GetType(String))
        table.Columns.Add("location_category", GetType(String))
        table.Columns.Add("roomId", GetType(String))
        Return table
    End Function

    Sub InsertToSql()

        Dim sw As New Stopwatch
        sw.Start()
        Dim AllWifiList As New List(Of WLANList)()


        For Each group In WifiGroupList

            Dim wifiList = group.GetWifiData
            For Each Wifi In wifiList
                AllWifiList.Add(Wifi)
            Next
        Next

        Dim linqQuery = From wifidata In AllWifiList
                        Group wifidata By mac = wifidata.Mac, ssid = wifidata.ssid, channel = wifidata.channel Into Group
                        Select New With {
                            .mac = mac,
                            .ssid = ssid,
                            .channel = channel,
                            .AvgLinkquality = Group.Average(Function(x) Convert.ToInt32(x.linkquality)),
                            .AvgRssi = Group.Average(Function(x) Convert.ToInt32(x.rssi))
        }

        For Each linqData In linqQuery
            dt.Rows.Add(groupindex,
                        linqData.ssid,
                        linqData.mac,
                        linqData.channel,
                        linqData.AvgLinkquality,
                        linqData.AvgRssi, "Avg",
                        Date.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                        DBNull.Value,
                        txtStuID.Text.ToUpper(),
                        txtLocation.Text,
                        str_LocationName,
                        cbxFloor.SelectedItem.ToString(),
                        location_Category,
                        str_roomID)
        Next

        '17.01.12 ping data
        GetPingTbRow()

        Using SqlConnection As New SqlConnection With {.ConnectionString = "server=140.137.149.200;database=PowerBI;UID=prj_wifi;password=f@!e7dJJ"}
            Using SqlCommand As New SqlCommand With {.CommandType = CommandType.StoredProcedure, .Connection = SqlConnection, .CommandText = "InsertTable"}

                SqlCommand.Parameters.Add(New SqlParameter("@MyTableType", dt))
                SqlConnection.Open()
                SqlCommand.ExecuteNonQuery()

            End Using

        End Using

        sw.Stop()
        MessageBox.Show("Save Finish! Cost " + sw.ElapsedMilliseconds.ToString() + " ms")
        cbListIndex.Items.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            InsertToSql()
            WifiGroupList.Clear()
            txtLocation.Text = ""
            str_roomID = ""

        Catch ex As Exception
            MessageBox.Show(ex.ToString())

        End Try

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For i As Integer = 1 To 99
            cbxID.Items.Add(i.ToString("00"))
        Next
        For i As Integer = 0 To 4
            cbxNo_extpant.Items.Add(i)
        Next
        For i As Integer = 1 To 8
            cbxClassroomNo.Items.Add(i)
        Next

        cbxNo_extpant.SelectedIndex = 0
        cbxClassroomNo.SelectedIndex = 0
    End Sub

    '17/07/20 Add new generate locationID automatically
    Private Sub btnLocation_Click(sender As Object, e As EventArgs) Handles btnLocation.Click
        Try
            Dim _locationName = cbxLocationName.SelectedItem.ToString()
            Dim _floor = cbxFloor.SelectedItem.ToString()
            Dim _No = cbxID.SelectedItem.ToString()

            If rdoClassroom.Checked = True Then
                txtLocation.Text = IIf(cbxNo_extpant.SelectedIndex = 0,
                                       _locationName + _floor + _No + "-" + cbxClassroomNo.SelectedItem.ToString(),
                                       _locationName + _floor + _No + "-" + cbxNo_extpant.SelectedItem.ToString() + "-" + cbxClassroomNo.SelectedItem.ToString())
            ElseIf rdoAisle.Checked = True Then
                txtLocation.Text = _locationName + _floor + "F" + _No
            End If
        Catch ex As Exception
            MessageBox.Show("請確認地點選擇是否有遺漏")
        End Try

    End Sub

    Private Sub rdoAisle_CheckedChanged(sender As Object, e As EventArgs) Handles rdoAisle.CheckedChanged
        If rdoAisle.Checked = True Then
            cbxNo_extpant.Enabled = False
            cbxClassroomNo.Enabled = False
        Else
            cbxNo_extpant.Enabled = True
            cbxClassroomNo.Enabled = True
        End If
    End Sub
End Class
