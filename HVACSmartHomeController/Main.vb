'Alexis Villagran

Option Strict On
Option Explicit On

Imports System.IO.Ports

Public Class Main
#Region "Core Output States (What We Intend To Send To Hardware)"

    ' Actual commanded outputs (these represent what the system WANTS the hardware to do)
    Private fanOutput As Boolean = False
    Private heatOutput As Boolean = False
    Private coolOutput As Boolean = False

#End Region

#Region "Filter Monitoring (Digital Bit 4)"

    Private prevFilterDirtyBit As Boolean = False
    Private filterDirtyStartTime As DateTime = DateTime.MinValue

    Private fanRuntimeStart As DateTime = DateTime.MinValue
    Private cumulativeFanSeconds As Long = 0

    Private WithEvents FilterTimer As New System.Windows.Forms.Timer()

    Private Const FILTER_HOLD_SECONDS As Integer = 5
    Private Const FILTER_CHECK_MINUTES As Integer = 2

#End Region

#Region "Latching / Edge Detection State"

    ' Latched modes (persist until toggled off or forced off by interlock/fault)
    Private heatLatched As Boolean = False
    Private coolLatched As Boolean = False
    Private fanLatched As Boolean = False
    Private interlockLatched As Boolean = False

    ' Previous input states (for rising-edge detection)
    Private prevHeatBit As Boolean = False
    Private prevCoolBit As Boolean = False
    Private prevFanBit As Boolean = False
    Private prevInterlockBit As Boolean = False

    ' Most recently processed digital byte (already inverted for pull-up inputs)
    Private latestDigitalByte As Byte = 0

#End Region

#Region "Temperature Readings / Control Constants"

    ' Latest parsed temperatures
    Private currentemp As Single = 0.0F
    Private hardwaretemp As Single = 0.0F

    Private Const HYSTERESIS As Single = 0.5F
    Private lastSetpoint As Single = 71.0F

#End Region

#Region "Settings / Logging"

    Private Const SETTINGS_FILE As String = "HVAC Settings.txt"
    Private Const ERROR_LOG_FILE As String = "errorLog.txt"

    Private Function GetSettingsPath() As String
        Return IO.Path.Combine(Application.StartupPath, SETTINGS_FILE)
    End Function

    Private Function GetErrorLogPath() As String
        Return IO.Path.Combine(Application.StartupPath, ERROR_LOG_FILE)
    End Function

#End Region

#Region "Quiet Board Detection / Auto-Connect"

    Private WithEvents DetectTimer As New System.Windows.Forms.Timer()
    Private previouslyAvailablePorts As New List(Of String)
    Private hardwareVerified As Boolean = False

    ' Expected handshake response signature
    Private ReadOnly ExpectedHandshakeResponse As Byte() = {
        &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0,
        &H0, &H0, &H0, &H0, &H0, &H0, &HFF, &H0, &H0, &H0,
        &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0,
        &H0, &HD0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0,
        &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0, &H0,
        &H0, &H0, &H0, &H0, &H0, &H0, &H51, &H79, &H40, &H32,
        &H2E, &H30, &H68, &HC0, &HFD, &H40, &HCC, &HC0, &HBF, &H0,
        &HFF, &H68, &HC0, &HFD, &H40, &HEE, &HC0, &HE5, &H80
    }

#End Region

