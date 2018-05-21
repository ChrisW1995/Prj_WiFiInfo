Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports System.Threading
Imports System.Text

Namespace NativeWifi
    ''' <summary>
    ''' Represents a client to the Zeroconf (Native Wifi) service.
    ''' </summary>
    ''' <remarks>
    ''' This class is the entrypoint to Native Wifi management. To manage WiFi settings, create an instance
    ''' of this class.
    ''' </remarks>
    Public Class WlanClient
        ''' <summary>
        ''' Represents a Wifi network interface.
        ''' </summary>
        Public Class WlanInterface
            Private client As WlanClient
            Private info As Wlan.WlanInterfaceInfo

#Region "Events"
            ''' <summary>
            ''' Represents a method that will handle <see cref="WlanNotification"/> events.
            ''' </summary>
            ''' <param name="notifyData">The notification data.</param>
            Public Delegate Sub WlanNotificationEventHandler(ByVal notifyData As Wlan.WlanNotificationData)

            ''' <summary>
            ''' Represents a method that will handle <see cref="WlanConnectionNotification"/> events.
            ''' </summary>
            ''' <param name="notifyData">The notification data.</param>
            ''' <param name="connNotifyData">The notification data.</param>
            Public Delegate Sub WlanConnectionNotificationEventHandler(ByVal notifyData As Wlan.WlanNotificationData, ByVal connNotifyData As Wlan.WlanConnectionNotificationData)

            ''' <summary>
            ''' Represents a method that will handle <see cref="WlanReasonNotification"/> events.
            ''' </summary>
            ''' <param name="notifyData">The notification data.</param>
            ''' <param name="reasonCode">The reason code.</param>
            Public Delegate Sub WlanReasonNotificationEventHandler(ByVal notifyData As Wlan.WlanNotificationData, ByVal reasonCode As Wlan.WlanReasonCode)

            ''' <summary>
            ''' Occurs when an event of any kind occurs on a WLAN interface.
            ''' </summary>
            Public Event WlanNotification As WlanNotificationEventHandler

            ''' <summary>
            ''' Occurs when a WLAN interface changes connection state.
            ''' </summary>
            Public Event WlanConnectionNotification As WlanConnectionNotificationEventHandler

            ''' <summary>
            ''' Occurs when a WLAN operation fails due to some reason.
            ''' </summary>
            Public Event WlanReasonNotification As WlanReasonNotificationEventHandler

#End Region

#Region "Event queue"
            Private queueEvents As Boolean
            Private eventQueueFilled As New AutoResetEvent(False)
            Private eventQueue As New Queue(Of Object)()

            Private Structure WlanConnectionNotificationEventData
                Public notifyData As Wlan.WlanNotificationData
                Public connNotifyData As Wlan.WlanConnectionNotificationData
            End Structure
            Private Structure WlanReasonNotificationData
                Public notifyData As Wlan.WlanNotificationData
                Public reasonCode As Wlan.WlanReasonCode
            End Structure
#End Region

            Friend Sub New(ByVal client As WlanClient, ByVal info As Wlan.WlanInterfaceInfo)
                Me.client = client
                Me.info = info
            End Sub

            ''' <summary>
            ''' Sets a parameter of the interface whose data type is <see cref="int"/>.
            ''' </summary>
            ''' <param name="opCode">The opcode of the parameter.</param>
            ''' <param name="value">The value to set.</param>
            Private Sub SetInterfaceInt(ByVal opCode As Wlan.WlanIntfOpcode, ByVal value As Integer)
                Dim valuePtr As IntPtr = Marshal.AllocHGlobal(4)
                Marshal.WriteInt32(valuePtr, value)
                Try
                    Wlan.ThrowIfError(Wlan.WlanSetInterface(client.clientHandle, info.interfaceGuid, opCode, 4, valuePtr, IntPtr.Zero))
                Finally
                    Marshal.FreeHGlobal(valuePtr)
                End Try
            End Sub

            ''' <summary>
            ''' Gets a parameter of the interface whose data type is <see cref="int"/>.
            ''' </summary>
            ''' <param name="opCode">The opcode of the parameter.</param>
            ''' <returns>The integer value.</returns>
            Private Function GetInterfaceInt(ByVal opCode As Wlan.WlanIntfOpcode) As Integer
                Dim valuePtr As IntPtr
                Dim valueSize As Integer
                Dim opcodeValueType As Wlan.WlanOpcodeValueType
                Wlan.ThrowIfError(Wlan.WlanQueryInterface(client.clientHandle, info.interfaceGuid, opCode, IntPtr.Zero, valueSize, valuePtr, _
                 opcodeValueType))
                Try
                    Return Marshal.ReadInt32(valuePtr)
                Finally
                    Wlan.WlanFreeMemory(valuePtr)
                End Try
            End Function

            ''' <summary>
            ''' Gets or sets a value indicating whether this <see cref="WlanInterface"/> is automatically configured.
            ''' </summary>
            ''' <value><c>true</c> if "autoconf" is enabled; otherwise, <c>false</c>.</value>
            Public Property Autoconf() As Boolean
                Get
                    Return GetInterfaceInt(Wlan.WlanIntfOpcode.AutoconfEnabled) <> 0
                End Get
                Set(ByVal value As Boolean)
                    SetInterfaceInt(Wlan.WlanIntfOpcode.AutoconfEnabled, If(value, 1, 0))
                End Set
            End Property

            ''' <summary>
            ''' Gets or sets the BSS type for the indicated interface.
            ''' </summary>
            ''' <value>The type of the BSS.</value>
            Public Property BssType() As Wlan.Dot11BssType
                Get
                    Return DirectCast(GetInterfaceInt(Wlan.WlanIntfOpcode.BssType), Wlan.Dot11BssType)
                End Get
                Set(ByVal value As Wlan.Dot11BssType)
                    SetInterfaceInt(Wlan.WlanIntfOpcode.BssType, CInt(value))
                End Set
            End Property

            ''' <summary>
            ''' Gets the state of the interface.
            ''' </summary>
            ''' <value>The state of the interface.</value>
            Public ReadOnly Property InterfaceState() As Wlan.WlanInterfaceState
                Get
                    Return DirectCast(GetInterfaceInt(Wlan.WlanIntfOpcode.InterfaceState), Wlan.WlanInterfaceState)
                End Get
            End Property

            ''' <summary>
            ''' Gets the channel.
            ''' </summary>
            ''' <value>The channel.</value>
            ''' <remarks>Not supported on Windows XP SP2.</remarks>
            Public ReadOnly Property Channel() As Integer
                Get
                    Return GetInterfaceInt(Wlan.WlanIntfOpcode.ChannelNumber)
                End Get
            End Property

            ''' <summary>
            ''' Gets the RSSI.
            ''' </summary>
            ''' <value>The RSSI.</value>
            ''' <remarks>Not supported on Windows XP SP2.</remarks>
            Public ReadOnly Property RSSI() As Integer
                Get
                    Return GetInterfaceInt(Wlan.WlanIntfOpcode.RSSI)
                End Get
            End Property
            ''' <summary>
            ''' Gets the attributes of the current connection.
            ''' </summary>
            ''' <value>The current connection attributes.</value>
            ''' <exception cref="Win32Exception">An exception with code 0x0000139F (The group or resource is not in the correct state to perform the requested operation.) will be thrown if the interface is not connected to a network.</exception>
            Public ReadOnly Property CurrentConnection() As Wlan.WlanConnectionAttributes
                Get
                    Dim valueSize As Integer
                    Dim valuePtr As IntPtr
                    Dim opcodeValueType As Wlan.WlanOpcodeValueType
                    Wlan.ThrowIfError(Wlan.WlanQueryInterface(client.clientHandle, info.interfaceGuid, Wlan.WlanIntfOpcode.CurrentConnection, IntPtr.Zero, valueSize, valuePtr, _
                     opcodeValueType))
                    Try
                        Return DirectCast(Marshal.PtrToStructure(valuePtr, GetType(Wlan.WlanConnectionAttributes)), Wlan.WlanConnectionAttributes)
                    Finally
                        Wlan.WlanFreeMemory(valuePtr)
                    End Try
                End Get
            End Property

            ''' <summary>
            ''' Requests a scan for available networks.
            ''' </summary>
            ''' <remarks>
            ''' The method returns immediately. Progress is reported through the <see cref="WlanNotification"/> event.
            ''' </remarks>
            Public Sub Scan()
                Wlan.ThrowIfError(Wlan.WlanScan(client.clientHandle, info.interfaceGuid, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
            End Sub

            ''' <summary>
            ''' Converts a pointer to a available networks list (header + entries) to an array of available network entries.
            ''' </summary>
            ''' <param name="bssListPtr">A pointer to an available networks list's header.</param>
            ''' <returns>An array of available network entries.</returns>
            Private Function ConvertAvailableNetworkListPtr(ByVal availNetListPtr As IntPtr) As Wlan.WlanAvailableNetwork()
                Dim availNetListHeader As Wlan.WlanAvailableNetworkListHeader = DirectCast(Marshal.PtrToStructure(availNetListPtr, GetType(Wlan.WlanAvailableNetworkListHeader)), Wlan.WlanAvailableNetworkListHeader)
                Dim availNetListIt As Long = availNetListPtr.ToInt64() + Marshal.SizeOf(GetType(Wlan.WlanAvailableNetworkListHeader))
                Dim availNets As Wlan.WlanAvailableNetwork() = New Wlan.WlanAvailableNetwork(availNetListHeader.numberOfItems - 1) {}
                For i As Integer = 0 To availNetListHeader.numberOfItems - 1
                    availNets(i) = DirectCast(Marshal.PtrToStructure(New IntPtr(availNetListIt), GetType(Wlan.WlanAvailableNetwork)), Wlan.WlanAvailableNetwork)
                    availNetListIt += Marshal.SizeOf(GetType(Wlan.WlanAvailableNetwork))
                Next
                Return availNets
            End Function

            ''' <summary>
            ''' Retrieves the list of available networks.
            ''' </summary>
            ''' <param name="flags">Controls the type of networks returned.</param>
            ''' <returns>A list of the available networks.</returns>
            Public Function GetAvailableNetworkList(ByVal flags As Wlan.WlanGetAvailableNetworkFlags) As Wlan.WlanAvailableNetwork()
                Dim availNetListPtr As IntPtr
                Wlan.ThrowIfError(Wlan.WlanGetAvailableNetworkList(client.clientHandle, info.interfaceGuid, flags, IntPtr.Zero, availNetListPtr))
                Try
                    Return ConvertAvailableNetworkListPtr(availNetListPtr)
                Finally
                    Wlan.WlanFreeMemory(availNetListPtr)
                End Try
            End Function

            ''' <summary>
            ''' Converts a pointer to a BSS list (header + entries) to an array of BSS entries.
            ''' </summary>
            ''' <param name="bssListPtr">A pointer to a BSS list's header.</param>
            ''' <returns>An array of BSS entries.</returns>
            Private Function ConvertBssListPtr(ByVal bssListPtr As IntPtr) As Wlan.WlanBssEntry()
                Dim bssListHeader As Wlan.WlanBssListHeader = DirectCast(Marshal.PtrToStructure(bssListPtr, GetType(Wlan.WlanBssListHeader)), Wlan.WlanBssListHeader)
                Dim bssListIt As Long = bssListPtr.ToInt64() + Marshal.SizeOf(GetType(Wlan.WlanBssListHeader))
                Dim bssEntries As Wlan.WlanBssEntry() = New Wlan.WlanBssEntry(bssListHeader.numberOfItems - 1) {}
                For i As Integer = 0 To bssListHeader.numberOfItems - 1
                    bssEntries(i) = DirectCast(Marshal.PtrToStructure(New IntPtr(bssListIt), GetType(Wlan.WlanBssEntry)), Wlan.WlanBssEntry)
                    bssListIt += Marshal.SizeOf(GetType(Wlan.WlanBssEntry))
                Next
                Return bssEntries
            End Function

            ''' <summary>
            ''' Retrieves the basic service sets (BSS) list of all available networks.
            ''' </summary>
            Public Function GetNetworkBssList() As Wlan.WlanBssEntry()
                Dim bssListPtr As IntPtr
                Wlan.ThrowIfError(Wlan.WlanGetNetworkBssList(client.clientHandle, info.interfaceGuid, IntPtr.Zero, Wlan.Dot11BssType.Any, False, IntPtr.Zero, _
                 bssListPtr))
                Try
                    Return ConvertBssListPtr(bssListPtr)
                Finally
                    Wlan.WlanFreeMemory(bssListPtr)
                End Try
            End Function

            ''' <summary>
            ''' Retrieves the basic service sets (BSS) list of the specified network.
            ''' </summary>
            ''' <param name="ssid">Specifies the SSID of the network from which the BSS list is requested.</param>
            ''' <param name="bssType">Indicates the BSS type of the network.</param>
            ''' <param name="securityEnabled">Indicates whether security is enabled on the network.</param>
            Public Function GetNetworkBssList(ByVal ssid As Wlan.Dot11Ssid, ByVal bssType As Wlan.Dot11BssType, ByVal securityEnabled As Boolean) As Wlan.WlanBssEntry()
                Dim ssidPtr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ssid))
                Marshal.StructureToPtr(ssid, ssidPtr, False)
                Try
                    Dim bssListPtr As IntPtr
                    Wlan.ThrowIfError(Wlan.WlanGetNetworkBssList(client.clientHandle, info.interfaceGuid, ssidPtr, bssType, securityEnabled, IntPtr.Zero, _
                     bssListPtr))
                    Try
                        Return ConvertBssListPtr(bssListPtr)
                    Finally
                        Wlan.WlanFreeMemory(bssListPtr)
                    End Try
                Finally
                    Marshal.FreeHGlobal(ssidPtr)
                End Try
            End Function

            ''' <summary>
            ''' Connects to a network defined by a connection parameters structure.
            ''' </summary>
            ''' <param name="connectionParams">The connection paramters.</param>
            Protected Sub Connect(ByVal connectionParams As Wlan.WlanConnectionParameters)
                Wlan.ThrowIfError(Wlan.WlanConnect(client.clientHandle, info.interfaceGuid, connectionParams, IntPtr.Zero))
            End Sub

            ''' <summary>
            ''' Requests a connection (association) to the specified wireless network.
            ''' </summary>
            ''' <remarks>
            ''' The method returns immediately. Progress is reported through the <see cref="WlanNotification"/> event.
            ''' </remarks>
            Public Sub Connect(ByVal connectionMode As Wlan.WlanConnectionMode, ByVal bssType As Wlan.Dot11BssType, ByVal profile As String)
                Dim connectionParams As New Wlan.WlanConnectionParameters()
                connectionParams.wlanConnectionMode = connectionMode
                connectionParams.profile = profile
                connectionParams.dot11BssType = bssType
                connectionParams.flags = 0
                Connect(connectionParams)
            End Sub

            ''' <summary>
            ''' Connects (associates) to the specified wireless network, returning either on a success to connect
            ''' or a failure.
            ''' </summary>
            ''' <param name="connectionMode"></param>
            ''' <param name="bssType"></param>
            ''' <param name="profile"></param>
            ''' <param name="connectTimeout"></param>
            ''' <returns></returns>
            Public Function ConnectSynchronously(ByVal connectionMode As Wlan.WlanConnectionMode, ByVal bssType As Wlan.Dot11BssType, ByVal profile As String, ByVal connectTimeout As Integer) As Boolean
                queueEvents = True
                Try
                    Connect(connectionMode, bssType, profile)
                    While queueEvents AndAlso eventQueueFilled.WaitOne(connectTimeout, True)
                        SyncLock eventQueue
                            While eventQueue.Count <> 0
                                Dim e As Object = eventQueue.Dequeue()
                                If TypeOf e Is WlanConnectionNotificationEventData Then
                                    Dim wlanConnectionData As WlanConnectionNotificationEventData = CType(e, WlanConnectionNotificationEventData)
                                    ' Check if the conditions are good to indicate either success or failure.
                                    If wlanConnectionData.notifyData.notificationSource = Wlan.WlanNotificationSource.ACM Then
                                        Select Case DirectCast(wlanConnectionData.notifyData.notificationCode, Wlan.WlanNotificationCodeAcm)
                                            Case Wlan.WlanNotificationCodeAcm.ConnectionComplete
                                                If wlanConnectionData.connNotifyData.profileName = profile Then
                                                    Return True
                                                End If
                                                Exit Select
                                        End Select
                                    End If
                                    Exit While
                                End If
                            End While
                        End SyncLock
                    End While
                Finally
                    queueEvents = False
                    eventQueue.Clear()
                End Try
                Return False
                ' timeout expired and no "connection complete"
            End Function

            ''' <summary>
            ''' Connects to the specified wireless network.
            ''' </summary>
            ''' <remarks>
            ''' The method returns immediately. Progress is reported through the <see cref="WlanNotification"/> event.
            ''' </remarks>
            Public Sub Connect(ByVal connectionMode As Wlan.WlanConnectionMode, ByVal bssType As Wlan.Dot11BssType, ByVal ssid As Wlan.Dot11Ssid, ByVal flags As Wlan.WlanConnectionFlags)
                Dim connectionParams As New Wlan.WlanConnectionParameters()
                connectionParams.wlanConnectionMode = connectionMode
                connectionParams.dot11SsidPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ssid))
                Marshal.StructureToPtr(ssid, connectionParams.dot11SsidPtr, False)
                connectionParams.dot11BssType = bssType
                connectionParams.flags = flags
                Connect(connectionParams)
                Marshal.DestroyStructure(connectionParams.dot11SsidPtr, ssid.[GetType]())
                Marshal.FreeHGlobal(connectionParams.dot11SsidPtr)
            End Sub

            ''' <summary>
            ''' Deletes a profile.
            ''' </summary>
            ''' <param name="profileName">
            ''' The name of the profile to be deleted. Profile names are case-sensitive.
            ''' On Windows XP SP2, the supplied name must match the profile name derived automatically from the SSID of the network. For an infrastructure network profile, the SSID must be supplied for the profile name. For an ad hoc network profile, the supplied name must be the SSID of the ad hoc network followed by <c>-adhoc</c>.
            ''' </param>
            Public Sub DeleteProfile(ByVal profileName As String)
                Wlan.ThrowIfError(Wlan.WlanDeleteProfile(client.clientHandle, info.interfaceGuid, profileName, IntPtr.Zero))
            End Sub

            ''' <summary>
            ''' Sets the profile.
            ''' </summary>
            ''' <param name="flags">The flags to set on the profile.</param>
            ''' <param name="profileXml">The XML representation of the profile. On Windows XP SP 2, special care should be taken to adhere to its limitations.</param>
            ''' <param name="overwrite">If a profile by the given name already exists, then specifies whether to overwrite it (if <c>true</c>) or return an error (if <c>false</c>).</param>
            ''' <returns>The resulting code indicating a success or the reason why the profile wasn't valid.</returns>
            Public Function SetProfile(ByVal flags As Wlan.WlanProfileFlags, ByVal profileXml As String, ByVal overwrite As Boolean) As Wlan.WlanReasonCode
                Dim reasonCode As Wlan.WlanReasonCode
                Wlan.ThrowIfError(Wlan.WlanSetProfile(client.clientHandle, info.interfaceGuid, flags, profileXml, Nothing, overwrite, _
                 IntPtr.Zero, reasonCode))
                Return reasonCode
            End Function

            ''' <summary>
            ''' Gets the profile's XML specification.
            ''' </summary>
            ''' <param name="profileName">The name of the profile.</param>
            ''' <returns>The XML document.</returns>
            Public Function GetProfileXml(ByVal profileName As String) As String
                Dim profileXmlPtr As IntPtr
                Dim flags As Wlan.WlanProfileFlags
                Dim access As Wlan.WlanAccess
                Wlan.ThrowIfError(Wlan.WlanGetProfile(client.clientHandle, info.interfaceGuid, profileName, IntPtr.Zero, profileXmlPtr, flags, _
                 access))
                Try
                    Return Marshal.PtrToStringUni(profileXmlPtr)
                Finally
                    Wlan.WlanFreeMemory(profileXmlPtr)
                End Try
            End Function

            ''' <summary>
            ''' Gets the information of all profiles on this interface.
            ''' </summary>
            ''' <returns>The profiles information.</returns>
            Public Function GetProfiles() As Wlan.WlanProfileInfo()
                Dim profileListPtr As IntPtr
                Wlan.ThrowIfError(Wlan.WlanGetProfileList(client.clientHandle, info.interfaceGuid, IntPtr.Zero, profileListPtr))
                Try
                    Dim header As Wlan.WlanProfileInfoListHeader = DirectCast(Marshal.PtrToStructure(profileListPtr, GetType(Wlan.WlanProfileInfoListHeader)), Wlan.WlanProfileInfoListHeader)
                    Dim profileInfos As Wlan.WlanProfileInfo() = New Wlan.WlanProfileInfo(header.numberOfItems - 1) {}
                    Dim profileListIterator As Long = profileListPtr.ToInt64() + Marshal.SizeOf(header)
                    For i As Integer = 0 To header.numberOfItems - 1
                        Dim profileInfo As Wlan.WlanProfileInfo = DirectCast(Marshal.PtrToStructure(New IntPtr(profileListIterator), GetType(Wlan.WlanProfileInfo)), Wlan.WlanProfileInfo)
                        profileInfos(i) = profileInfo
                        profileListIterator += Marshal.SizeOf(profileInfo)
                    Next
                    Return profileInfos
                Finally
                    Wlan.WlanFreeMemory(profileListPtr)
                End Try
            End Function

            Friend Sub OnWlanConnection(ByVal notifyData As Wlan.WlanNotificationData, ByVal connNotifyData As Wlan.WlanConnectionNotificationData)
                RaiseEvent WlanConnectionNotification(notifyData, connNotifyData)

                If queueEvents Then
                    Dim queuedEvent As New WlanConnectionNotificationEventData()
                    queuedEvent.notifyData = notifyData
                    queuedEvent.connNotifyData = connNotifyData
                    EnqueueEvent(queuedEvent)
                End If
            End Sub

            Friend Sub OnWlanReason(ByVal notifyData As Wlan.WlanNotificationData, ByVal reasonCode As Wlan.WlanReasonCode)
                RaiseEvent WlanReasonNotification(notifyData, reasonCode)
                If queueEvents Then
                    Dim queuedEvent As New WlanReasonNotificationData()
                    queuedEvent.notifyData = notifyData
                    queuedEvent.reasonCode = reasonCode
                    EnqueueEvent(queuedEvent)
                End If
            End Sub

            Friend Sub OnWlanNotification(ByVal notifyData As Wlan.WlanNotificationData)
                RaiseEvent WlanNotification(notifyData)
            End Sub

            ''' <summary>
            ''' Enqueues a notification event to be processed serially.
            ''' </summary>
            Private Sub EnqueueEvent(ByVal queuedEvent As Object)
                SyncLock eventQueue
                    eventQueue.Enqueue(queuedEvent)
                End SyncLock
                eventQueueFilled.[Set]()
            End Sub

            ''' <summary>
            ''' Gets the network interface of this wireless interface.
            ''' </summary>
            ''' <remarks>
            ''' The network interface allows querying of generic network properties such as the interface's IP address.
            ''' </remarks>
            Public ReadOnly Property NetworkInterface() As NetworkInterface
                Get
                    ' Do not cache the NetworkInterface; We need it fresh
                    ' each time cause otherwise it caches the IP information.
                    For Each netIface As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()
                        Dim netIfaceGuid As New Guid(netIface.Id)
                        If netIfaceGuid.Equals(info.interfaceGuid) Then
                            Return netIface
                        End If
                    Next
                    Return Nothing
                End Get
            End Property

            ''' <summary>
            ''' The GUID of the interface (same content as the <see cref="System.Net.NetworkInformation.NetworkInterface.Id"/> value).
            ''' </summary>
            Public ReadOnly Property InterfaceGuid() As Guid
                Get
                    Return info.interfaceGuid
                End Get
            End Property

            ''' <summary>
            ''' The description of the interface.
            ''' This is a user-immutable string containing the vendor and model name of the adapter.
            ''' </summary>
            Public ReadOnly Property InterfaceDescription() As String
                Get
                    Return info.interfaceDescription
                End Get
            End Property

            ''' <summary>
            ''' The friendly name given to the interface by the user (e.g. "Local Area Network Connection").
            ''' </summary>
            Public ReadOnly Property InterfaceName() As String
                Get
                    Return NetworkInterface.Name
                End Get
            End Property
        End Class

        Private clientHandle As IntPtr
        Private negotiatedVersion As UInteger
        Private wlanNotificationCallback As Wlan.WlanNotificationCallbackDelegate

        Private ifaces As New Dictionary(Of Guid, WlanInterface)()

        ''' <summary>
        ''' Creates a new instance of a Native Wifi service client.
        ''' </summary>
        Public Sub New()
            Wlan.ThrowIfError(Wlan.WlanOpenHandle(Wlan.WLAN_CLIENT_VERSION_XP_SP2, IntPtr.Zero, negotiatedVersion, clientHandle))
            Try
                Dim prevSrc As Wlan.WlanNotificationSource
                wlanNotificationCallback = New Wlan.WlanNotificationCallbackDelegate(AddressOf OnWlanNotification)
                Wlan.ThrowIfError(Wlan.WlanRegisterNotification(clientHandle, Wlan.WlanNotificationSource.All, False, wlanNotificationCallback, IntPtr.Zero, IntPtr.Zero, _
                 prevSrc))
            Catch
                Wlan.WlanCloseHandle(clientHandle, IntPtr.Zero)
                Throw
            End Try
        End Sub

        Protected Overrides Sub Finalize()
            Try
                Wlan.WlanCloseHandle(clientHandle, IntPtr.Zero)
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        Private Function ParseWlanConnectionNotification(ByRef notifyData As Wlan.WlanNotificationData) As System.Nullable(Of Wlan.WlanConnectionNotificationData)
            Dim expectedSize As Integer = Marshal.SizeOf(GetType(Wlan.WlanConnectionNotificationData))
            If notifyData.dataSize < expectedSize Then
                Return Nothing
            End If

            Dim connNotifyData As Wlan.WlanConnectionNotificationData = DirectCast(Marshal.PtrToStructure(notifyData.dataPtr, GetType(Wlan.WlanConnectionNotificationData)), Wlan.WlanConnectionNotificationData)
            If connNotifyData.wlanReasonCode = Wlan.WlanReasonCode.Success Then
                Dim profileXmlPtr As New IntPtr(notifyData.dataPtr.ToInt64() + Marshal.OffsetOf(GetType(Wlan.WlanConnectionNotificationData), "profileXml").ToInt64())
                connNotifyData.profileXml = Marshal.PtrToStringUni(profileXmlPtr)
            End If
            Return connNotifyData
        End Function

        Private Sub OnWlanNotification(ByRef notifyData As Wlan.WlanNotificationData, ByVal context As IntPtr)
            Dim wlanIface As WlanInterface = If(ifaces.ContainsKey(notifyData.interfaceGuid), ifaces(notifyData.interfaceGuid), Nothing)

            Select Case notifyData.notificationSource
                Case Wlan.WlanNotificationSource.ACM
                    Select Case DirectCast(notifyData.notificationCode, Wlan.WlanNotificationCodeAcm)
                        Case Wlan.WlanNotificationCodeAcm.ConnectionStart, Wlan.WlanNotificationCodeAcm.ConnectionComplete, Wlan.WlanNotificationCodeAcm.ConnectionAttemptFail, Wlan.WlanNotificationCodeAcm.Disconnecting, Wlan.WlanNotificationCodeAcm.Disconnected
                            Dim connNotifyData As System.Nullable(Of Wlan.WlanConnectionNotificationData) = ParseWlanConnectionNotification(notifyData)
                            If connNotifyData.HasValue Then
                                If wlanIface IsNot Nothing Then
                                    wlanIface.OnWlanConnection(notifyData, connNotifyData.Value)
                                End If
                            End If
                            Exit Select
                        Case Wlan.WlanNotificationCodeAcm.ScanFail
                            If True Then
                                'int expectedSize = Marshal.SizeOf(typeof (Wlan.WlanReasonCode));
                                'if (notifyData.dataSize >= expectedSize)
                                '{
                                'Wlan.WlanReasonCode reasonCode = (Wlan.WlanReasonCode) Marshal.ReadInt32(notifyData.dataPtr);
                                'if (wlanIface != null)
                                'wlanIface.OnWlanReason(notifyData, reasonCode);
                                '}
                            End If
                            Exit Select
                    End Select
                    Exit Select
                Case Wlan.WlanNotificationSource.MSM
                    Select Case DirectCast(notifyData.notificationCode, Wlan.WlanNotificationCodeMsm)
                        Case Wlan.WlanNotificationCodeMsm.Associating, Wlan.WlanNotificationCodeMsm.Associated, Wlan.WlanNotificationCodeMsm.Authenticating, Wlan.WlanNotificationCodeMsm.Connected, Wlan.WlanNotificationCodeMsm.RoamingStart, Wlan.WlanNotificationCodeMsm.RoamingEnd, _
                         Wlan.WlanNotificationCodeMsm.Disassociating, Wlan.WlanNotificationCodeMsm.Disconnected, Wlan.WlanNotificationCodeMsm.PeerJoin, Wlan.WlanNotificationCodeMsm.PeerLeave, Wlan.WlanNotificationCodeMsm.AdapterRemoval
                            Dim connNotifyData As System.Nullable(Of Wlan.WlanConnectionNotificationData) = ParseWlanConnectionNotification(notifyData)
                            If connNotifyData.HasValue Then
                                If wlanIface IsNot Nothing Then
                                    wlanIface.OnWlanConnection(notifyData, connNotifyData.Value)
                                End If
                            End If
                            Exit Select
                    End Select
                    Exit Select
            End Select

            If wlanIface IsNot Nothing Then
                wlanIface.OnWlanNotification(notifyData)
            End If
        End Sub

        ''' <summary>
        ''' Gets the WLAN interfaces.
        ''' </summary>
        ''' <value>The WLAN interfaces.</value>
        Public ReadOnly Property Interfaces() As WlanInterface()
            Get
                Dim ifaceList As IntPtr
                Wlan.ThrowIfError(Wlan.WlanEnumInterfaces(clientHandle, IntPtr.Zero, ifaceList))
                Try
                    Dim header As Wlan.WlanInterfaceInfoListHeader = DirectCast(Marshal.PtrToStructure(ifaceList, GetType(Wlan.WlanInterfaceInfoListHeader)), Wlan.WlanInterfaceInfoListHeader)
                    Dim listIterator As Int64 = ifaceList.ToInt64() + Marshal.SizeOf(header)
                    Dim interfaces__1 As WlanInterface() = New WlanInterface(header.numberOfItems - 1) {}
                    Dim currentIfaceGuids As New List(Of Guid)()
                    For i As Integer = 0 To header.numberOfItems - 1
                        Dim info As Wlan.WlanInterfaceInfo = DirectCast(Marshal.PtrToStructure(New IntPtr(listIterator), GetType(Wlan.WlanInterfaceInfo)), Wlan.WlanInterfaceInfo)
                        listIterator += Marshal.SizeOf(info)
                        Dim wlanIface As WlanInterface
                        currentIfaceGuids.Add(info.interfaceGuid)
                        If ifaces.ContainsKey(info.interfaceGuid) Then
                            wlanIface = ifaces(info.interfaceGuid)
                        Else
                            wlanIface = New WlanInterface(Me, info)
                        End If
                        interfaces__1(i) = wlanIface
                        ifaces(info.interfaceGuid) = wlanIface
                    Next

                    ' Remove stale interfaces
                    Dim deadIfacesGuids As New Queue(Of Guid)()
                    For Each ifaceGuid As Guid In ifaces.Keys
                        If Not currentIfaceGuids.Contains(ifaceGuid) Then
                            deadIfacesGuids.Enqueue(ifaceGuid)
                        End If
                    Next
                    While deadIfacesGuids.Count <> 0
                        Dim deadIfaceGuid As Guid = deadIfacesGuids.Dequeue()
                        ifaces.Remove(deadIfaceGuid)
                    End While

                    Return interfaces__1
                Finally
                    Wlan.WlanFreeMemory(ifaceList)
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Gets a string that describes a specified reason code.
        ''' </summary>
        ''' <param name="reasonCode">The reason code.</param>
        ''' <returns>The string.</returns>
        Public Function GetStringForReasonCode(ByVal reasonCode As Wlan.WlanReasonCode) As String
            Dim sb As New StringBuilder(1024)
            ' the 1024 size here is arbitrary; the WlanReasonCodeToString docs fail to specify a recommended size
            Wlan.ThrowIfError(Wlan.WlanReasonCodeToString(reasonCode, sb.Capacity, sb, IntPtr.Zero))
            Return sb.ToString()
        End Function
    End Class
End Namespace