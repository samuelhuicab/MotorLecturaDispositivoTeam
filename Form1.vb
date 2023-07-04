Imports System.Net
Imports Newtonsoft.Json
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Public Class Form1
    Dim token As String = ""
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            token = "20852347-9xsrJxKqpyA3acZwKl4J"
        Catch ex As Exception

        End Try
    End Sub
    Private Function ValidateCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Ignorar todos los errores de validación del certificado
        Return True
    End Function

    Private Sub btnapi_Click(sender As Object, e As EventArgs) Handles btnapi.Click
        Try
            Using client As New WebClient
                client.Headers.Add("Authorization", "Bearer " & token)
                ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateCertificate
                Dim responseBody As String = client.DownloadString("https://webapi.teamviewer.com/api/v1/monitoring/devices/1667864555/hardware")
                MsgBox(responseBody)
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class
