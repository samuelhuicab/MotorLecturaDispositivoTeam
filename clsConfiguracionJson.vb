Public Class RspTeamEncabezado
    Public Property teamviewer_id As String
    Public Property device_name As String
    Public Property group_name As String
    Public Property items As New List(Of RspItemsEncabezado)
End Class

Public Class RspItemsEncabezado
    Public Property name As String
    Public Property type As String
    Public Property details As String
    Public Property manufacturer As String
End Class
