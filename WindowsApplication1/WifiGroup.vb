Public Class WifiGroup
    Dim wifilist As New List(Of WLANList)()

    Public Sub AddWifi(ByVal ssid As String, ByVal Mac As String, ByVal channel As String, ByVal linkquality As String, ByVal rssi As String)

        wifilist.Add(New WLANList() With {
            .ssid = ssid,
            .Mac = Mac,
            .channel = channel,
            .linkquality = linkquality,
            .rssi = rssi
                     })

    End Sub

    Public Function GetWifiData() As List(Of WLANList)
        Return wifilist
    End Function
End Class
