Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Diagnostics
Imports System.ComponentModel

Namespace NativeWifi
    ' TODO: Separate the functions and the structs/enums. Many of the structs/enums should remain public
    ' (since they're reused in the OOP interfaces) -- the rest (including all P/Invoke function mappings)
    ' should become internal.

    ' All structures which native methods rely on should be kept in the Wlan class.
    ' Only change the layout of those structures if it matches the native API documentation.
    ' Some structures might have helper properties but adding or changing fields is prohibited.
    ' This class is not documented since all the documentation resides in the MSDN. The code
    ' documentation only covers details which concern interop users.
    ' Some identifier names were modified to correspond to .NET naming conventions
    ' but otherwise retain their native meaning.

    ''' <summary>
    ''' Defines the Native Wifi API through P/Invoke interop.
    ''' </summary>
    ''' <remarks>
    ''' This class is intended for internal use. Use the <see cref="WlanCliient"/> class instead.
    ''' </remarks>
    Public NotInheritable Class Wlan
        Private Sub New()
        End Sub
#Region "P/Invoke API"
        ''' <summary>
        ''' Defines various opcodes used to set and query parameters for an interface.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_INTF_OPCODE</c> type.
        ''' </remarks>
        Public Enum WlanIntfOpcode
            ''' <summary>
            ''' Opcode used to set or query whether auto config is enabled.
            ''' </summary>
            AutoconfEnabled = 1
            ''' <summary>
            ''' Opcode used to set or query whether background scan is enabled.
            ''' </summary>
            BackgroundScanEnabled
            ''' <summary>
            ''' Opcode used to set or query the media streaming mode of the driver.
            ''' </summary>
            MediaStreamingMode
            ''' <summary>
            ''' Opcode used to set or query the radio state.
            ''' </summary>
            RadioState
            ''' <summary>
            ''' Opcode used to set or query the BSS type of the interface.
            ''' </summary>
            BssType
            ''' <summary>
            ''' Opcode used to query the state of the interface.
            ''' </summary>
            InterfaceState
            ''' <summary>
            ''' Opcode used to query information about the current connection of the interface.
            ''' </summary>
            CurrentConnection
            ''' <summary>
            ''' Opcose used to query the current channel on which the wireless interface is operating.
            ''' </summary>
            ChannelNumber
            ''' <summary>
            ''' Opcode used to query the supported auth/cipher pairs for infrastructure mode.
            ''' </summary>
            SupportedInfrastructureAuthCipherPairs
            ''' <summary>
            ''' Opcode used to query the supported auth/cipher pairs for ad hoc mode.
            ''' </summary>
            SupportedAdhocAuthCipherPairs
            ''' <summary>
            ''' Opcode used to query the list of supported country or region strings.
            ''' </summary>
            SupportedCountryOrRegionStringList
            ''' <summary>
            ''' Opcode used to set or query the current operation mode of the wireless interface.
            ''' </summary>
            CurrentOperationMode
            ''' <summary>
            ''' Opcode used to query driver statistics.
            ''' </summary>
            Statistics = &H10000101
            ''' <summary>
            ''' Opcode used to query the received signal strength.
            ''' </summary>
            RSSI
            SecurityStart = &H20010000
            SecurityEnd = &H2FFFFFFF
            IhvStart = &H30000000
            IhvEnd = &H3FFFFFFF
        End Enum

        ''' <summary>
        ''' Specifies the origin of automatic configuration (auto config) settings.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_OPCODE_VALUE_TYPE</c> type.
        ''' </remarks>
        Public Enum WlanOpcodeValueType
            ''' <summary>
            ''' The auto config settings were queried, but the origin of the settings was not determined.
            ''' </summary>
            QueryOnly = 0
            ''' <summary>
            ''' The auto config settings were set by group policy.
            ''' </summary>
            SetByGroupPolicy = 1
            ''' <summary>
            ''' The auto config settings were set by the user.
            ''' </summary>
            SetByUser = 2
            ''' <summary>
            ''' The auto config settings are invalid.
            ''' </summary>
            Invalid = 3
        End Enum

        Public Const WLAN_CLIENT_VERSION_XP_SP2 As UInteger = 1
        Public Const WLAN_CLIENT_VERSION_LONGHORN As UInteger = 2

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanOpenHandle(<[In]()> ByVal clientVersion As UInt32, <[In](), Out()> ByVal pReserved As IntPtr, <Out()> ByRef negotiatedVersion As UInt32, <Out()> ByRef clientHandle As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanCloseHandle(<[In]()> ByVal clientHandle As IntPtr, <[In](), Out()> ByVal pReserved As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanEnumInterfaces(<[In]()> ByVal clientHandle As IntPtr, <[In](), Out()> ByVal pReserved As IntPtr, <Out()> ByRef ppInterfaceList As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanQueryInterface(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal opCode As WlanIntfOpcode, <[In](), Out()> ByVal pReserved As IntPtr, <Out()> ByRef dataSize As Integer, <Out()> ByRef ppData As IntPtr, _
   <Out()> ByRef wlanOpcodeValueType As WlanOpcodeValueType) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanSetInterface(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal opCode As WlanIntfOpcode, <[In]()> ByVal dataSize As UInteger, <[In]()> ByVal pData As IntPtr, <[In](), Out()> ByVal pReserved As IntPtr) As Integer
        End Function

        ''' <param name="pDot11Ssid">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
        ''' <param name="pIeData">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanScan(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal pDot11Ssid As IntPtr, <[In]()> ByVal pIeData As IntPtr, <[In](), Out()> ByVal pReserved As IntPtr) As Integer
        End Function

        ''' <summary>
        ''' Defines flags passed to <see cref="WlanGetAvailableNetworkList"/>.
        ''' </summary>
        <Flags()> _
        Public Enum WlanGetAvailableNetworkFlags
            ''' <summary>
            ''' Include all ad-hoc network profiles in the available network list, including profiles that are not visible.
            ''' </summary>
            IncludeAllAdhocProfiles = &H1
            ''' <summary>
            ''' Include all hidden network profiles in the available network list, including profiles that are not visible.
            ''' </summary>
            IncludeAllManualHiddenProfiles = &H2
        End Enum

        ''' <summary>
        ''' The header of an array of information about available networks.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure WlanAvailableNetworkListHeader
            ''' <summary>
            ''' Contains the number of <see cref=""/> items following the header.
            ''' </summary>
            Public numberOfItems As UInteger
            ''' <summary>
            ''' The index of the current item. The index of the first item is 0.
            ''' </summary>
            Public index As UInteger
        End Structure

        ''' <summary>
        ''' Contains various flags for the network.
        ''' </summary>
        <Flags()> _
        Public Enum WlanAvailableNetworkFlags
            ''' <summary>
            ''' This network is currently connected.
            ''' </summary>
            Connected = &H1
            ''' <summary>
            ''' There is a profile for this network.
            ''' </summary>
            HasProfile = &H2
        End Enum

        ''' <summary>
        ''' Contains information about an available wireless network.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure WlanAvailableNetwork
            ''' <summary>
            ''' Contains the profile name associated with the network.
            ''' If the network doesn't have a profile, this member will be empty.
            ''' If multiple profiles are associated with the network, there will be multiple entries with the same SSID in the visible network list. Profile names are case-sensitive.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)> _
            Public profileName As String
            ''' <summary>
            ''' Contains the SSID of the visible wireless network.
            ''' </summary>
            Public dot11Ssid As Dot11Ssid
            ''' <summary>
            ''' Specifies whether the network is infrastructure or ad hoc.
            ''' </summary>
            Public dot11BssType As Dot11BssType
            ''' <summary>
            ''' Indicates the number of BSSIDs in the network.
            ''' </summary>
            Public numberOfBssids As UInteger
            ''' <summary>
            ''' Indicates whether the network is connectable or not.
            ''' </summary>
            Public networkConnectable As Boolean
            ''' <summary>
            ''' Indicates why a network cannot be connected to. This member is only valid when <see cref="networkConnectable"/> is <c>false</c>.
            ''' </summary>
            Public wlanNotConnectableReason As WlanReasonCode
            ''' <summary>
            ''' The number of PHY types supported on available networks.
            ''' The maximum value of this field is 8. If more than 8 PHY types are supported, <see cref="morePhyTypes"/> must be set to <c>true</c>.
            ''' </summary>
            Private numberOfPhyTypes As UInteger
            ''' <summary>
            ''' Contains an array of <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
            ''' When <see cref="numberOfPhyTypes"/> is greater than 8, this array contains only the first 8 PHY types.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=8)> _
            Private m_dot11PhyTypes As Dot11PhyType()
            ''' <summary>
            ''' Gets the <see cref="Dot11PhyType"/> values that represent the PHY types supported by the available networks.
            ''' </summary>
            Public ReadOnly Property Dot11PhyTypes() As Dot11PhyType()
                Get
                    Dim ret As Dot11PhyType() = New Dot11PhyType(numberOfPhyTypes - 1) {}
                    Array.Copy(m_dot11PhyTypes, ret, numberOfPhyTypes)
                    Return ret
                End Get
            End Property
            ''' <summary>
            ''' Specifies if there are more than 8 PHY types supported.
            ''' When this member is set to <c>true</c>, an application must call <see cref="WlanClient.WlanInterface.GetNetworkBssList"/> to get the complete list of PHY types.
            ''' <see cref="WlanBssEntry.phyId"/> contains the PHY type for an entry.
            ''' </summary>
            Public morePhyTypes As Boolean
            ''' <summary>
            ''' A percentage value that represents the signal quality of the network.
            ''' This field contains a value between 0 and 100.
            ''' A value of 0 implies an actual RSSI signal strength of -100 dbm.
            ''' A value of 100 implies an actual RSSI signal strength of -50 dbm.
            ''' You can calculate the RSSI signal strength value for values between 1 and 99 using linear interpolation.
            ''' </summary>
            Public wlanSignalQuality As UInteger
            ''' <summary>
            ''' Indicates whether security is enabled on the network.
            ''' </summary>
            Public securityEnabled As Boolean
            ''' <summary>
            ''' Indicates the default authentication algorithm used to join this network for the first time.
            ''' </summary>
            Public dot11DefaultAuthAlgorithm As Dot11AuthAlgorithm
            ''' <summary>
            ''' Indicates the default cipher algorithm to be used when joining this network.
            ''' </summary>
            Public dot11DefaultCipherAlgorithm As Dot11CipherAlgorithm
            ''' <summary>
            ''' Contains various flags for the network.
            ''' </summary>
            Public flags As WlanAvailableNetworkFlags
            ''' <summary>
            ''' Reserved for future use. Must be set to NULL.
            ''' </summary>
            Private reserved As UInteger
        End Structure

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanGetAvailableNetworkList(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal flags As WlanGetAvailableNetworkFlags, <[In](), Out()> ByVal reservedPtr As IntPtr, <Out()> ByRef availableNetworkListPtr As IntPtr) As Integer
        End Function

        <Flags()> _
        Public Enum WlanProfileFlags
            ''' <remarks>
            ''' The only option available on Windows XP SP2.
            ''' </remarks>
            AllUser = 0
            GroupPolicy = 1
            User = 2
        End Enum

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanSetProfile(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal flags As WlanProfileFlags, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal profileXml As String, <[In](), [Optional](), MarshalAs(UnmanagedType.LPWStr)> ByVal allUserProfileSecurity As String, <[In]()> ByVal overwrite As Boolean, _
   <[In]()> ByVal pReserved As IntPtr, <Out()> ByRef reasonCode As WlanReasonCode) As Integer
        End Function

        ''' <summary>
        ''' Defines the access mask of an all-user profile.
        ''' </summary>
        <Flags()> _
        Public Enum WlanAccess
            ''' <summary>
            ''' The user can view profile permissions.
            ''' </summary>
            ReadAccess = &H20000 Or &H1
            ''' <summary>
            ''' The user has read access, and the user can also connect to and disconnect from a network using the profile.
            ''' </summary>
            ExecuteAccess = ReadAccess Or &H20
            ''' <summary>
            ''' The user has execute access and the user can also modify and delete permissions associated with a profile.
            ''' </summary>
            WriteAccess = ReadAccess Or ExecuteAccess Or &H2 Or &H10000 Or &H40000
        End Enum

        ''' <param name="flags">Not supported on Windows XP SP2: must be a <c>null</c> reference.</param>
        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanGetProfile(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal profileName As String, <[In]()> ByVal pReserved As IntPtr, <Out()> ByRef profileXml As IntPtr, <Out(), [Optional]()> ByRef flags As WlanProfileFlags, _
   <Out(), [Optional]()> ByRef grantedAccess As WlanAccess) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanGetProfileList(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal pReserved As IntPtr, <Out()> ByRef profileList As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Sub WlanFreeMemory(ByVal pMemory As IntPtr)
        End Sub

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanReasonCodeToString(<[In]()> ByVal reasonCode As WlanReasonCode, <[In]()> ByVal bufferSize As Integer, <[In](), Out()> ByVal stringBuffer As StringBuilder, ByVal pReserved As IntPtr) As Integer
        End Function

        ''' <summary>
        ''' Specifies where the notification comes from.
        ''' </summary>
        <Flags()> _
        Public Enum WlanNotificationSource
            None = 0
            ''' <summary>
            ''' All notifications, including those generated by the 802.1X module.
            ''' </summary>
            All = &HFFFF
            ''' <summary>
            ''' Notifications generated by the auto configuration module.
            ''' </summary>
            ACM = &H8
            ''' <summary>
            ''' Notifications generated by MSM.
            ''' </summary>
            MSM = &H10
            ''' <summary>
            ''' Notifications generated by the security module.
            ''' </summary>
            Security = &H20
            ''' <summary>
            ''' Notifications generated by independent hardware vendors (IHV).
            ''' </summary>
            IHV = &H40
        End Enum

        ''' <summary>
        ''' Indicates the type of an ACM (<see cref="WlanNotificationSource.ACM"/>) notification.
        ''' </summary>
        ''' <remarks>
        ''' The enumeration identifiers correspond to the native <c>wlan_notification_acm_</c> identifiers.
        ''' On Windows XP SP2, only the <c>ConnectionComplete</c> and <c>Disconnected</c> notifications are available.
        ''' </remarks>
        Public Enum WlanNotificationCodeAcm
            AutoconfEnabled = 1
            AutoconfDisabled
            BackgroundScanEnabled
            BackgroundScanDisabled
            BssTypeChange
            PowerSettingChange
            ScanComplete
            ScanFail
            ConnectionStart
            ConnectionComplete
            ConnectionAttemptFail
            FilterListChange
            InterfaceArrival
            InterfaceRemoval
            ProfileChange
            ProfileNameChange
            ProfilesExhausted
            NetworkNotAvailable
            NetworkAvailable
            Disconnecting
            Disconnected
            AdhocNetworkStateChange
        End Enum

        ''' <summary>
        ''' Indicates the type of an MSM (<see cref="WlanNotificationSource.MSM"/>) notification.
        ''' </summary>
        ''' <remarks>
        ''' The enumeration identifiers correspond to the native <c>wlan_notification_msm_</c> identifiers.
        ''' </remarks>
        Public Enum WlanNotificationCodeMsm
            Associating = 1
            Associated
            Authenticating
            Connected
            RoamingStart
            RoamingEnd
            RadioStateChange
            SignalQualityChange
            Disassociating
            Disconnected
            PeerJoin
            PeerLeave
            AdapterRemoval
            AdapterOperationModeChange
        End Enum

        ''' <summary>
        ''' Contains information provided when registering for notifications.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_NOTIFICATION_DATA</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanNotificationData
            ''' <summary>
            ''' Specifies where the notification comes from.
            ''' </summary>
            ''' <remarks>
            ''' On Windows XP SP2, this field must be set to <see cref="WlanNotificationSource.None"/>, <see cref="WlanNotificationSource.All"/> or <see cref="WlanNotificationSource.ACM"/>.
            ''' </remarks>
            Public notificationSource As WlanNotificationSource
            ''' <summary>
            ''' Indicates the type of notification. The value of this field indicates what type of associated data will be present in <see cref="dataPtr"/>.
            ''' </summary>
            Public m_notificationCode As Integer
            ''' <summary>
            ''' Indicates which interface the notification is for.
            ''' </summary>
            Public interfaceGuid As Guid
            ''' <summary>
            ''' Specifies the size of <see cref="dataPtr"/>, in bytes.
            ''' </summary>
            Public dataSize As Integer
            ''' <summary>
            ''' Pointer to additional data needed for the notification, as indicated by <see cref="notificationCode"/>.
            ''' </summary>
            Public dataPtr As IntPtr

            ''' <summary>
            ''' Gets the notification code (in the correct enumeration type) according to the notification source.
            ''' </summary>
            Public ReadOnly Property NotificationCode() As Object
                Get
                    If notificationSource = WlanNotificationSource.MSM Then
                        Return CType(m_notificationCode, WlanNotificationCodeMsm)
                    ElseIf notificationSource = WlanNotificationSource.ACM Then
                        Return CType(m_notificationCode, WlanNotificationCodeAcm)
                    Else
                        Return m_notificationCode
                    End If
                End Get
            End Property

        End Structure

        ''' <summary>
        ''' Defines the callback function which accepts WLAN notifications.
        ''' </summary>
        Public Delegate Sub WlanNotificationCallbackDelegate(ByRef notificationData As WlanNotificationData, ByVal context As IntPtr)

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanRegisterNotification(<[In]()> ByVal clientHandle As IntPtr, <[In]()> ByVal notifSource As WlanNotificationSource, <[In]()> ByVal ignoreDuplicate As Boolean, <[In]()> ByVal funcCallback As WlanNotificationCallbackDelegate, <[In]()> ByVal callbackContext As IntPtr, <[In]()> ByVal reserved As IntPtr, _
   <Out()> ByRef prevNotifSource As WlanNotificationSource) As Integer
        End Function

        ''' <summary>
        ''' Defines connection parameter flags.
        ''' </summary>
        <Flags()> _
        Public Enum WlanConnectionFlags
            ''' <summary>
            ''' Connect to the destination network even if the destination is a hidden network. A hidden network does not broadcast its SSID. Do not use this flag if the destination network is an ad-hoc network.
            ''' <para>If the profile specified by <see cref="WlanConnectionParameters.profile"/> is not <c>null</c>, then this flag is ignored and the nonBroadcast profile element determines whether to connect to a hidden network.</para>
            ''' </summary>
            HiddenNetwork = &H1
            ''' <summary>
            ''' Do not form an ad-hoc network. Only join an ad-hoc network if the network already exists. Do not use this flag if the destination network is an infrastructure network.
            ''' </summary>
            AdhocJoinOnly = &H2
            ''' <summary>
            ''' Ignore the privacy bit when connecting to the network. Ignoring the privacy bit has the effect of ignoring whether packets are encryption and ignoring the method of encryption used. Only use this flag when connecting to an infrastructure network using a temporary profile.
            ''' </summary>
            IgnorePrivacyBit = &H4
            ''' <summary>
            ''' Exempt EAPOL traffic from encryption and decryption. This flag is used when an application must send EAPOL traffic over an infrastructure network that uses Open authentication and WEP encryption. This flag must not be used to connect to networks that require 802.1X authentication. This flag is only valid when <see cref="WlanConnectionParameters.wlanConnectionMode"/> is set to <see cref="WlanConnectionMode.TemporaryProfile"/>. Avoid using this flag whenever possible.
            ''' </summary>
            EapolPassthrough = &H8
        End Enum

        ''' <summary>
        ''' Specifies the parameters used when using the <see cref="WlanConnect"/> function.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_CONNECTION_PARAMETERS</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanConnectionParameters
            ''' <summary>
            ''' Specifies the mode of connection.
            ''' </summary>
            Public wlanConnectionMode As WlanConnectionMode
            ''' <summary>
            ''' Specifies the profile being used for the connection.
            ''' The contents of the field depend on the <see cref="wlanConnectionMode"/>:
            ''' <list type="table">
            ''' <listheader>
            ''' <term>Value of <see cref="wlanConnectionMode"/></term>
            ''' <description>Contents of the profile string</description>
            ''' </listheader>
            ''' <item>
            ''' <term><see cref="WlanConnectionMode.Profile"/></term>
            ''' <description>The name of the profile used for the connection.</description>
            ''' </item>
            ''' <item>
            ''' <term><see cref="WlanConnectionMode.TemporaryProfile"/></term>
            ''' <description>The XML representation of the profile used for the connection.</description>
            ''' </item>
            ''' <item>
            ''' <term><see cref="WlanConnectionMode.DiscoverySecure"/>, <see cref="WlanConnectionMode.DiscoveryUnsecure"/> or <see cref="WlanConnectionMode.Auto"/></term>
            ''' <description><c>null</c></description>
            ''' </item>
            ''' </list>
            ''' </summary>
            <MarshalAs(UnmanagedType.LPWStr)> _
            Public profile As String
            ''' <summary>
            ''' Pointer to a <see cref="Dot11Ssid"/> structure that specifies the SSID of the network to connect to.
            ''' This field is optional. When set to <c>null</c>, all SSIDs in the profile will be tried.
            ''' This field must not be <c>null</c> if <see cref="wlanConnectionMode"/> is set to <see cref="WlanConnectionMode.DiscoverySecure"/> or <see cref="WlanConnectionMode.DiscoveryUnsecure"/>.
            ''' </summary>
            Public dot11SsidPtr As IntPtr
            ''' <summary>
            ''' Pointer to a <see cref="Dot11BssidList"/> structure that contains the list of basic service set (BSS) identifiers desired for the connection.
            ''' </summary>
            ''' <remarks>
            ''' On Windows XP SP2, must be set to <c>null</c>.
            ''' </remarks>
            Public desiredBssidListPtr As IntPtr
            ''' <summary>
            ''' A <see cref="Dot11BssType"/> value that indicates the BSS type of the network. If a profile is provided, this BSS type must be the same as the one in the profile.
            ''' </summary>
            Public dot11BssType As Dot11BssType
            ''' <summary>
            ''' Specifies ocnnection parameters.
            ''' </summary>
            ''' <remarks>
            ''' On Windows XP SP2, must be set to 0.
            ''' </remarks>
            Public flags As WlanConnectionFlags
        End Structure

        ''' <summary>
        ''' The connection state of an ad hoc network.
        ''' </summary>
        Public Enum WlanAdhocNetworkState
            ''' <summary>
            ''' The ad hoc network has been formed, but no client or host is connected to the network.
            ''' </summary>
            Formed = 0
            ''' <summary>
            ''' A client or host is connected to the ad hoc network.
            ''' </summary>
            Connected = 1
        End Enum

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanConnect(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByRef connectionParameters As WlanConnectionParameters, ByVal pReserved As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanDeleteProfile(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal profileName As String, ByVal reservedPtr As IntPtr) As Integer
        End Function

        <DllImport("wlanapi.dll")> _
        Public Shared Function WlanGetNetworkBssList(<[In]()> ByVal clientHandle As IntPtr, <[In](), MarshalAs(UnmanagedType.LPStruct)> ByVal interfaceGuid As Guid, <[In]()> ByVal dot11SsidInt As IntPtr, <[In]()> ByVal dot11BssType As Dot11BssType, <[In]()> ByVal securityEnabled As Boolean, ByVal reservedPtr As IntPtr, _
   <Out()> ByRef wlanBssList As IntPtr) As Integer
        End Function

        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure WlanBssListHeader
            Friend totalSize As UInteger
            Friend numberOfItems As UInteger
        End Structure

        ''' <summary>
        ''' Contains information about a basic service set (BSS).
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanBssEntry
            ''' <summary>
            ''' Contains the SSID of the access point (AP) associated with the BSS.
            ''' </summary>
            Public dot11Ssid As Dot11Ssid
            ''' <summary>
            ''' The identifier of the PHY on which the AP is operating.
            ''' </summary>
            Public phyId As UInteger
            ''' <summary>
            ''' Contains the BSS identifier.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)> _
            Public dot11Bssid As Byte()
            ''' <summary>
            ''' Specifies whether the network is infrastructure or ad hoc.
            ''' </summary>
            Public dot11BssType As Dot11BssType
            Public dot11BssPhyType As Dot11PhyType
            ''' <summary>
            ''' The received signal strength in dBm.
            ''' </summary>
            Public rssi As Integer
            ''' <summary>
            ''' The link quality reported by the driver. Ranges from 0-100.
            ''' </summary>
            Public linkQuality As UInteger
            ''' <summary>
            ''' If 802.11d is not implemented, the network interface card (NIC) must set this field to TRUE. If 802.11d is implemented (but not necessarily enabled), the NIC must set this field to TRUE if the BSS operation complies with the configured regulatory domain.
            ''' </summary>
            Public inRegDomain As Boolean
            ''' <summary>
            ''' Contains the beacon interval value from the beacon packet or probe response.
            ''' </summary>
            Public beaconPeriod As UShort
            ''' <summary>
            ''' The timestamp from the beacon packet or probe response.
            ''' </summary>
            Public timestamp As ULong
            ''' <summary>
            ''' The host timestamp value when the beacon or probe response is received.
            ''' </summary>
            Public hostTimestamp As ULong
            ''' <summary>
            ''' The capability value from the beacon packet or probe response.
            ''' </summary>
            Public capabilityInformation As UShort
            ''' <summary>
            ''' The frequency of the center channel, in kHz.
            ''' </summary>
            Public chCenterFrequency As UInteger
            ''' <summary>
            ''' Contains the set of data transfer rates supported by the BSS.
            ''' </summary>
            Public wlanRateSet As WlanRateSet
            ''' <summary>
            ''' Offset of the information element (IE) data blob.
            ''' </summary>
            Public ieOffset As UInteger
            ''' <summary>
            ''' Size of the IE data blob, in bytes.
            ''' </summary>
            Public ieSize As UInteger
        End Structure

        ''' <summary>
        ''' Contains the set of supported data rates.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanRateSet
            ''' <summary>
            ''' The length, in bytes, of <see cref="rateSet"/>.
            ''' </summary>
            Private rateSetLength As UInteger
            ''' <summary>
            ''' An array of supported data transfer rates.
            ''' If the rate is a basic rate, the first bit of the rate value is set to 1.
            ''' A basic rate is the data transfer rate that all stations in a basic service set (BSS) can use to receive frames from the wireless medium.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=126)> _
            Private rateSet As UShort()

            Public ReadOnly Property Rates() As UShort()
                Get
                    Dim rates__1 As UShort() = New UShort(rateSetLength / 2 - 1) {}
                    Array.Copy(rateSet, rates__1, rates__1.Length)
                    Return rates__1
                End Get
            End Property

            ''' <summary>
            ''' CalculateS the data transfer rate in Mbps for an arbitrary supported rate.
            ''' </summary>
            ''' <param name="rate"></param>
            ''' <returns></returns>
            Public Function GetRateInMbps(ByVal rate As Integer) As Double
                Return (rateSet(rate) And &H7FFF) * 0.5
            End Function
        End Structure

        ''' <summary>
        ''' Represents an error occuring during WLAN operations which indicate their failure via a <see cref="WlanReasonCode"/>.
        ''' </summary>
        Public Class WlanException
            Inherits Exception
            Private m_reasonCode As WlanReasonCode

            Private Sub New(ByVal reasonCode As WlanReasonCode)
                Me.m_reasonCode = reasonCode
            End Sub

            ''' <summary>
            ''' Gets the WLAN reason code.
            ''' </summary>
            ''' <value>The WLAN reason code.</value>
            Public ReadOnly Property ReasonCode() As WlanReasonCode
                Get
                    Return m_reasonCode
                End Get
            End Property

            ''' <summary>
            ''' Gets a message that describes the reason code.
            ''' </summary>
            ''' <value></value>
            ''' <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
            Public Overrides ReadOnly Property Message() As String
                Get
                    Dim sb As New StringBuilder(1024)
                    If WlanReasonCodeToString(m_reasonCode, sb.Capacity, sb, IntPtr.Zero) = 0 Then
                        Return sb.ToString()
                    Else
                        Return String.Empty
                    End If
                End Get
            End Property
        End Class

        ' TODO: .NET-ify the WlanReasonCode enum (naming convention + docs).

        ''' <summary>
        ''' Specifies reasons for a failure of a WLAN operation.
        ''' </summary>
        ''' <remarks>
        ''' To get the WLAN API native reason code identifiers, prefix the identifiers with <c>WLAN_REASON_CODE_</c>.
        ''' </remarks>
        Public Enum WlanReasonCode
            Success = 0
            ' general codes
            UNKNOWN = &H10000 + 1

            RANGE_SIZE = &H10000
            BASE = &H10000 + RANGE_SIZE

            ' range for Auto Config
            '
            AC_BASE = &H10000 + RANGE_SIZE
            AC_CONNECT_BASE = (AC_BASE + RANGE_SIZE / 2)
            AC_END = (AC_BASE + RANGE_SIZE - 1)

            ' range for profile manager
            ' it has profile adding failure reason codes, but may not have 
            ' connection reason codes
            '
            PROFILE_BASE = &H10000 + (7 * RANGE_SIZE)
            PROFILE_CONNECT_BASE = (PROFILE_BASE + RANGE_SIZE / 2)
            PROFILE_END = (PROFILE_BASE + RANGE_SIZE - 1)

            ' range for MSM
            '
            MSM_BASE = &H10000 + (2 * RANGE_SIZE)
            MSM_CONNECT_BASE = (MSM_BASE + RANGE_SIZE / 2)
            MSM_END = (MSM_BASE + RANGE_SIZE - 1)

            ' range for MSMSEC
            '
            MSMSEC_BASE = &H10000 + (3 * RANGE_SIZE)
            MSMSEC_CONNECT_BASE = (MSMSEC_BASE + RANGE_SIZE / 2)
            MSMSEC_END = (MSMSEC_BASE + RANGE_SIZE - 1)

            ' AC network incompatible reason codes
            '
            NETWORK_NOT_COMPATIBLE = (AC_BASE + 1)
            PROFILE_NOT_COMPATIBLE = (AC_BASE + 2)

            ' AC connect reason code
            '
            NO_AUTO_CONNECTION = (AC_CONNECT_BASE + 1)
            NOT_VISIBLE = (AC_CONNECT_BASE + 2)
            GP_DENIED = (AC_CONNECT_BASE + 3)
            USER_DENIED = (AC_CONNECT_BASE + 4)
            BSS_TYPE_NOT_ALLOWED = (AC_CONNECT_BASE + 5)
            IN_FAILED_LIST = (AC_CONNECT_BASE + 6)
            IN_BLOCKED_LIST = (AC_CONNECT_BASE + 7)
            SSID_LIST_TOO_LONG = (AC_CONNECT_BASE + 8)
            CONNECT_CALL_FAIL = (AC_CONNECT_BASE + 9)
            SCAN_CALL_FAIL = (AC_CONNECT_BASE + 10)
            NETWORK_NOT_AVAILABLE = (AC_CONNECT_BASE + 11)
            PROFILE_CHANGED_OR_DELETED = (AC_CONNECT_BASE + 12)
            KEY_MISMATCH = (AC_CONNECT_BASE + 13)
            USER_NOT_RESPOND = (AC_CONNECT_BASE + 14)

            ' Profile validation errors
            '
            INVALID_PROFILE_SCHEMA = (PROFILE_BASE + 1)
            PROFILE_MISSING = (PROFILE_BASE + 2)
            INVALID_PROFILE_NAME = (PROFILE_BASE + 3)
            INVALID_PROFILE_TYPE = (PROFILE_BASE + 4)
            INVALID_PHY_TYPE = (PROFILE_BASE + 5)
            MSM_SECURITY_MISSING = (PROFILE_BASE + 6)
            IHV_SECURITY_NOT_SUPPORTED = (PROFILE_BASE + 7)
            IHV_OUI_MISMATCH = (PROFILE_BASE + 8)
            ' IHV OUI not present but there is IHV settings in profile
            IHV_OUI_MISSING = (PROFILE_BASE + 9)
            ' IHV OUI is present but there is no IHV settings in profile
            IHV_SETTINGS_MISSING = (PROFILE_BASE + 10)
            ' both/conflict MSMSec and IHV security settings exist in profile 
            CONFLICT_SECURITY = (PROFILE_BASE + 11)
            ' no IHV or MSMSec security settings in profile
            SECURITY_MISSING = (PROFILE_BASE + 12)
            INVALID_BSS_TYPE = (PROFILE_BASE + 13)
            INVALID_ADHOC_CONNECTION_MODE = (PROFILE_BASE + 14)
            NON_BROADCAST_SET_FOR_ADHOC = (PROFILE_BASE + 15)
            AUTO_SWITCH_SET_FOR_ADHOC = (PROFILE_BASE + 16)
            AUTO_SWITCH_SET_FOR_MANUAL_CONNECTION = (PROFILE_BASE + 17)
            IHV_SECURITY_ONEX_MISSING = (PROFILE_BASE + 18)
            PROFILE_SSID_INVALID = (PROFILE_BASE + 19)
            TOO_MANY_SSID = (PROFILE_BASE + 20)

            ' MSM network incompatible reasons
            '
            UNSUPPORTED_SECURITY_SET_BY_OS = (MSM_BASE + 1)
            UNSUPPORTED_SECURITY_SET = (MSM_BASE + 2)
            BSS_TYPE_UNMATCH = (MSM_BASE + 3)
            PHY_TYPE_UNMATCH = (MSM_BASE + 4)
            DATARATE_UNMATCH = (MSM_BASE + 5)

            ' MSM connection failure reasons, to be defined
            ' failure reason codes
            '
            ' user called to disconnect
            USER_CANCELLED = (MSM_CONNECT_BASE + 1)
            ' got disconnect while associating
            ASSOCIATION_FAILURE = (MSM_CONNECT_BASE + 2)
            ' timeout for association
            ASSOCIATION_TIMEOUT = (MSM_CONNECT_BASE + 3)
            ' pre-association security completed with failure
            PRE_SECURITY_FAILURE = (MSM_CONNECT_BASE + 4)
            ' fail to start post-association security
            START_SECURITY_FAILURE = (MSM_CONNECT_BASE + 5)
            ' post-association security completed with failure
            SECURITY_FAILURE = (MSM_CONNECT_BASE + 6)
            ' security watchdog timeout
            SECURITY_TIMEOUT = (MSM_CONNECT_BASE + 7)
            ' got disconnect from driver when roaming
            ROAMING_FAILURE = (MSM_CONNECT_BASE + 8)
            ' failed to start security for roaming
            ROAMING_SECURITY_FAILURE = (MSM_CONNECT_BASE + 9)
            ' failed to start security for adhoc-join
            ADHOC_SECURITY_FAILURE = (MSM_CONNECT_BASE + 10)
            ' got disconnection from driver
            DRIVER_DISCONNECTED = (MSM_CONNECT_BASE + 11)
            ' driver operation failed
            DRIVER_OPERATION_FAILURE = (MSM_CONNECT_BASE + 12)
            ' Ihv service is not available
            IHV_NOT_AVAILABLE = (MSM_CONNECT_BASE + 13)
            ' Response from ihv timed out
            IHV_NOT_RESPONDING = (MSM_CONNECT_BASE + 14)
            ' Timed out waiting for driver to disconnect
            DISCONNECT_TIMEOUT = (MSM_CONNECT_BASE + 15)
            ' An internal error prevented the operation from being completed.
            INTERNAL_FAILURE = (MSM_CONNECT_BASE + 16)
            ' UI Request timed out.
            UI_REQUEST_TIMEOUT = (MSM_CONNECT_BASE + 17)
            ' Roaming too often, post security is not completed after 5 times.
            TOO_MANY_SECURITY_ATTEMPTS = (MSM_CONNECT_BASE + 18)

            ' MSMSEC reason codes
            '

            MSMSEC_MIN = MSMSEC_BASE

            ' Key index specified is not valid
            MSMSEC_PROFILE_INVALID_KEY_INDEX = (MSMSEC_BASE + 1)
            ' Key required, PSK present
            MSMSEC_PROFILE_PSK_PRESENT = (MSMSEC_BASE + 2)
            ' Invalid key length
            MSMSEC_PROFILE_KEY_LENGTH = (MSMSEC_BASE + 3)
            ' Invalid PSK length
            MSMSEC_PROFILE_PSK_LENGTH = (MSMSEC_BASE + 4)
            ' No auth/cipher specified
            MSMSEC_PROFILE_NO_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 5)
            ' Too many auth/cipher specified
            MSMSEC_PROFILE_TOO_MANY_AUTH_CIPHER_SPECIFIED = (MSMSEC_BASE + 6)
            ' Profile contains duplicate auth/cipher
            MSMSEC_PROFILE_DUPLICATE_AUTH_CIPHER = (MSMSEC_BASE + 7)
            ' Profile raw data is invalid (1x or key data)
            MSMSEC_PROFILE_RAWDATA_INVALID = (MSMSEC_BASE + 8)
            ' Invalid auth/cipher combination
            MSMSEC_PROFILE_INVALID_AUTH_CIPHER = (MSMSEC_BASE + 9)
            ' 802.1x disabled when it's required to be enabled
            MSMSEC_PROFILE_ONEX_DISABLED = (MSMSEC_BASE + 10)
            ' 802.1x enabled when it's required to be disabled
            MSMSEC_PROFILE_ONEX_ENABLED = (MSMSEC_BASE + 11)
            MSMSEC_PROFILE_INVALID_PMKCACHE_MODE = (MSMSEC_BASE + 12)
            MSMSEC_PROFILE_INVALID_PMKCACHE_SIZE = (MSMSEC_BASE + 13)
            MSMSEC_PROFILE_INVALID_PMKCACHE_TTL = (MSMSEC_BASE + 14)
            MSMSEC_PROFILE_INVALID_PREAUTH_MODE = (MSMSEC_BASE + 15)
            MSMSEC_PROFILE_INVALID_PREAUTH_THROTTLE = (MSMSEC_BASE + 16)
            ' PreAuth enabled when PMK cache is disabled
            MSMSEC_PROFILE_PREAUTH_ONLY_ENABLED = (MSMSEC_BASE + 17)
            ' Capability matching failed at network
            MSMSEC_CAPABILITY_NETWORK = (MSMSEC_BASE + 18)
            ' Capability matching failed at NIC
            MSMSEC_CAPABILITY_NIC = (MSMSEC_BASE + 19)
            ' Capability matching failed at profile
            MSMSEC_CAPABILITY_PROFILE = (MSMSEC_BASE + 20)
            ' Network does not support specified discovery type
            MSMSEC_CAPABILITY_DISCOVERY = (MSMSEC_BASE + 21)
            ' Passphrase contains invalid character
            MSMSEC_PROFILE_PASSPHRASE_CHAR = (MSMSEC_BASE + 22)
            ' Key material contains invalid character
            MSMSEC_PROFILE_KEYMATERIAL_CHAR = (MSMSEC_BASE + 23)
            ' Wrong key type specified for the auth/cipher pair
            MSMSEC_PROFILE_WRONG_KEYTYPE = (MSMSEC_BASE + 24)
            ' "Mixed cell" suspected (AP not beaconing privacy, we have privacy enabled profile)
            MSMSEC_MIXED_CELL = (MSMSEC_BASE + 25)
            ' Auth timers or number of timeouts in profile is incorrect
            MSMSEC_PROFILE_AUTH_TIMERS_INVALID = (MSMSEC_BASE + 26)
            ' Group key update interval in profile is incorrect
            MSMSEC_PROFILE_INVALID_GKEY_INTV = (MSMSEC_BASE + 27)
            ' "Transition network" suspected, trying legacy 802.11 security
            MSMSEC_TRANSITION_NETWORK = (MSMSEC_BASE + 28)
            ' Key contains characters which do not map to ASCII
            MSMSEC_PROFILE_KEY_UNMAPPED_CHAR = (MSMSEC_BASE + 29)
            ' Capability matching failed at profile (auth not found)
            MSMSEC_CAPABILITY_PROFILE_AUTH = (MSMSEC_BASE + 30)
            ' Capability matching failed at profile (cipher not found)
            MSMSEC_CAPABILITY_PROFILE_CIPHER = (MSMSEC_BASE + 31)

            ' Failed to queue UI request
            MSMSEC_UI_REQUEST_FAILURE = (MSMSEC_CONNECT_BASE + 1)
            ' 802.1x authentication did not start within configured time 
            MSMSEC_AUTH_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 2)
            ' 802.1x authentication did not complete within configured time
            MSMSEC_AUTH_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 3)
            ' Dynamic key exchange did not start within configured time
            MSMSEC_KEY_START_TIMEOUT = (MSMSEC_CONNECT_BASE + 4)
            ' Dynamic key exchange did not succeed within configured time
            MSMSEC_KEY_SUCCESS_TIMEOUT = (MSMSEC_CONNECT_BASE + 5)
            ' Message 3 of 4 way handshake has no key data (RSN/WPA)
            MSMSEC_M3_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 6)
            ' Message 3 of 4 way handshake has no IE (RSN/WPA)
            MSMSEC_M3_MISSING_IE = (MSMSEC_CONNECT_BASE + 7)
            ' Message 3 of 4 way handshake has no Group Key (RSN)
            MSMSEC_M3_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 8)
            ' Matching security capabilities of IE in M3 failed (RSN/WPA)
            MSMSEC_PR_IE_MATCHING = (MSMSEC_CONNECT_BASE + 9)
            ' Matching security capabilities of Secondary IE in M3 failed (RSN)
            MSMSEC_SEC_IE_MATCHING = (MSMSEC_CONNECT_BASE + 10)
            ' Required a pairwise key but AP configured only group keys
            MSMSEC_NO_PAIRWISE_KEY = (MSMSEC_CONNECT_BASE + 11)
            ' Message 1 of group key handshake has no key data (RSN/WPA)
            MSMSEC_G1_MISSING_KEY_DATA = (MSMSEC_CONNECT_BASE + 12)
            ' Message 1 of group key handshake has no group key
            MSMSEC_G1_MISSING_GRP_KEY = (MSMSEC_CONNECT_BASE + 13)
            ' AP reset secure bit after connection was secured
            MSMSEC_PEER_INDICATED_INSECURE = (MSMSEC_CONNECT_BASE + 14)
            ' 802.1x indicated there is no authenticator but profile requires 802.1x
            MSMSEC_NO_AUTHENTICATOR = (MSMSEC_CONNECT_BASE + 15)
            ' Plumbing settings to NIC failed
            MSMSEC_NIC_FAILURE = (MSMSEC_CONNECT_BASE + 16)
            ' Operation was cancelled by caller
            MSMSEC_CANCELLED = (MSMSEC_CONNECT_BASE + 17)
            ' Key was in incorrect format 
            MSMSEC_KEY_FORMAT = (MSMSEC_CONNECT_BASE + 18)
            ' Security downgrade detected
            MSMSEC_DOWNGRADE_DETECTED = (MSMSEC_CONNECT_BASE + 19)
            ' PSK mismatch suspected
            MSMSEC_PSK_MISMATCH_SUSPECTED = (MSMSEC_CONNECT_BASE + 20)
            ' Forced failure because connection method was not secure
            MSMSEC_FORCED_FAILURE = (MSMSEC_CONNECT_BASE + 21)
            ' ui request couldn't be queued or user pressed cancel
            MSMSEC_SECURITY_UI_FAILURE = (MSMSEC_CONNECT_BASE + 22)

            MSMSEC_MAX = MSMSEC_END
        End Enum

        ''' <summary>
        ''' Contains information about connection related notifications.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_CONNECTION_NOTIFICATION_DATA</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure WlanConnectionNotificationData
            ''' <remarks>
            ''' On Windows XP SP 2, only <see cref="WlanConnectionMode.Profile"/> is supported.
            ''' </remarks>
            Public wlanConnectionMode As WlanConnectionMode
            ''' <summary>
            ''' The name of the profile used for the connection. Profile names are case-sensitive.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
            Public profileName As String
            ''' <summary>
            ''' The SSID of the association.
            ''' </summary>
            Public dot11Ssid As Dot11Ssid
            ''' <summary>
            ''' The BSS network type.
            ''' </summary>
            Public dot11BssType As Dot11BssType
            ''' <summary>
            ''' Indicates whether security is enabled for this connection.
            ''' </summary>
            Public securityEnabled As Boolean
            ''' <summary>
            ''' Indicates the reason for an operation failure.
            ''' This field has a value of <see cref="WlanReasonCode.Success"/> for all connection-related notifications except <see cref="WlanNotificationCodeAcm.ConnectionComplete"/>.
            ''' If the connection fails, this field indicates the reason for the failure.
            ''' </summary>
            Public wlanReasonCode As WlanReasonCode
            ''' <summary>
            ''' This field contains the XML presentation of the profile used for discovery, if the connection succeeds.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=1)> _
            Public profileXml As String
        End Structure

        ''' <summary>
        ''' Indicates the state of an interface.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_INTERFACE_STATE</c> type.
        ''' </remarks>
        Public Enum WlanInterfaceState
            ''' <summary>
            ''' The interface is not ready to operate.
            ''' </summary>
            NotReady = 0
            ''' <summary>
            ''' The interface is connected to a network.
            ''' </summary>
            Connected = 1
            ''' <summary>
            ''' The interface is the first node in an ad hoc network. No peer has connected.
            ''' </summary>
            AdHocNetworkFormed = 2
            ''' <summary>
            ''' The interface is disconnecting from the current network.
            ''' </summary>
            Disconnecting = 3
            ''' <summary>
            ''' The interface is not connected to any network.
            ''' </summary>
            Disconnected = 4
            ''' <summary>
            ''' The interface is attempting to associate with a network.
            ''' </summary>
            Associating = 5
            ''' <summary>
            ''' Auto configuration is discovering the settings for the network.
            ''' </summary>
            Discovering = 6
            ''' <summary>
            ''' The interface is in the process of authenticating.
            ''' </summary>
            Authenticating = 7
        End Enum

        ''' <summary>
        ''' Contains the SSID of an interface.
        ''' </summary>
        Public Structure Dot11Ssid
            ''' <summary>
            ''' The length, in bytes, of the <see cref="SSID"/> array.
            ''' </summary>
            Public SSIDLength As UInteger
            ''' <summary>
            ''' The SSID.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=32)>
            Public SSID As Byte()
        End Structure

        ''' <summary>
        ''' Defines an 802.11 PHY and media type.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>DOT11_PHY_TYPE</c> type.
        ''' </remarks>
        Public Enum Dot11PhyType As UInteger
            ''' <summary>
            ''' Specifies an unknown or uninitialized PHY type.
            ''' </summary>
            Unknown = 0
            ''' <summary>
            ''' Specifies any PHY type.
            ''' </summary>
            Any = Unknown
            ''' <summary>
            ''' Specifies a frequency-hopping spread-spectrum (FHSS) PHY. Bluetooth devices can use FHSS or an adaptation of FHSS.
            ''' </summary>
            FHSS = 1
            ''' <summary>
            ''' Specifies a direct sequence spread spectrum (DSSS) PHY.
            ''' </summary>
            DSSS = 2
            ''' <summary>
            ''' Specifies an infrared (IR) baseband PHY.
            ''' </summary>
            IrBaseband = 3
            ''' <summary>
            ''' Specifies an orthogonal frequency division multiplexing (OFDM) PHY. 802.11a devices can use OFDM.
            ''' </summary>
            OFDM = 4
            ''' <summary>
            ''' Specifies a high-rate DSSS (HRDSSS) PHY.
            ''' </summary>
            HRDSSS = 5
            ''' <summary>
            ''' Specifies an extended rate PHY (ERP). 802.11g devices can use ERP.
            ''' </summary>
            ERP = 6
            ''' <summary>
            ''' Specifies the start of the range that is used to define PHY types that are developed by an independent hardware vendor (IHV).
            ''' </summary>
            IHV_Start = &H80000000UI
            ''' <summary>
            ''' Specifies the end of the range that is used to define PHY types that are developed by an independent hardware vendor (IHV).
            ''' </summary>
            IHV_End = &HFFFFFFFFUI
        End Enum

        ''' <summary>
        ''' Defines a basic service set (BSS) network type.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>DOT11_BSS_TYPE</c> type.
        ''' </remarks>
        Public Enum Dot11BssType
            ''' <summary>
            ''' Specifies an infrastructure BSS network.
            ''' </summary>
            Infrastructure = 1
            ''' <summary>
            ''' Specifies an independent BSS (IBSS) network.
            ''' </summary>
            Independent = 2
            ''' <summary>
            ''' Specifies either infrastructure or IBSS network.
            ''' </summary>
            Any = 3
        End Enum

        ''' <summary>
        ''' Contains association attributes for a connection
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_ASSOCIATION_ATTRIBUTES</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanAssociationAttributes
            ''' <summary>
            ''' The SSID of the association.
            ''' </summary>
            Public dot11Ssid As Dot11Ssid
            ''' <summary>
            ''' Specifies whether the network is infrastructure or ad hoc.
            ''' </summary>
            Public dot11BssType As Dot11BssType
            ''' <summary>
            ''' The BSSID of the association.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)> _
            Public m_dot11Bssid As Byte()
            ''' <summary>
            ''' The physical type of the association.
            ''' </summary>
            Public dot11PhyType As Dot11PhyType
            ''' <summary>
            ''' The position of the <see cref="Dot11PhyType"/> value in the structure containing the list of PHY types.
            ''' </summary>
            Public dot11PhyIndex As UInteger
            ''' <summary>
            ''' A percentage value that represents the signal quality of the network.
            ''' This field contains a value between 0 and 100.
            ''' A value of 0 implies an actual RSSI signal strength of -100 dbm.
            ''' A value of 100 implies an actual RSSI signal strength of -50 dbm.
            ''' You can calculate the RSSI signal strength value for values between 1 and 99 using linear interpolation.
            ''' </summary>
            Public wlanSignalQuality As UInteger
            ''' <summary>
            ''' The receiving rate of the association.
            ''' </summary>
            Public rxRate As UInteger
            ''' <summary>
            ''' The transmission rate of the association.
            ''' </summary>
            Public txRate As UInteger

            ''' <summary>
            ''' Gets the BSSID of the associated access point.
            ''' </summary>
            ''' <value>The BSSID.</value>
            Public ReadOnly Property Dot11Bssid() As PhysicalAddress
                Get
                    Return New PhysicalAddress(m_dot11Bssid)
                End Get
            End Property
        End Structure

        ''' <summary>
        ''' Defines the mode of connection.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_CONNECTION_MODE</c> type.
        ''' </remarks>
        Public Enum WlanConnectionMode
            ''' <summary>
            ''' A profile will be used to make the connection.
            ''' </summary>
            Profile = 0
            ''' <summary>
            ''' A temporary profile will be used to make the connection.
            ''' </summary>
            TemporaryProfile
            ''' <summary>
            ''' Secure discovery will be used to make the connection.
            ''' </summary>
            DiscoverySecure
            ''' <summary>
            ''' Unsecure discovery will be used to make the connection.
            ''' </summary>
            DiscoveryUnsecure
            ''' <summary>
            ''' A connection will be made automatically, generally using a persistent profile.
            ''' </summary>
            Auto
            ''' <summary>
            ''' Not used.
            ''' </summary>
            Invalid
        End Enum

        ''' <summary>
        ''' Defines a wireless LAN authentication algorithm.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>DOT11_AUTH_ALGORITHM</c> type.
        ''' </remarks>
        Public Enum Dot11AuthAlgorithm As UInteger
            ''' <summary>
            ''' Specifies an IEEE 802.11 Open System authentication algorithm.
            ''' </summary>
            IEEE80211_Open = 1
            ''' <summary>
            ''' Specifies an 802.11 Shared Key authentication algorithm that requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication.
            ''' </summary>
            IEEE80211_SharedKey = 2
            ''' <summary>
            ''' Specifies a Wi-Fi Protected Access (WPA) algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
            ''' <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
            ''' <para>When the WPA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the WPA information element (IE).</para>
            ''' </summary>
            WPA = 3
            ''' <summary>
            ''' Specifies a WPA algorithm that uses preshared keys (PSK). IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
            ''' <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
            ''' <para>When the WPA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2 (preshared key) within the WPA IE.</para>
            ''' </summary>
            WPA_PSK = 4
            ''' <summary>
            ''' This value is not supported.
            ''' </summary>
            WPA_None = 5
            ''' <summary>
            ''' Specifies an 802.11i Robust Security Network Association (RSNA) algorithm. WPA2 is one such algorithm. IEEE 802.1X port authentication is performed by the supplicant, authenticator, and authentication server. Cipher keys are dynamically derived through the authentication process.
            ''' <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
            ''' <para>When the RSNA algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the RSN IE.</para>
            ''' </summary>
            RSNA = 6
            ''' <summary>
            ''' Specifies an 802.11i RSNA algorithm that uses PSK. IEEE 802.1X port authentication is performed by the supplicant and authenticator. Cipher keys are dynamically derived through a preshared key that is used on both the supplicant and authenticator.
            ''' <para>This algorithm is valid only for BSS types of <see cref="Dot11BssType.Infrastructure"/>.</para>
            ''' <para>When the RSNA PSK algorithm is enabled, the 802.11 station will associate only with an access point whose beacon or probe responses contain the authentication suite of type 2(preshared key) within the RSN IE.</para>
            ''' </summary>
            RSNA_PSK = 7
            ''' <summary>
            ''' Indicates the start of the range that specifies proprietary authentication algorithms that are developed by an IHV.
            ''' </summary>
            ''' <remarks>
            ''' This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
            ''' </remarks>
            IHV_Start = &H80000000UI
            ''' <summary>
            ''' Indicates the end of the range that specifies proprietary authentication algorithms that are developed by an IHV.
            ''' </summary>
            ''' <remarks>
            ''' This enumerator is valid only when the miniport driver is operating in Extensible Station (ExtSTA) mode.
            ''' </remarks>
            IHV_End = &HFFFFFFFFUI
        End Enum

        ''' <summary>
        ''' Defines a cipher algorithm for data encryption and decryption.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>DOT11_CIPHER_ALGORITHM</c> type.
        ''' </remarks>
        Public Enum Dot11CipherAlgorithm As UInteger
            ''' <summary>
            ''' Specifies that no cipher algorithm is enabled or supported.
            ''' </summary>
            None = &H0
            ''' <summary>
            ''' Specifies a Wired Equivalent Privacy (WEP) algorithm, which is the RC4-based algorithm that is specified in the 802.11-1999 standard. This enumerator specifies the WEP cipher algorithm with a 40-bit cipher key.
            ''' </summary>
            WEP40 = &H1
            ''' <summary>
            ''' Specifies a Temporal Key Integrity Protocol (TKIP) algorithm, which is the RC4-based cipher suite that is based on the algorithms that are defined in the WPA specification and IEEE 802.11i-2004 standard. This cipher also uses the Michael Message Integrity Code (MIC) algorithm for forgery protection.
            ''' </summary>
            TKIP = &H2
            ''' <summary>
            ''' Specifies an AES-CCMP algorithm, as specified in the IEEE 802.11i-2004 standard and RFC 3610. Advanced Encryption Standard (AES) is the encryption algorithm defined in FIPS PUB 197.
            ''' </summary>
            CCMP = &H4
            ''' <summary>
            ''' Specifies a WEP cipher algorithm with a 104-bit cipher key.
            ''' </summary>
            WEP104 = &H5
            ''' <summary>
            ''' Specifies a Robust Security Network (RSN) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
            ''' </summary>
            WPA_UseGroup = &H100
            ''' <summary>
            ''' Specifies a Wifi Protected Access (WPA) Use Group Key cipher suite. For more information about the Use Group Key cipher suite, refer to Clause 7.3.2.9.1 of the IEEE 802.11i-2004 standard.
            ''' </summary>
            RSN_UseGroup = &H100
            ''' <summary>
            ''' Specifies a WEP cipher algorithm with a cipher key of any length.
            ''' </summary>
            WEP = &H101
            ''' <summary>
            ''' Specifies the start of the range that is used to define proprietary cipher algorithms that are developed by an independent hardware vendor (IHV).
            ''' </summary>
            IHV_Start = &H80000000UI
            ''' <summary>
            ''' Specifies the end of the range that is used to define proprietary cipher algorithms that are developed by an IHV.
            ''' </summary>
            IHV_End = &HFFFFFFFFUI
        End Enum

        ''' <summary>
        ''' Defines the security attributes for a wireless connection.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_SECURITY_ATTRIBUTES</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure WlanSecurityAttributes
            ''' <summary>
            ''' Indicates whether security is enabled for this connection.
            ''' </summary>
            <MarshalAs(UnmanagedType.Bool)> _
            Public securityEnabled As Boolean
            <MarshalAs(UnmanagedType.Bool)> _
            Public oneXEnabled As Boolean
            ''' <summary>
            ''' The authentication algorithm.
            ''' </summary>
            Public dot11AuthAlgorithm As Dot11AuthAlgorithm
            ''' <summary>
            ''' The cipher algorithm.
            ''' </summary>
            Public dot11CipherAlgorithm As Dot11CipherAlgorithm
        End Structure

        ''' <summary>
        ''' Defines the attributes of a wireless connection.
        ''' </summary>
        ''' <remarks>
        ''' Corresponds to the native <c>WLAN_CONNECTION_ATTRIBUTES</c> type.
        ''' </remarks>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure WlanConnectionAttributes
            ''' <summary>
            ''' The state of the interface.
            ''' </summary>
            Public isState As WlanInterfaceState
            ''' <summary>
            ''' The mode of the connection.
            ''' </summary>
            Public wlanConnectionMode As WlanConnectionMode
            ''' <summary>
            ''' The name of the profile used for the connection. Profile names are case-sensitive.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)> _
            Public profileName As String
            ''' <summary>
            ''' The attributes of the association.
            ''' </summary>
            Public wlanAssociationAttributes As WlanAssociationAttributes
            ''' <summary>
            ''' The security attributes of the connection.
            ''' </summary>
            Public wlanSecurityAttributes As WlanSecurityAttributes
        End Structure

        ''' <summary>
        ''' Contains information about a LAN interface.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure WlanInterfaceInfo
            ''' <summary>
            ''' The GUID of the interface.
            ''' </summary>
            Public interfaceGuid As Guid
            ''' <summary>
            ''' The description of the interface.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)> _
            Public interfaceDescription As String
            ''' <summary>
            ''' The current state of the interface.
            ''' </summary>
            Public isState As WlanInterfaceState
        End Structure

        ''' <summary>
        ''' The header of the list returned by <see cref="WlanEnumInterfaces"/>.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure WlanInterfaceInfoListHeader
            Public numberOfItems As UInteger
            Public index As UInteger
        End Structure

        ''' <summary>
        ''' The header of the list returned by <see cref="WlanGetProfileList"/>.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure WlanProfileInfoListHeader
            Public numberOfItems As UInteger
            Public index As UInteger
        End Structure

        ''' <summary>
        ''' Contains basic information about a profile.
        ''' </summary>
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure WlanProfileInfo
            ''' <summary>
            ''' The name of the profile. This value may be the name of a domain if the profile is for provisioning. Profile names are case-sensitive.
            ''' </summary>
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)> _
            Public profileName As String
            ''' <summary>
            ''' Profile flags.
            ''' </summary>
            Public profileFlags As WlanProfileFlags
        End Structure

        ''' <summary>
        ''' Flags that specifiy the miniport driver's current operation mode.
        ''' </summary>
        <Flags()> _
        Public Enum Dot11OperationMode As UInteger
            Unknown = &H0
            Station = &H1
            AP = &H2
            ''' <summary>
            ''' Specifies that the miniport driver supports the Extensible Station (ExtSTA) operation mode.
            ''' </summary>
            ExtensibleStation = &H4
            ''' <summary>
            ''' Specifies that the miniport driver supports the Network Monitor (NetMon) operation mode.
            ''' </summary>
            NetworkMonitor = &H80000000UI
        End Enum
#End Region

        ''' <summary>
        ''' Helper method to wrap calls to Native WiFi API methods.
        ''' If the method falls, throws an exception containing the error code.
        ''' </summary>
        ''' <param name="win32ErrorCode">The error code.</param>
        <DebuggerStepThrough()> _
        Friend Shared Sub ThrowIfError(ByVal win32ErrorCode As Integer)
            If win32ErrorCode <> 0 Then
                Throw New Win32Exception(win32ErrorCode)
            End If
        End Sub
    End Class
End Namespace