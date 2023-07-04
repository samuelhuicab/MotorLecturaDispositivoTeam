Imports System.Net
Imports Newtonsoft.Json
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Public Class Form1
    Dim token As String = ""
    Dim jdataHardware
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            ProcesoUnico()
            token = "20852347-9xsrJxKqpyA3acZwKl4J"
            Me.MaximizeBox = False
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnapi_Click(sender As Object, e As EventArgs) Handles btnapi.Click
        Try
            Using client As New WebClient
                client.Headers.Add("Authorization", "Bearer " & token)
                ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateCertificate
                Dim responseBody As String = client.DownloadString("https://webapi.teamviewer.com/api/v1/monitoring/devices/1667864555/hardware")
                jdataHardware = JsonConvert.DeserializeObject(Of RspTeamEncabezado)(responseBody)
            End Using
            MsgBox(jdataHardware.teamviewer_id)
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
End Class