#Region "Serial Communications (Setup / Connect / Send / Receive)"

    Private Sub SetDefaults()
        ' Reset serial state and refresh COM ports
        Try
            If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                SerialPort1.Close()
            End If
        Catch
            ' ignore
        End Try

        hardwareVerified = False
        ConnectionStatusLabel.Text = "No connection"
        RefreshPorts()
    End Sub

    Private Sub RefreshPorts()
        Dim ports() As String = SerialPort.GetPortNames()

        PortsComboBox.Items.Clear()
        For Each port As String In ports
            PortsComboBox.Items.Add(port)
        Next

        Try
            PortsComboBox.SelectedIndex = 0
        Catch
            ' No ports found
        End Try
    End Sub

    Private Sub AutoConnect()
        RefreshPorts()

        For Each port As String In PortsComboBox.Items
            PortsComboBox.SelectedItem = port

            Try
                Connect()

                If SerialPort1.IsOpen Then
                    WriteHandshake()
                    Threading.Thread.Sleep(200)
                    If IsQuietBoard() Then
                        ConnectionStatusLabel.Text = $"Connected automatically on {SerialPort1.PortName}"
                        Exit For
                    Else
                        SerialPort1.Close()
                    End If
                End If
            Catch
                ' Skip invalid port
            End Try
        Next

        If Not SerialPort1.IsOpen Then
            ConnectionStatusLabel.Text = "Auto-connect failed or wrong device detected. Try manual connect."
        End If
    End Sub

    Private Sub Connect()
        Try
            If SerialPort1 Is Nothing Then Exit Sub

            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            End If

            SerialPort1.BaudRate = 115200
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.PortName = CStr(PortsComboBox.SelectedItem)

            SerialPort1.Open()

        Catch ex As Exception
            MsgBox(ex.Message)
            SetDefaults()
        End Try
    End Sub

    Private Sub Send(data() As Byte)
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Exit Sub

        Try
            SerialPort1.ReadExisting()
            SerialPort1.Write(data, 0, data.Length)
        Catch
            ' ignore
        End Try
    End Sub

    Private Function Receive() As Byte()
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Return Array.Empty(Of Byte)()

        Dim count As Integer = SerialPort1.BytesToRead
        If count <= 0 Then Return Array.Empty(Of Byte)()

        Dim data(count - 1) As Byte
        SerialPort1.Read(data, 0, count)
        Return data
    End Function

    Private Sub WriteHandshake()
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Exit Sub

        Dim data(0) As Byte
        data(0) = &B11110000
        SerialPort1.Write(data, 0, 1)
    End Sub

#End Region

#Region "Polling / Requests"

    Private Sub RequestAllAnalogInputs()
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Exit Sub

        Dim cmd(0) As Byte
        cmd(0) = &H30 ' Request all analog channels
        SerialPort1.Write(cmd, 0, 1)
    End Sub

    Private Sub ReadTemperatureAndDigital()
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Exit Sub

        ' NOTE: currently both commands use &H30 (same packet request)
        Dim cmdAnalog(0) As Byte
        cmdAnalog(0) = &H30
        SerialPort1.Write(cmdAnalog, 0, 1)

        Dim cmdDigital(0) As Byte
        cmdDigital(0) = &H30
        SerialPort1.Write(cmdDigital, 0, 1)
    End Sub

    ' Toggle placeholder (not currently used)
    Private ReadAnalogNext As Boolean = True

    Private Sub ReadTimer_Tick(sender As Object, e As EventArgs) Handles ReadTimer.Tick
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Exit Sub

        ' Request combined packet (digital + analog)
        Dim cmd(0) As Byte
        cmd(0) = &H30
        SerialPort1.Write(cmd, 0, 1)

        CurrentTimeTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy")
        ClockTextBox.Text = DateTime.Now.ToString("hh:mm:s")
    End Sub

#End Region

