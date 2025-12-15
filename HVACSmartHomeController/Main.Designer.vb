<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.ReadTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.CurrentTempTextBox = New System.Windows.Forms.TextBox()
        Me.SerialTextBox = New System.Windows.Forms.TextBox()
        Me.SetTempTextBox = New System.Windows.Forms.TextBox()
        Me.PortsComboBox = New System.Windows.Forms.ComboBox()
        Me.ConnectionStatusLabel = New System.Windows.Forms.Label()
        Me.SerialTextBoxLabel = New System.Windows.Forms.Label()
        Me.COMPortLabel = New System.Windows.Forms.Label()
        Me.TargetTempLabel = New System.Windows.Forms.Label()
        Me.CurrentTempTextBoxLabel = New System.Windows.Forms.Label()
        Me.DigitalInputsTextBox = New System.Windows.Forms.TextBox()
        Me.DigitalInputTextBoxLabel = New System.Windows.Forms.Label()
        Me.CurrentTimeTextBox = New System.Windows.Forms.TextBox()
        Me.CurrentTimeLabel = New System.Windows.Forms.Label()
        Me.OperationTextBoxLabel = New System.Windows.Forms.Label()
        Me.OperationTextBox = New System.Windows.Forms.TextBox()
        Me.FanTextBox = New System.Windows.Forms.TextBox()
        Me.FanLabel = New System.Windows.Forms.Label()
        Me.HardwareTextBox = New System.Windows.Forms.TextBox()
        Me.HadwareTempTextBoxLabel = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ExitToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.LowTempTextBox = New System.Windows.Forms.TextBox()
        Me.TargetTempLowLabel = New System.Windows.Forms.Label()
        Me.IncrementHighTempButton = New System.Windows.Forms.Button()
        Me.DecrementLowTempButton = New System.Windows.Forms.Button()
        Me.IncrementLowTempButton = New System.Windows.Forms.Button()
        Me.DecrementHighTempButton = New System.Windows.Forms.Button()
        Me.SafetyTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SerialTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ClockTextBox = New System.Windows.Forms.TextBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        '
        'ReadTimer
        '
        '
        'ExitButton
        '
        Me.ExitButton.Location = New System.Drawing.Point(303, 373)
        Me.ExitButton.Margin = New System.Windows.Forms.Padding(2)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(140, 57)
        Me.ExitButton.TabIndex = 4
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'CurrentTempTextBox
        '
        Me.CurrentTempTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!)
        Me.CurrentTempTextBox.Location = New System.Drawing.Point(469, 124)
        Me.CurrentTempTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.CurrentTempTextBox.Name = "CurrentTempTextBox"
        Me.CurrentTempTextBox.Size = New System.Drawing.Size(116, 45)
        Me.CurrentTempTextBox.TabIndex = 5
        '
        'SerialTextBox
        '
        Me.SerialTextBox.Location = New System.Drawing.Point(399, 232)
        Me.SerialTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.SerialTextBox.Multiline = True
        Me.SerialTextBox.Name = "SerialTextBox"
        Me.SerialTextBox.Size = New System.Drawing.Size(252, 61)
        Me.SerialTextBox.TabIndex = 6
        '
        'SetTempTextBox
        '
        Me.SetTempTextBox.Location = New System.Drawing.Point(365, 133)
        Me.SetTempTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.SetTempTextBox.Name = "SetTempTextBox"
        Me.SetTempTextBox.Size = New System.Drawing.Size(68, 20)
        Me.SetTempTextBox.TabIndex = 7
        '
        'PortsComboBox
        '
        Me.PortsComboBox.FormattingEnabled = True
        Me.PortsComboBox.Location = New System.Drawing.Point(447, 409)
        Me.PortsComboBox.Margin = New System.Windows.Forms.Padding(2)
        Me.PortsComboBox.Name = "PortsComboBox"
        Me.PortsComboBox.Size = New System.Drawing.Size(82, 21)
        Me.PortsComboBox.TabIndex = 8
        '
        'ConnectionStatusLabel
        '
        Me.ConnectionStatusLabel.AutoSize = True
        Me.ConnectionStatusLabel.Location = New System.Drawing.Point(21, 6)
        Me.ConnectionStatusLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.ConnectionStatusLabel.Name = "ConnectionStatusLabel"
        Me.ConnectionStatusLabel.Size = New System.Drawing.Size(48, 13)
        Me.ConnectionStatusLabel.TabIndex = 9
        Me.ConnectionStatusLabel.Text = "Filler text"
        '
        'SerialTextBoxLabel
        '
        Me.SerialTextBoxLabel.AutoSize = True
        Me.SerialTextBoxLabel.Location = New System.Drawing.Point(503, 295)
        Me.SerialTextBoxLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.SerialTextBoxLabel.Name = "SerialTextBoxLabel"
        Me.SerialTextBoxLabel.Size = New System.Drawing.Size(59, 13)
        Me.SerialTextBoxLabel.TabIndex = 10
        Me.SerialTextBoxLabel.Text = "Serial Data"
        '
        'COMPortLabel
        '
        Me.COMPortLabel.AutoSize = True
        Me.COMPortLabel.Location = New System.Drawing.Point(445, 395)
        Me.COMPortLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.COMPortLabel.Name = "COMPortLabel"
        Me.COMPortLabel.Size = New System.Drawing.Size(58, 13)
        Me.COMPortLabel.TabIndex = 11
        Me.COMPortLabel.Text = "COM Ports"
        '
        'TargetTempLabel
        '
        Me.TargetTempLabel.AutoSize = True
        Me.TargetTempLabel.Location = New System.Drawing.Point(334, 118)
        Me.TargetTempLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.TargetTempLabel.Name = "TargetTempLabel"
        Me.TargetTempLabel.Size = New System.Drawing.Size(126, 13)
        Me.TargetTempLabel.TabIndex = 12
        Me.TargetTempLabel.Text = "Target Temperature High"
        '
        'CurrentTempTextBoxLabel
        '
        Me.CurrentTempTextBoxLabel.AutoSize = True
        Me.CurrentTempTextBoxLabel.Location = New System.Drawing.Point(477, 109)
        Me.CurrentTempTextBoxLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.CurrentTempTextBoxLabel.Name = "CurrentTempTextBoxLabel"
        Me.CurrentTempTextBoxLabel.Size = New System.Drawing.Size(101, 13)
        Me.CurrentTempTextBoxLabel.TabIndex = 13
        Me.CurrentTempTextBoxLabel.Text = "CurrentTemperature"
        '
        'DigitalInputsTextBox
        '
        Me.DigitalInputsTextBox.Location = New System.Drawing.Point(655, 232)
        Me.DigitalInputsTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.DigitalInputsTextBox.Multiline = True
        Me.DigitalInputsTextBox.Name = "DigitalInputsTextBox"
        Me.DigitalInputsTextBox.Size = New System.Drawing.Size(17, 111)
        Me.DigitalInputsTextBox.TabIndex = 14
        '
        'DigitalInputTextBoxLabel
        '
        Me.DigitalInputTextBoxLabel.AutoSize = True
        Me.DigitalInputTextBoxLabel.Location = New System.Drawing.Point(626, 345)
        Me.DigitalInputTextBoxLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.DigitalInputTextBoxLabel.Name = "DigitalInputTextBoxLabel"
        Me.DigitalInputTextBoxLabel.Size = New System.Drawing.Size(68, 13)
        Me.DigitalInputTextBoxLabel.TabIndex = 15
        Me.DigitalInputTextBoxLabel.Text = "Digital Inputs"
        '
        'CurrentTimeTextBox
        '
        Me.CurrentTimeTextBox.Location = New System.Drawing.Point(448, 371)
        Me.CurrentTimeTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.CurrentTimeTextBox.Name = "CurrentTimeTextBox"
        Me.CurrentTimeTextBox.Size = New System.Drawing.Size(68, 20)
        Me.CurrentTimeTextBox.TabIndex = 17
        '
        'CurrentTimeLabel
        '
        Me.CurrentTimeLabel.AutoSize = True
        Me.CurrentTimeLabel.Location = New System.Drawing.Point(445, 356)
        Me.CurrentTimeLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.CurrentTimeLabel.Name = "CurrentTimeLabel"
        Me.CurrentTimeLabel.Size = New System.Drawing.Size(30, 13)
        Me.CurrentTimeLabel.TabIndex = 18
        Me.CurrentTimeLabel.Text = "Date"
        '
        'OperationTextBoxLabel
        '
        Me.OperationTextBoxLabel.AutoSize = True
        Me.OperationTextBoxLabel.Location = New System.Drawing.Point(356, 171)
        Me.OperationTextBoxLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.OperationTextBoxLabel.Name = "OperationTextBoxLabel"
        Me.OperationTextBoxLabel.Size = New System.Drawing.Size(53, 13)
        Me.OperationTextBoxLabel.TabIndex = 19
        Me.OperationTextBoxLabel.Text = "Operation"
        '
        'OperationTextBox
        '
        Me.OperationTextBox.Location = New System.Drawing.Point(358, 186)
        Me.OperationTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.OperationTextBox.Name = "OperationTextBox"
        Me.OperationTextBox.Size = New System.Drawing.Size(102, 20)
        Me.OperationTextBox.TabIndex = 20
        '
        'FanTextBox
        '
        Me.FanTextBox.Location = New System.Drawing.Point(595, 186)
        Me.FanTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.FanTextBox.Name = "FanTextBox"
        Me.FanTextBox.Size = New System.Drawing.Size(135, 20)
        Me.FanTextBox.TabIndex = 22
        '
        'FanLabel
        '
        Me.FanLabel.AutoSize = True
        Me.FanLabel.Location = New System.Drawing.Point(593, 171)
        Me.FanLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.FanLabel.Name = "FanLabel"
        Me.FanLabel.Size = New System.Drawing.Size(58, 13)
        Me.FanLabel.TabIndex = 23
        Me.FanLabel.Text = "Fan Status"
        '
        'HardwareTextBox
        '
        Me.HardwareTextBox.Location = New System.Drawing.Point(494, 186)
        Me.HardwareTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.HardwareTextBox.Name = "HardwareTextBox"
        Me.HardwareTextBox.Size = New System.Drawing.Size(68, 20)
        Me.HardwareTextBox.TabIndex = 26
        '
        'HadwareTempTextBoxLabel
        '
        Me.HadwareTempTextBoxLabel.AutoSize = True
        Me.HadwareTempTextBoxLabel.Location = New System.Drawing.Point(480, 171)
        Me.HadwareTempTextBoxLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.HadwareTempTextBoxLabel.Name = "HadwareTempTextBoxLabel"
        Me.HadwareTempTextBoxLabel.Size = New System.Drawing.Size(98, 13)
        Me.HadwareTempTextBoxLabel.TabIndex = 27
        Me.HadwareTempTextBoxLabel.Text = "Board Temperature"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(734, 31)
        Me.ToolStrip1.TabIndex = 28
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ExitToolStripButton
        '
        Me.ExitToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ExitToolStripButton.Image = CType(resources.GetObject("ExitToolStripButton.Image"), System.Drawing.Image)
        Me.ExitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExitToolStripButton.Name = "ExitToolStripButton"
        Me.ExitToolStripButton.Size = New System.Drawing.Size(28, 28)
        Me.ExitToolStripButton.Text = "Exit"
        '
        'LowTempTextBox
        '
        Me.LowTempTextBox.Location = New System.Drawing.Point(629, 133)
        Me.LowTempTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.LowTempTextBox.Name = "LowTempTextBox"
        Me.LowTempTextBox.Size = New System.Drawing.Size(68, 20)
        Me.LowTempTextBox.TabIndex = 29
        '
        'TargetTempLowLabel
        '
        Me.TargetTempLowLabel.AutoSize = True
        Me.TargetTempLowLabel.Location = New System.Drawing.Point(598, 118)
        Me.TargetTempLowLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.TargetTempLowLabel.Name = "TargetTempLowLabel"
        Me.TargetTempLowLabel.Size = New System.Drawing.Size(124, 13)
        Me.TargetTempLowLabel.TabIndex = 30
        Me.TargetTempLowLabel.Text = "Target Temperature Low"
        '
        'IncrementHighTempButton
        '
        Me.IncrementHighTempButton.Location = New System.Drawing.Point(333, 133)
        Me.IncrementHighTempButton.Margin = New System.Windows.Forms.Padding(2)
        Me.IncrementHighTempButton.Name = "IncrementHighTempButton"
        Me.IncrementHighTempButton.Size = New System.Drawing.Size(28, 21)
        Me.IncrementHighTempButton.TabIndex = 31
        Me.IncrementHighTempButton.Text = "+"
        Me.IncrementHighTempButton.UseVisualStyleBackColor = True
        '
        'DecrementLowTempButton
        '
        Me.DecrementLowTempButton.Location = New System.Drawing.Point(701, 133)
        Me.DecrementLowTempButton.Margin = New System.Windows.Forms.Padding(2)
        Me.DecrementLowTempButton.Name = "DecrementLowTempButton"
        Me.DecrementLowTempButton.Size = New System.Drawing.Size(29, 21)
        Me.DecrementLowTempButton.TabIndex = 32
        Me.DecrementLowTempButton.Text = "-"
        Me.DecrementLowTempButton.UseVisualStyleBackColor = True
        '
        'IncrementLowTempButton
        '
        Me.IncrementLowTempButton.Location = New System.Drawing.Point(597, 133)
        Me.IncrementLowTempButton.Margin = New System.Windows.Forms.Padding(2)
        Me.IncrementLowTempButton.Name = "IncrementLowTempButton"
        Me.IncrementLowTempButton.Size = New System.Drawing.Size(28, 20)
        Me.IncrementLowTempButton.TabIndex = 33
        Me.IncrementLowTempButton.Text = "+"
        Me.IncrementLowTempButton.UseVisualStyleBackColor = True
        '
        'DecrementHighTempButton
        '
        Me.DecrementHighTempButton.Location = New System.Drawing.Point(437, 133)
        Me.DecrementHighTempButton.Margin = New System.Windows.Forms.Padding(2)
        Me.DecrementHighTempButton.Name = "DecrementHighTempButton"
        Me.DecrementHighTempButton.Size = New System.Drawing.Size(28, 20)
        Me.DecrementHighTempButton.TabIndex = 34
        Me.DecrementHighTempButton.Text = "-"
        Me.DecrementHighTempButton.UseVisualStyleBackColor = True
        '
        'SafetyTimer
        '
        Me.SafetyTimer.Interval = 30000
        '
        'SerialTimer
        '
        '
        'ClockTextBox
        '
        Me.ClockTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!)
        Me.ClockTextBox.Location = New System.Drawing.Point(427, 33)
        Me.ClockTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.ClockTextBox.Name = "ClockTextBox"
        Me.ClockTextBox.Size = New System.Drawing.Size(182, 53)
        Me.ClockTextBox.TabIndex = 36
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = Global.HVACSmartHomeController.My.Resources.Resources.ISU_logo_black
        Me.PictureBox2.Location = New System.Drawing.Point(-301, -35)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(600, 600)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 25
        Me.PictureBox2.TabStop = False
        '
        'HVAC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ClientSize = New System.Drawing.Size(734, 487)
        Me.Controls.Add(Me.ClockTextBox)
        Me.Controls.Add(Me.DecrementHighTempButton)
        Me.Controls.Add(Me.IncrementLowTempButton)
        Me.Controls.Add(Me.DecrementLowTempButton)
        Me.Controls.Add(Me.IncrementHighTempButton)
        Me.Controls.Add(Me.TargetTempLowLabel)
        Me.Controls.Add(Me.LowTempTextBox)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.HadwareTempTextBoxLabel)
        Me.Controls.Add(Me.HardwareTextBox)
        Me.Controls.Add(Me.FanLabel)
        Me.Controls.Add(Me.FanTextBox)
        Me.Controls.Add(Me.OperationTextBox)
        Me.Controls.Add(Me.OperationTextBoxLabel)
        Me.Controls.Add(Me.CurrentTimeLabel)
        Me.Controls.Add(Me.CurrentTimeTextBox)
        Me.Controls.Add(Me.DigitalInputTextBoxLabel)
        Me.Controls.Add(Me.DigitalInputsTextBox)
        Me.Controls.Add(Me.CurrentTempTextBoxLabel)
        Me.Controls.Add(Me.TargetTempLabel)
        Me.Controls.Add(Me.COMPortLabel)
        Me.Controls.Add(Me.SerialTextBoxLabel)
        Me.Controls.Add(Me.ConnectionStatusLabel)
        Me.Controls.Add(Me.PortsComboBox)
        Me.Controls.Add(Me.SetTempTextBox)
        Me.Controls.Add(Me.CurrentTempTextBox)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.SerialTextBox)
        Me.Controls.Add(Me.PictureBox2)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "Main"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents ReadTimer As Timer
    Friend WithEvents ExitButton As Button
    Friend WithEvents CurrentTempTextBox As TextBox
    Friend WithEvents SerialTextBox As TextBox
    Friend WithEvents SetTempTextBox As TextBox
    Friend WithEvents PortsComboBox As ComboBox
    Friend WithEvents ConnectionStatusLabel As Label
    Friend WithEvents SerialTextBoxLabel As Label
    Friend WithEvents COMPortLabel As Label
    Friend WithEvents TargetTempLabel As Label
    Friend WithEvents CurrentTempTextBoxLabel As Label
    Friend WithEvents DigitalInputsTextBox As TextBox
    Friend WithEvents DigitalInputTextBoxLabel As Label
    Friend WithEvents CurrentTimeTextBox As TextBox
    Friend WithEvents CurrentTimeLabel As Label
    Friend WithEvents OperationTextBoxLabel As Label
    Friend WithEvents OperationTextBox As TextBox
    Friend WithEvents FanTextBox As TextBox
    Friend WithEvents FanLabel As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents HardwareTextBox As TextBox
    Friend WithEvents HadwareTempTextBoxLabel As Label
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ExitToolStripButton As ToolStripButton
    Friend WithEvents Timer1 As Timer
    Friend WithEvents LowTempTextBox As TextBox
    Friend WithEvents TargetTempLowLabel As Label
    Friend WithEvents IncrementHighTempButton As Button
    Friend WithEvents DecrementLowTempButton As Button
    Friend WithEvents IncrementLowTempButton As Button
    Friend WithEvents DecrementHighTempButton As Button
    Friend WithEvents SafetyTimer As Timer
    Friend WithEvents SerialTimer As Timer
    Friend WithEvents ClockTextBox As TextBox
End Class
