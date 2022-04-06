Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports System.Web

Module Correos
    Private correos As New MailMessage
    Private envios As New SmtpClient

    Public cn As New SqlConnection
    Public cmd As New SqlCommand

    Public Sub Correo(emisor As TextBox, password As TextBox, destinatario As String, asunto As String, mensajeHTML As TextBox, rutaImagen As TextBox)

        Try
            ' Montamos la estructura básica del mensaje...
            Dim mail As MailMessage = New MailMessage()
            mail.From = New MailAddress(emisor.Text)
            mail.To.Add(destinatario)
            mail.Subject = asunto

            ' Creamos la vista para clientes que
            ' sólo pueden acceder a texto plano...
            'Dim text As String = "Hola, ayer estuve disfrutando de un paisaje estupendo."
            'Dim plainView As AlternateView = AlternateView.CreateAlternateViewFromString(text, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain)


            If rutaImagen.Text <> "" Then
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(mensajeHTML.Text, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html)
                Dim img As LinkedResource = New LinkedResource(rutaImagen.Text, MediaTypeNames.Image.Jpeg)
                img.ContentId = "wbtestimagenbg"
                htmlView.LinkedResources.Add(img)
                'mail.AlternateViews.Add(plainView)
                mail.AlternateViews.Add(htmlView)

                envios.Credentials = New NetworkCredential(emisor.Text, password.Text)
                envios.Host = "smtp.gmail.com"
                envios.Port = 587
                envios.EnableSsl = True
                envios.Send(mail)
            Else
                Dim htmlView As AlternateView = AlternateView.CreateAlternateViewFromString(mensajeHTML.Text, System.Text.Encoding.UTF8, MediaTypeNames.Text.Html)
                mail.AlternateViews.Add(htmlView)

                envios.Credentials = New NetworkCredential(emisor.Text, password.Text)
                envios.Host = "smtp.gmail.com"
                envios.Port = 587
                envios.EnableSsl = True
                envios.Send(mail)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub New()
        Try
            cn = New SqlConnection("Data Source=Alpha-PC;Initial Catalog=correo;Integrated Security=True")
            cn.Open()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            MessageBox.Show(ex.Message)
            cn.Close()
        End Try
    End Sub

    Public Sub cargarGrid(DtGrid As DataGridView)
        Dim da As New SqlDataAdapter
        Dim dt As New DataTable

        Try
            dt.Clear()
            cmd = New SqlCommand("select Correo, Nombre, Monto from Correo", cn)
            cmd.CommandType = CommandType.Text
            da = New SqlDataAdapter(cmd)
            dt = New DataTable

            da.Fill(dt)
            DtGrid.DataSource = dt
            cn.Close()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            MessageBox.Show(ex.Message)
            cn.Close()
        End Try
    End Sub
    Function ExportarExcel(ByVal ElGrid As DataGridView) As Boolean

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        Try

            'Añadimos el Libro al programa, y la hoja al libro
            exLibro = exApp.Workbooks.Add
            exHoja = exLibro.Worksheets.Add()

            ' ¿Cuantas columnas y cuantas filas?
            Dim NCol As Integer = ElGrid.ColumnCount
            Dim NRow As Integer = ElGrid.RowCount
            'Aqui recorremos todas las filas, y por cada fila todas las columnas

            'y vamos escribiendo.
            For i As Integer = 1 To NCol
                exHoja.Cells.Item(1, i) = ElGrid.Columns(i - 1).Name.ToString
            Next

            For Fila As Integer = 0 To NRow - 1
                For Col As Integer = 0 To NCol - 1
                    exHoja.Cells.Item(Fila + 2, Col + 1) = ElGrid.Item(Col, Fila).Value
                Next
            Next

            'Titulo en negrita, Alineado al centro y que el tamaño de la columna
            'se ajuste al texto
            exHoja.Rows.Item(1).Font.Bold = 1
            exHoja.Rows.Item(1).HorizontalAlignment = 3
            exHoja.Columns.AutoFit()

            'Aplicación visible
            exApp.Application.Visible = True
            exHoja = Nothing
            exLibro = Nothing
            exApp = Nothing

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al exportar a Excel")
            Return False

        End Try

        Return True

    End Function


End Module