#Region "Safety Check"

    Private Sub SafetyTimer_Tick(sender As Object, e As EventArgs) Handles SafetyTimer.Tick
        ' Copy to locals to reduce race conditions during rapid serial updates
        Dim localCurrentTemp As Single = currentemp
        Dim localHardwareTemp As Single = hardwaretemp
        Dim activeMode As String = ""

        If heatLatched Then
            activeMode = "Heating"

            If localCurrentTemp <= localHardwareTemp Then
                MessageBox.Show(
                    "Heating Failure Detected: Room temperature is not increasing as expected." & vbCrLf &
                    $"Room: {localCurrentTemp:F1} °F, Hardware: {localHardwareTemp:F1} °F",
                    "Safety Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )
            End If

        ElseIf coolLatched Then
            activeMode = "Cooling"

            If localCurrentTemp >= localHardwareTemp Then
                MessageBox.Show(
                    "Cooling Failure Detected: Room temperature is not decreasing as expected." & vbCrLf &
                    $"Room: {localCurrentTemp:F1} °F, Hardware: {localHardwareTemp:F1} °F",
                    "Safety Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )
            End If
        End If

        ' Debug logging to UI
        If activeMode <> "" Then
            SerialTextBox.AppendText($"{DateTime.Now:HH:mm:ss} - Safety check ({activeMode}): Room {localCurrentTemp:F1} °F, Hardware {localHardwareTemp:F1} °F{vbCrLf}")
        End If
    End Sub

#End Region

#Region "Hot-Plug Detection"

    ' IMPORTANT: Add Handles DetectTimer.Tick so this actually runs when DetectTimer fires.
    Private Sub DetectTimer_Tick(sender As Object, e As EventArgs) Handles DetectTimer.Tick
        If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then Return ' Already connected

        Dim currentPorts As String() = SerialPort.GetPortNames()
        Dim newPorts = currentPorts.Except(previouslyAvailablePorts).ToList()

        If newPorts.Count > 0 Then
            For Each port As String In newPorts
                Try
                    PortsComboBox.SelectedItem = port
                    Connect()

                    If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                        WriteHandshake()
                        Threading.Thread.Sleep(200)

                        If IsQuietBoard() Then
                            Me.Invoke(Sub()
                                          ConnectionStatusLabel.Text = $"Auto-connected to compliant device on {SerialPort1.PortName}"
                                      End Sub)

                            previouslyAvailablePorts = currentPorts.ToList()
                            Return
                        Else
                            SerialPort1.Close()
                        End If
                    End If
                Catch
                    ' Skip invalid/new port
                End Try
            Next
        End If

        previouslyAvailablePorts = currentPorts.ToList()
    End Sub

#End Region

#Region "SerialPort Events"

    Private Sub SerialPort1_ErrorReceived(sender As Object, e As SerialErrorReceivedEventArgs) Handles SerialPort1.ErrorReceived
        LogError("Connection lost (error detected).")

        Me.Invoke(Sub()
                      If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                          Try
                              SerialPort1.Close()
                          Catch
                              ' ignore
                          End Try
                      End If

                      ConnectionStatusLabel.Text = "Connection lost (error detected)."
                      SetDefaults()
                  End Sub)
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived

        Try
            Dim bytesToRead As Integer = SerialPort1.BytesToRead
            If bytesToRead < 4 Then Exit Sub

            Dim buffer(bytesToRead - 1) As Byte
            SerialPort1.Read(buffer, 0, bytesToRead)

            ' Display raw packet
            Dim hex As String = BitConverter.ToString(buffer).Replace("-", " ")
            Me.Invoke(Sub()
                          SerialTextBox.AppendText($"{DateTime.Now:HH:mm:ss.fff} → {hex}{vbCrLf}")
                          SerialTextBox.SelectionStart = SerialTextBox.Text.Length
                          SerialTextBox.ScrollToCaret()
                      End Sub)

            ' DIGITAL INPUTS (Byte 0) — inverted due to pull-ups (pressed = 1)
            Dim digitalByte As Byte = buffer(0)
            digitalByte = Not digitalByte
            latestDigitalByte = digitalByte

            Dim safetyInterlock As Boolean = (digitalByte And &H1) <> 0   ' Bit 0
            Dim heatButtonNow As Boolean = (digitalByte And &H2) <> 0     ' Bit 1
            Dim fanButtonNow As Boolean = (digitalByte And &H4) <> 0      ' Bit 2
            Dim coolButtonNow As Boolean = (digitalByte And &H8) <> 0     ' Bit 3

            ' Display bits (LSB-first)
            Dim binary As String = Convert.ToString(digitalByte, 2).PadLeft(8, "0"c)
            Dim displayBits As String = New String(binary.Reverse().ToArray())
            WriteToTextBox(DigitalInputsTextBox, displayBits)

            ' === LATCHING (RISING EDGE DETECTION) ===
            If safetyInterlock AndAlso Not prevInterlockBit Then
                interlockLatched = True
                heatLatched = False
                coolLatched = False
                fanLatched = False

                LogError("Safety interlock pressed")

                ' Notify hardware (0x20) on interlock trigger
                Try
                    If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                        Dim data(0) As Byte
                        data(0) = &H20
                        Send(data)
                    End If
                Catch
                    ' ignore
                End Try
            End If

            ' Example reset: release fan button clears interlock latch
            If Not fanButtonNow AndAlso prevFanBit AndAlso interlockLatched Then
                interlockLatched = False
            End If

            If Not interlockLatched Then
                ' Heat toggle
                If heatButtonNow AndAlso Not prevHeatBit Then
                    heatLatched = Not heatLatched
                    If heatLatched Then coolLatched = False
                End If

                ' Cool toggle
                If coolButtonNow AndAlso Not prevCoolBit Then
                    coolLatched = Not coolLatched
                    If coolLatched Then heatLatched = False
                End If

                ' Fan toggle
                If fanButtonNow AndAlso Not prevFanBit Then
                    fanLatched = Not fanLatched
                End If
            Else
                ' During interlock latch, force all modes off
                heatLatched = False
                coolLatched = False
                fanLatched = False
            End If

            ' Update previous states
            prevHeatBit = heatButtonNow
            prevCoolBit = coolButtonNow
            prevFanBit = fanButtonNow
            prevInterlockBit = safetyInterlock

            ' === TEMPERATURE PARSING (Bytes 1–4) ===
            Dim adcLow0 As Integer = buffer(2)
            Dim adcHigh0 As Integer = buffer(1)
            Dim adc10bit0 As Integer = (adcHigh0 << 8) Or adcLow0
            Dim currentTemp As Single = 32 + (adc10bit0 / 1023.0F) * 0.95F

            Dim adcLow1 As Integer = buffer(4)
            Dim adcHigh1 As Integer = buffer(3)
            Dim adc10bit1 As Integer = (adcHigh1 << 8) Or adcLow1
            Dim hardwareTemp As Single = 40 + (adc10bit1 / 1023.0F) * 0.95F

            Me.currentemp = currentTemp
            Me.hardwaretemp = hardwareTemp

            WriteToTextBox(CurrentTempTextBox, currentTemp.ToString("F1") & " °F")
            WriteToTextBox(HardwareTextBox, hardwareTemp.ToString("F1") & " °F")

            ' === MODE DISPLAY + HARDWARE MODE CODES ===
            Dim mode As String = "OFF"

            If interlockLatched Then
                Dim data(0) As Byte
                data(0) = &H21
                Send(data)

                mode = "SAFETY LOCKOUT"
            Else
                If coolLatched Then
                    mode = "COOLING"

                    Dim data(0) As Byte
                    data(0) = &H22
                    Send(data)

                ElseIf heatLatched Then
                    Dim data(0) As Byte
                    data(0) = &H24
                    Send(data)

                    mode = "HEATING"
                End If
            End If

            Me.Invoke(Sub()
                          OperationTextBox.Text = mode
                          ' Removed: BackColor/ForeColor/Font changes (default UI styling only)
                      End Sub)

            ' Enable/disable safety timer only when heating/cooling
            Me.Invoke(Sub()
                          If heatLatched Or coolLatched Then
                              If Not SafetyTimer.Enabled Then SafetyTimer.Start()
                          Else
                              If SafetyTimer.Enabled Then SafetyTimer.Stop()
                          End If
                      End Sub)

            ' === OUTPUT CONTROL LOGIC ===
            If interlockLatched Then
                fanOutput = False
                heatOutput = False
                coolOutput = False
            Else
                fanOutput = If(fanLatched, True, (heatLatched Or coolLatched))
                heatOutput = heatLatched
                coolOutput = coolLatched
            End If

            ' Fan status UI (text only; no color changes)
            Me.Invoke(Sub()
                          If interlockLatched Then
                              FanTextBox.Text = "FAN: OFF (SAFETY)"

                          ElseIf fanOutput Then
                              If fanLatched Then
                                  FanTextBox.Text = "FAN: ON (MANUAL OVERRIDE)"
                              Else
                                  FanTextBox.Text = "FAN: ON (AUTO - " & OperationTextBox.Text & ")"

                                  Dim data(0) As Byte
                                  data(0) = &H22
                                  Send(data)
                              End If
                          Else
                              FanTextBox.Text = "FAN: OFF"
                          End If

                          ' Removed: BackColor/ForeColor/Font changes (default UI styling only)
                      End Sub)

        Catch ioEx As IO.IOException
            Me.Invoke(Sub()
                          ConnectionStatusLabel.Text = "Connection lost (device removed)."
                          SetDefaults()
                      End Sub)

        Catch ex As Exception
            Me.Invoke(Sub()
                          SerialTextBox.AppendText($"Error: {ex.Message}{vbCrLf}")
                      End Sub)
        End Try
    End Sub

#End Region

#Region "Hardware Verification (Handshake)"

    Private Function IsQuietBoard() As Boolean
        If hardwareVerified Then Return True
        If SerialPort1 Is Nothing OrElse Not SerialPort1.IsOpen Then Return False

        SerialPort1.DiscardInBuffer()

        Dim handshake(0) As Byte
        handshake(0) = &HF0
        Send(handshake)

        Dim timeoutMs As Integer = 500
        Dim waitedMs As Integer = 0

        Do While SerialPort1.BytesToRead < ExpectedHandshakeResponse.Length AndAlso waitedMs < timeoutMs
            Threading.Thread.Sleep(200)
            waitedMs += 10
        Loop

        If SerialPort1.BytesToRead < ExpectedHandshakeResponse.Length Then
            hardwareVerified = False
            Return False
        End If

        Dim resp(ExpectedHandshakeResponse.Length - 1) As Byte
        Dim bytesRead As Integer = SerialPort1.Read(resp, 0, ExpectedHandshakeResponse.Length)

        If bytesRead <> ExpectedHandshakeResponse.Length Then
            hardwareVerified = False
            Return False
        End If

        For i As Integer = 0 To ExpectedHandshakeResponse.Length - 1
            If resp(i) <> ExpectedHandshakeResponse(i) Then
                hardwareVerified = False
                Return False
            End If
        Next

        hardwareVerified = True
        Return True
    End Function

#End Region

#Region "UI Thread Helper"

    Public Sub WriteToTextBox(targetTextBox As TextBox, content As String)
        If targetTextBox.InvokeRequired Then
            targetTextBox.Invoke(New Action(Sub() targetTextBox.Text = content))
        Else
            targetTextBox.Text = content
        End If
    End Sub

#End Region

#Region "Setpoint Commands"

    Private Sub SendSetpoint()
        Try
            Dim setpoint As Integer = CInt(SetTempTextBox.Text)

            If setpoint < 0 OrElse setpoint > 100 Then
                MsgBox("Setpoint must be between 0 and 100.")
                Exit Sub
            End If

            ' TODO: Send setpoint to hardware (protocol-specific)
        Catch ex As Exception
            MsgBox($"Error sending setpoint: {ex.Message}")
        End Try
    End Sub

    Private Sub SendLowSetpoint()
        Try
            Dim lowSetpoint As Integer = CInt(LowTempTextBox.Text)
            Dim highSetpoint As Integer = CInt(SetTempTextBox.Text)

            If lowSetpoint >= highSetpoint Then
                MsgBox("Low threshold must be lower than High threshold.")
                Exit Sub
            End If

            ' TODO: Send low setpoint to hardware (protocol-specific)
        Catch
            ' ignore parse errors
        End Try
    End Sub

#End Region

#Region "Form / Control Event Handlers"

    Private Sub HVAC_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()

        ReadTimer.Interval = 100
        ReadTimer.Enabled = True

        ' Removed: any forced form font/background color so it uses designer/default theme colors
        ' Me.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        ' Me.BackColor = Color.FromArgb(244, 121, 32)

        FilterTimer.Interval = 30000
        FilterTimer.Enabled = True

        LoadSettings()

        IncrementTemperatureHigh(SetTempTextBox, 0)
        IncrementTemperatureLow(LowTempTextBox, 0)

        If PortsComboBoxHasSelection() Then
            Connect()
            WriteHandshake()
        End If

        Dim availablePorts = SerialPort.GetPortNames()
        previouslyAvailablePorts = availablePorts.ToList()

        If availablePorts.Length = 0 Then
            ConnectionStatusLabel.Text = "No serial devices detected. Waiting for device..."
            SerialTimer.Interval = 5000
            SerialTimer.Start()
        Else
            If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                Dim start(10) As Byte
                start(0) = &H30
                start(1) = &H20
                start(2) = &H0
                start(3) = &H5F
                start(4) = &H41
                start(5) = &H19
                start(6) = &H0
                start(7) = &H42
                start(8) = &H2
                start(9) = &H80

                Try
                    SerialPort1.Write(start, 0, 10)
                Catch
                    Me.Invoke(Sub()
                                  ConnectionStatusLabel.Text = "Failed to send startup packet."
                              End Sub)
                End Try
            End If
        End If

        DetectTimer.Interval = 3000
        DetectTimer.Start()
    End Sub

    Private Function PortsComboBoxHasSelection() As Boolean
        Return PortsComboBox.SelectedItem IsNot Nothing AndAlso
               Not String.IsNullOrEmpty(PortsComboBox.SelectedItem.ToString())
    End Function

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripButton_Click(sender As Object, e As EventArgs) Handles ExitToolStripButton.Click
        Me.Close()
    End Sub

    Private Sub SetTempTextBox_TextChanged(sender As Object, e As EventArgs) Handles SetTempTextBox.TextChanged
        SaveSettings()
    End Sub

    Private Sub LowTempTextBox_TextChanged(sender As Object, e As EventArgs) Handles LowTempTextBox.TextChanged
        SaveSettings()
    End Sub

    Private Sub PortsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PortsComboBox.SelectedIndexChanged
        SaveSettings()
    End Sub

    Private Sub IncrementHighTempButton_Click(sender As Object, e As EventArgs) Handles IncrementHighTempButton.Click
        IncrementTemperatureHigh(SetTempTextBox, 0.5F)
    End Sub

    Private Sub DecrementHighTempButton_Click(sender As Object, e As EventArgs) Handles DecrementHighTempButton.Click
        ' NOTE: Preserved original behavior (it increments here too)
        IncrementTemperatureHigh(SetTempTextBox, 0.5F)
    End Sub

    Private Sub IncrementLowTempButton_Click(sender As Object, e As EventArgs) Handles IncrementLowTempButton.Click
        IncrementTemperatureLow(LowTempTextBox, 0.5F)
    End Sub

    Private Sub DecrementLowTempButton_Click(sender As Object, e As EventArgs) Handles DecrementLowTempButton.Click
        IncrementTemperatureLow(LowTempTextBox, -0.5F)
    End Sub

#End Region

#Region "Temperature Increment Helpers"

    Private Sub IncrementTemperatureHigh(targetTextBox As TextBox, change As Single)
        Dim currentTemp As Single
        If Not Single.TryParse(targetTextBox.Text, currentTemp) Then
            currentTemp = 90.0F
        End If

        Dim newTemp As Single = currentTemp + change

        If newTemp < 50.0F Then newTemp = 50.0F
        If newTemp > 90.0F Then newTemp = 90.0F

        WriteToTextBox(targetTextBox, newTemp.ToString("F1"))

        If targetTextBox Is SetTempTextBox Then
            SendSetpoint()
        ElseIf targetTextBox Is LowTempTextBox Then
            SendLowSetpoint()
        End If

        SaveSettings()
    End Sub

    Private Sub IncrementTemperatureLow(targetTextBox As TextBox, change As Single)
        Dim currentTemp As Single
        If Not Single.TryParse(targetTextBox.Text, currentTemp) Then
            currentTemp = 70.0F
        End If

        Dim newTemp As Single = currentTemp + change

        If newTemp < 50.0F Then newTemp = 50.0F
        If newTemp > 90.0F Then newTemp = 90.0F

        WriteToTextBox(targetTextBox, newTemp.ToString("F1"))

        If targetTextBox Is SetTempTextBox Then
            SendSetpoint()
        ElseIf targetTextBox Is LowTempTextBox Then
            SendLowSetpoint()
        End If

        SaveSettings()
    End Sub

#End Region

#Region "Settings Load/Save"

    Private Sub SaveSettings()
        Try
            Dim lines As New List(Of String)

            ' Line 0: High setpoint
            lines.Add(SetTempTextBox.Text.Trim())

            ' Line 1: Low setpoint
            lines.Add(LowTempTextBox.Text.Trim())

            ' Line 2: Selected COM port
            If PortsComboBox.SelectedItem IsNot Nothing Then
                lines.Add(PortsComboBox.SelectedItem.ToString())
            Else
                lines.Add("")
            End If

            IO.File.WriteAllLines(GetSettingsPath(), lines)
        Catch
            ' fail silently
        End Try
    End Sub

    Private Sub LoadSettings()
        Dim path As String = GetSettingsPath()
        If Not IO.File.Exists(path) Then Exit Sub

        Try
            Dim lines() As String = IO.File.ReadAllLines(path)

            If lines.Length > 0 AndAlso Not String.IsNullOrWhiteSpace(lines(0)) Then
                SetTempTextBox.Text = lines(0).Trim()
            End If

            Dim d As Double
            If lines.Length > 1 AndAlso Double.TryParse(lines(1), d) Then
                LowTempTextBox.Text = lines(1).Trim()
            End If

            Dim portIndex As Integer = 2
            If lines.Length > portIndex AndAlso Not String.IsNullOrWhiteSpace(lines(portIndex)) Then
                Dim savedPort As String = lines(portIndex).Trim()

                For i As Integer = 0 To PortsComboBox.Items.Count - 1
                    If PortsComboBox.Items(i).ToString() = savedPort Then
                        PortsComboBox.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

        Catch
            ' ignore corrupt file
        End Try
    End Sub

#End Region

#Region "Filter Fault Logic"

    Private Sub FilterTimer_Tick(sender As Object, e As EventArgs) Handles FilterTimer.Tick
        If interlockLatched Then Exit Sub

        Dim filterDirtyNow As Boolean = (latestDigitalByte And &H10) <> 0

        ' Detect continuous hold for FILTER_HOLD_SECONDS
        If filterDirtyNow AndAlso Not prevFilterDirtyBit Then
            filterDirtyStartTime = DateTime.Now
        ElseIf Not filterDirtyNow Then
            filterDirtyStartTime = DateTime.MinValue
        End If

        If filterDirtyNow AndAlso filterDirtyStartTime <> DateTime.MinValue Then
            If (DateTime.Now - filterDirtyStartTime).TotalSeconds >= FILTER_HOLD_SECONDS Then
                TriggerFilterFault()
            End If
        End If

        prevFilterDirtyBit = filterDirtyNow

        ' Track fan runtime accumulation
        If fanOutput Then
            If fanRuntimeStart = DateTime.MinValue Then fanRuntimeStart = DateTime.Now
        Else
            If fanRuntimeStart <> DateTime.MinValue Then
                cumulativeFanSeconds += CLng((DateTime.Now - fanRuntimeStart).TotalSeconds)
                fanRuntimeStart = DateTime.MinValue
            End If
        End If

        ' Periodic check: after FILTER_CHECK_MINUTES of runtime, if filter is dirty => fault
        If fanOutput AndAlso cumulativeFanSeconds >= (FILTER_CHECK_MINUTES * 60) Then
            If filterDirtyNow Then TriggerFilterFault()
            cumulativeFanSeconds = 0
        End If
    End Sub

    Private Sub TriggerFilterFault()
        LogError("Filter Error Detected: Filter is dirty or input held too long.")

        heatLatched = False
        coolLatched = False
        heatOutput = False
        coolOutput = False

        MessageBox.Show(
            "Filter Error Detected: Filter is dirty or input held too long." & vbCrLf &
            "Heating and cooling have been disabled. Replace/clean filter and reset system.",
            "Filter Fault",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        Me.Invoke(Sub()
                      OperationTextBox.Text = "FILTER FAULT"
                      ' Removed: BackColor/ForeColor changes
                  End Sub)
    End Sub

#End Region

#Region "Error Logging"

    Private Sub LogError(message As String)
        Try
            Dim path As String = GetErrorLogPath()
            Dim entry As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}"
            IO.File.AppendAllText(path, entry)
        Catch
            ' ignore IO errors
        End Try
    End Sub

#End Region

#Region "Serial Device Missing Prompt"

    Private Sub SerialTimer_Tick(sender As Object, e As EventArgs) Handles SerialTimer.Tick
        Dim ports = SerialPort.GetPortNames()

        If ports.Length > 0 Then
            SerialTimer.Stop()
            previouslyAvailablePorts = ports.ToList()

            Try
                If SerialPort1 IsNot Nothing AndAlso SerialPort1.IsOpen Then
                    Dim start(10) As Byte
                    start(0) = &H30
                    start(1) = &H20
                    start(2) = &H0
                    start(3) = &H5F
                    start(4) = &H41
                    start(5) = &H19
                    start(6) = &H0
                    start(7) = &H42
                    start(8) = &H2
                    start(9) = &H80
                    ' NOTE: Preserved original behavior: packet is built but not written here.
                End If
            Catch
                ' ignore
            End Try

            Return
        End If

        Dim res = MessageBox.Show(
            "No serial device detected. Please connect your device now." & vbCrLf &
            "Would you like to retry checking?",
            "No Serial Device",
            MessageBoxButtons.RetryCancel,
            MessageBoxIcon.Warning
        )

        If res = DialogResult.Cancel Then
            SerialTimer.Stop()
            ConnectionStatusLabel.Text = "No serial device. Connect device to enable communication."
        Else
            ConnectionStatusLabel.Text = "Retrying to detect serial device..."
            RefreshPorts()
        End If
    End Sub

#End Region

#Region "Misc Helpers"

    Private Function PortsBoxContains(portName As String) As Boolean
        For Each item In PortsComboBox.Items
            If item.ToString() = portName Then Return True
        Next
        Return False
    End Function

#End Region

End Class