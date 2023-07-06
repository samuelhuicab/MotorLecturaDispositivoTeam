Imports System.Net
Imports Newtonsoft.Json
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Form1
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MEMORYSTATUSEX
        Public dwLength As UInteger
        Public dwMemoryLoad As UInteger
        Public ullTotalPhys As ULong
        Public ullAvailPhys As ULong
        Public ullTotalPageFile As ULong
        Public ullAvailPageFile As ULong
        Public ullTotalVirtual As ULong
        Public ullAvailVirtual As ULong
        Public ullAvailExtendedVirtual As ULong
    End Structure
    Dim token As String = ""
    Dim jdataHardware
    <DllImport("kernel32.dll")>
    Public Shared Function GlobalMemoryStatusEx(ByRef lpBuffer As MEMORYSTATUSEX) As Boolean
    End Function
    <DllImport("kernel32.dll")>
    Public Shared Function GetSystemTimes(ByRef lpIdleTime As Long, ByRef lpKernelTime As Long, ByRef lpUserTime As Long) As Boolean
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function GetLogicalDriveStrings(ByVal nBufferLength As Integer, ByVal lpBuffer As StringBuilder) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function GetDiskFreeSpaceEx(ByVal lpDirectoryName As String, ByRef lpFreeBytesAvailable As ULong, ByRef lpTotalNumberOfBytes As ULong, ByRef lpTotalNumberOfFreeBytes As ULong) As Boolean
    End Function
    <StructLayout(LayoutKind.Sequential)>
    Public Structure IP_ADAPTER_ADDRESSES
        Public Length As UInteger
        Public IfIndex As Integer
        Public tsext As IntPtr
        Public AdapterName As String
        ' Otras propiedades del adaptador de red
    End Structure
    <DllImport("iphlpapi.dll", CharSet:=CharSet.Auto)>
    Public Shared Function GetAdaptersAddresses(ByVal Family As Integer, ByVal Flags As UInteger, ByVal Reserved As IntPtr, ByVal pAdapterAddresses As IntPtr, ByRef pOutBufLen As UInteger) As Integer
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'ProcesoUnico()
            token = "20852347-9xsrJxKqpyA3acZwKl4J"
            Me.MaximizeBox = False
            lblfechaanalisis.Text = Format(Now(), "yyyy-MM-dd HH:mm:ss")
            Timer1_Tick(sender, e)
            Timer1.Start()
        Catch ex As Exception

        End Try
    End Sub
    Private Function ValidateCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Icon = New Icon("src/img/7898471_monitoring_business_finance_office_marketing_icon.ico") ' Ruta al archivo de icono
            NotifyIcon1.Text = "Analizador Dispositivo"

            ' Ocultar la ventana principal
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
            Me.Hide()
        End If
    End Sub
    Public Sub ProcesoUnico()
        Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName
        Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)
        If Process.GetProcessesByName(aProcName).Length > 1 Then
            MessageBox.Show("El sistema ya se encuentra en ejecucion " & aModuleName.ToString)
            'forzarcierre = True
            Application.Exit()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Try
        '    lblfechaanalisis.Text = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        '    Using client As New WebClient
        '        client.Headers.Add("Authorization", "Bearer " & token)
        '        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateCertificate
        '        Dim responseBody As String = client.DownloadString("https://webapi.teamviewer.com/api/v1/monitoring/devices/1667864555/hardware")
        '        jdataHardware = JsonConvert.DeserializeObject(Of RspTeamEncabezado)(responseBody)
        '    End Using
        '    MsgBox(jdataHardware.device_name)
        'Catch ex As Exception

        'End Try
        lblfechaanalisis.Text = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        ' Obtener información de la RAM
        Dim memoryStatus As New MEMORYSTATUSEX()
        memoryStatus.dwLength = CType(Marshal.SizeOf(memoryStatus), UInteger)
        GlobalMemoryStatusEx(memoryStatus)
        Dim totalRAM As Double = memoryStatus.ullTotalPhys / 1024 / 1024 ' Convertir a megabytes
        Dim availableRAM As Double = memoryStatus.ullAvailPhys / 1024 / 1024 ' Convertir a megabytes

        ' Obtener información de la CPU
        Dim idleTime As Long
        Dim kernelTime As Long
        Dim userTime As Long
        GetSystemTimes(idleTime, kernelTime, userTime)
        Dim cpuUsage As Double = (kernelTime + userTime) / (Environment.ProcessorCount * 10000) ' Porcentaje de uso de CPU

        ' Obtener información de los discos duros
        Dim driveNames As New StringBuilder(128)
        GetLogicalDriveStrings(driveNames.Capacity, driveNames)
        Dim drives() As String = driveNames.ToString().Split(New Char() {ControlChars.NullChar}, StringSplitOptions.RemoveEmptyEntries)
        For Each drive As String In drives
            Dim freeBytesAvailable As ULong
            Dim totalNumberOfBytes As ULong
            Dim totalNumberOfFreeBytes As ULong
            GetDiskFreeSpaceEx(drive, freeBytesAvailable, totalNumberOfBytes, totalNumberOfFreeBytes)
            Dim totalSpace As Double = totalNumberOfBytes / 1024 / 1024 / 1024 ' Convertir a gigabytes
            Dim availableSpace As Double = freeBytesAvailable / 1024 / 1024 / 1024 ' Convertir a gigabytes
            ' Aquí puedes hacer lo que desees con la información de cada disco duro
        Next

        ' Obtener información de las tarjetas de red
        Dim adapterAddressesSize As UInteger = 0
        GetAdaptersAddresses(0, 0, IntPtr.Zero, IntPtr.Zero, adapterAddressesSize)
        Dim adapterAddressesBuffer As IntPtr = Marshal.AllocHGlobal(CType(adapterAddressesSize, Integer))
        GetAdaptersAddresses(0, 0, IntPtr.Zero, adapterAddressesBuffer, adapterAddressesSize)
        Dim adapterAddressPtr As IntPtr = adapterAddressesBuffer
        While adapterAddressPtr <> IntPtr.Zero
            Dim adapterAddress As IP_ADAPTER_ADDRESSES = Marshal.PtrToStructure(Of IP_ADAPTER_ADDRESSES)(adapterAddressPtr)
            ' Aquí puedes hacer lo que desees con la información de cada tarjeta de red
            adapterAddressPtr = adapterAddress.tsext
        End While
        Marshal.FreeHGlobal(adapterAddressesBuffer)

        ' Aquí puedes mostrar o utilizar la información obtenida
    End Sub
End Class
