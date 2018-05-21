<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lstvAvailableNetworkList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnGetWLANList = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.GroupListView = New System.Windows.Forms.ListView()
        Me.cbListIndex = New System.Windows.Forms.ComboBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStuID = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtLocation = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbxLocationName = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rdoClassroom = New System.Windows.Forms.RadioButton()
        Me.rdoAisle = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbxFloor = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbxID = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbxNo_extpant = New System.Windows.Forms.ComboBox()
        Me.cbxClassroomNo = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnLocation = New System.Windows.Forms.Button()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstvAvailableNetworkList
        '
        Me.lstvAvailableNetworkList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader12, Me.ColumnHeader13})
        Me.lstvAvailableNetworkList.Location = New System.Drawing.Point(12, 12)
        Me.lstvAvailableNetworkList.Name = "lstvAvailableNetworkList"
        Me.lstvAvailableNetworkList.Size = New System.Drawing.Size(943, 292)
        Me.lstvAvailableNetworkList.TabIndex = 0
        Me.lstvAvailableNetworkList.UseCompatibleStateImageBehavior = False
        Me.lstvAvailableNetworkList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "SSID"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "SignalQuality"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "dot11BssType"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "dot11DefaultAuthAlgorithm"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "dot11DefaultCipherAlgorithm"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "morePhyTypes"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "numberOfBssids"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "networkConnectable"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "flags"
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "profileName"
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "securityEnabled"
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "wlanNotConnectableReason"
        '
        'btnGetWLANList
        '
        Me.btnGetWLANList.Enabled = False
        Me.btnGetWLANList.Location = New System.Drawing.Point(20, 343)
        Me.btnGetWLANList.Name = "btnGetWLANList"
        Me.btnGetWLANList.Size = New System.Drawing.Size(62, 42)
        Me.btnGetWLANList.TabIndex = 1
        Me.btnGetWLANList.Text = "Get WLAN List"
        Me.btnGetWLANList.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Enabled = False
        Me.Button1.Location = New System.Drawing.Point(20, 410)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(62, 42)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "GetNetworkBSSList"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Button2
        '
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button2.Location = New System.Drawing.Point(843, 360)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "開始搜索"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Timer2
        '
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.SystemColors.Control
        Me.ProgressBar1.Location = New System.Drawing.Point(355, 430)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(623, 23)
        Me.ProgressBar1.TabIndex = 4
        '
        'GroupListView
        '
        Me.GroupListView.Location = New System.Drawing.Point(959, 12)
        Me.GroupListView.Name = "GroupListView"
        Me.GroupListView.Size = New System.Drawing.Size(264, 292)
        Me.GroupListView.TabIndex = 5
        Me.GroupListView.UseCompatibleStateImageBehavior = False
        Me.GroupListView.View = System.Windows.Forms.View.Details
        '
        'cbListIndex
        '
        Me.cbListIndex.FormattingEnabled = True
        Me.cbListIndex.Location = New System.Drawing.Point(1088, 343)
        Me.cbListIndex.Name = "cbListIndex"
        Me.cbListIndex.Size = New System.Drawing.Size(90, 20)
        Me.cbListIndex.TabIndex = 6
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(616, 470)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(103, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "儲存本次資料"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(99, 355)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 12)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "學號："
        '
        'txtStuID
        '
        Me.txtStuID.Location = New System.Drawing.Point(171, 353)
        Me.txtStuID.Name = "txtStuID"
        Me.txtStuID.Size = New System.Drawing.Size(98, 22)
        Me.txtStuID.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(99, 436)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 12)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "檢測地點："
        '
        'txtLocation
        '
        Me.txtLocation.Enabled = False
        Me.txtLocation.Location = New System.Drawing.Point(171, 436)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.Size = New System.Drawing.Size(98, 22)
        Me.txtLocation.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(993, 348)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 12)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "歷次資料列表："
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Font = New System.Drawing.Font("新細明體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.NumericUpDown1.Location = New System.Drawing.Point(182, 390)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.NumericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(65, 22)
        Me.NumericUpDown1.TabIndex = 13
        Me.NumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.NumericUpDown1.Value = New Decimal(New Integer() {180, 0, 0, 0})
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(99, 394)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 12)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "搜尋秒數(s)："
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(603, 349)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 12)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "檢測地點："
        '
        'cbxLocationName
        '
        Me.cbxLocationName.FormattingEnabled = True
        Me.cbxLocationName.Items.AddRange(New Object() {"恩", "成", "義", "典", "仁", "賢", "孝", "功", "忠", "德", "雅", "菲", "曉峯"})
        Me.cbxLocationName.Location = New System.Drawing.Point(671, 347)
        Me.cbxLocationName.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxLocationName.Name = "cbxLocationName"
        Me.cbxLocationName.Size = New System.Drawing.Size(44, 20)
        Me.cbxLocationName.TabIndex = 16
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(-21, -45)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Size = New System.Drawing.Size(109, 57)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'rdoClassroom
        '
        Me.rdoClassroom.AutoSize = True
        Me.rdoClassroom.Checked = True
        Me.rdoClassroom.Location = New System.Drawing.Point(11, 23)
        Me.rdoClassroom.Margin = New System.Windows.Forms.Padding(2)
        Me.rdoClassroom.Name = "rdoClassroom"
        Me.rdoClassroom.Size = New System.Drawing.Size(47, 16)
        Me.rdoClassroom.TabIndex = 18
        Me.rdoClassroom.TabStop = True
        Me.rdoClassroom.Text = "教室"
        Me.rdoClassroom.UseVisualStyleBackColor = True
        '
        'rdoAisle
        '
        Me.rdoAisle.AutoSize = True
        Me.rdoAisle.Location = New System.Drawing.Point(66, 23)
        Me.rdoAisle.Margin = New System.Windows.Forms.Padding(2)
        Me.rdoAisle.Name = "rdoAisle"
        Me.rdoAisle.Size = New System.Drawing.Size(47, 16)
        Me.rdoAisle.TabIndex = 19
        Me.rdoAisle.Text = "走廊"
        Me.rdoAisle.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rdoClassroom)
        Me.GroupBox2.Controls.Add(Me.rdoAisle)
        Me.GroupBox2.Location = New System.Drawing.Point(298, 343)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox2.Size = New System.Drawing.Size(121, 50)
        Me.GroupBox2.TabIndex = 20
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "地點別"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(495, 348)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(41, 12)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "樓層："
        '
        'cbxFloor
        '
        Me.cbxFloor.FormattingEnabled = True
        Me.cbxFloor.Items.AddRange(New Object() {"B2", "B1", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"})
        Me.cbxFloor.Location = New System.Drawing.Point(539, 346)
        Me.cbxFloor.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxFloor.Name = "cbxFloor"
        Me.cbxFloor.Size = New System.Drawing.Size(44, 20)
        Me.cbxFloor.TabIndex = 22
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(435, 382)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 12)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "教室號/走廊序號："
        '
        'cbxID
        '
        Me.cbxID.FormattingEnabled = True
        Me.cbxID.Location = New System.Drawing.Point(539, 380)
        Me.cbxID.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxID.Name = "cbxID"
        Me.cbxID.Size = New System.Drawing.Size(49, 20)
        Me.cbxID.TabIndex = 24
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(595, 382)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(9, 12)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "-"
        '
        'cbxNo_extpant
        '
        Me.cbxNo_extpant.FormattingEnabled = True
        Me.cbxNo_extpant.Location = New System.Drawing.Point(609, 380)
        Me.cbxNo_extpant.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxNo_extpant.Name = "cbxNo_extpant"
        Me.cbxNo_extpant.Size = New System.Drawing.Size(39, 20)
        Me.cbxNo_extpant.TabIndex = 26
        '
        'cbxClassroomNo
        '
        Me.cbxClassroomNo.FormattingEnabled = True
        Me.cbxClassroomNo.Location = New System.Drawing.Point(671, 380)
        Me.cbxClassroomNo.Margin = New System.Windows.Forms.Padding(2)
        Me.cbxClassroomNo.Name = "cbxClassroomNo"
        Me.cbxClassroomNo.Size = New System.Drawing.Size(39, 20)
        Me.cbxClassroomNo.TabIndex = 28
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(657, 382)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(9, 12)
        Me.Label9.TabIndex = 27
        Me.Label9.Text = "-"
        '
        'btnLocation
        '
        Me.btnLocation.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnLocation.Location = New System.Drawing.Point(741, 359)
        Me.btnLocation.Name = "btnLocation"
        Me.btnLocation.Size = New System.Drawing.Size(75, 23)
        Me.btnLocation.TabIndex = 29
        Me.btnLocation.Text = "確認地點"
        Me.btnLocation.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(788, 503)
        Me.Controls.Add(Me.btnLocation)
        Me.Controls.Add(Me.cbxClassroomNo)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.cbxNo_extpant)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cbxID)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cbxFloor)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cbxLocationName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtLocation)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtStuID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.cbListIndex)
        Me.Controls.Add(Me.GroupListView)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnGetWLANList)
        Me.Controls.Add(Me.lstvAvailableNetworkList)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WiFi"
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstvAvailableNetworkList As ListView
    Friend WithEvents btnGetWLANList As Button
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents ColumnHeader7 As ColumnHeader
    Friend WithEvents ColumnHeader8 As ColumnHeader
    Friend WithEvents ColumnHeader9 As ColumnHeader
    Friend WithEvents ColumnHeader10 As ColumnHeader
    Friend WithEvents ColumnHeader12 As ColumnHeader
    Friend WithEvents ColumnHeader13 As ColumnHeader
    Friend WithEvents Button1 As Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents GroupListView As ListView
    Friend WithEvents cbListIndex As ComboBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtStuID As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtLocation As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cbxLocationName As ComboBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents rdoClassroom As RadioButton
    Friend WithEvents rdoAisle As RadioButton
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cbxFloor As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cbxID As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cbxNo_extpant As ComboBox
    Friend WithEvents cbxClassroomNo As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents btnLocation As Button
End Class
