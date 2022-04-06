Imports System.Text.RegularExpressions

Public Class Form1


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WebBrowser1.Navigate("about:blank")
        dgvErrores.Columns.Add("Correo", "CorreoError")
        dgvErrores.Columns.Add("Nombre", "Nombre")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dgvDatos.RowCount < 1 Then
            MsgBox("No hay lista de datos para realizar el envío.")
        Else
            Try
                Dim variable As String = ""
                'dgvEnviados.Columns.Add("enviados", "Enviados")
                'For i = 0 To dgvDatos.RowCount - 1 Step 1
                '    dgvEnviados.Rows.Add(dgvDatos.Rows(i).Cells(0).Value)
                '    ' txtEnviados.Value = txtEnviados.Value + 1
                'Next
                Dim inicio = Now.ToString("HH:mm:ss")
                Dim fin As String

                For Each Fila As DataGridViewRow In dgvDatos.Rows

                    If ValidarCorreo(Fila.Cells(0).Value.ToString) = False Then
                        dgvErrores.Rows.Add(Fila.Cells(0).Value, Fila.Cells(1).Value)
                    Else
                        If txtRuta.Text <> "" Then
                            variable = Fila.Cells(1).Value
                            txtHTML.Text = Replace(txtHTML.Text, "@Cliente", variable)
                            Correo(txtEmisor, txtClave, Fila.Cells(0).Value, txtAsunto.Text, txtHTML, txtRuta)
                            txtEnviados.Value = txtEnviados.Value + 1
                            txtHTML.Text = Replace(txtHTML.Text, variable, "@Cliente")

                            txtTemporal.Text = txtTemporal.Text +
                          "<tr><td>" +
                           Fila.Cells(1).Value +
                           "</td><td>" +
                           Fila.Cells(0).Value +
                           "</td></tr>" + vbNewLine
                        Else
                            variable = Fila.Cells(1).Value
                            txtHTML2.Text = Replace(txtHTML2.Text, "@Cliente", variable)
                            Correo(txtEmisor, txtClave, Fila.Cells(0).Value, txtAsunto.Text, txtHTML2, txtRuta)
                            txtEnviados.Value = txtEnviados.Value + 1
                            txtHTML2.Text = Replace(txtHTML2.Text, variable, "@Cliente")

                            txtTemporal.Text = txtTemporal.Text +
                          "<tr><td>" +
                           Fila.Cells(1).Value +
                           "</td><td>" +
                           Fila.Cells(0).Value +
                           "</td></tr>" + vbNewLine
                        End If

                    End If
                Next
                txtConfirmacion.Text = Replace(txtConfirmacion.Text, "@Datos", txtTemporal.Text)
                Correo(txtEmisor, txtClave, "jhayr.marin95@gmail.com,ingeniandogt@gmail.com", "Confirmación", txtConfirmacion, txtRuta)

                fin = Now.ToString("HH:mm:ss")
                Dim tiempo = DateDiff(DateInterval.Second, Convert.ToDateTime(inicio), Convert.ToDateTime(fin))
                lblCorreo.Text = "Proceso tardó " + tiempo.ToString + " Segundos"
                MsgBox("Proceso completado")
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        cargarGrid(dgvDatos)
        If dgvDatos.RowCount > 0 Then
            txtTotal.Text = dgvDatos.RowCount
        End If
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        WebBrowser1.DocumentText = txtHTML.Text
    End Sub

    Private Sub WebBrowser1_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles WebBrowser1.Navigated
        WebBrowser1.Document.OpenNew(False)
        WebBrowser1.Document.Write(txtHTML.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        WebBrowser1.Refresh()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If dgvErrores.RowCount < 1 Then
            MsgBox("No hay datos para realizar la exportación.")
        Else
            ExportarExcel(dgvErrores)
        End If


    End Sub

    Public Function ValidarCorreo(ByVal sMail As String) As Boolean
        ' retorna true o false   
        Return Regex.IsMatch(sMail, "^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$")
    End Function

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles txtTemporal.TextChanged

    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Me.Close()

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        Me.WindowState = FormWindowState.Minimized

    End Sub
End Class
