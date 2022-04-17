Imports Newtonsoft
Imports System.IO
Imports Dropbox
Public Class Form1







    Dim Saldox As Int64 = 0

    Private Sub reccorer(filaactual)


        For i As Integer = filaactual + 1 To DataGridView1.Rows.Count() - 1 - 1 Step +1
            DataGridView1.Rows(i).Cells(4).Value = Convert.ToInt32(DataGridView1.Rows(i - 1).Cells(4).Value) - Convert.ToInt32(DataGridView1.Rows(i).Cells("Compras").Value) + Convert.ToInt32(DataGridView1.Rows(i).Cells("Cobranzas").Value)

        Next

        Saldox = DataGridView1.Rows(DataGridView1.Rows.Count() - 2).Cells(4).Value

    End Sub

    Private Sub reccorereliminar(filaactual, resto)


        For i As Integer = filaactual + 1 To DataGridView1.Rows.Count() - 1 - 1 Step +1
            DataGridView1.Rows(i).Cells(4).Value = DataGridView1.Rows(i).Cells(4).Value - resto
        Next



    End Sub
    Private Sub Agregar_Click(sender As Object, e As EventArgs) Handles Agregar.Click
        Try

            Dim Fecha As DateTime = DateTimePicker1.Value
            Dim Descripcion As String = TextBox2.Text
            Dim Compras As Int64 = TextBox3.Text
            Dim Cobranzas As Int64 = TextBox4.Text
            Dim Saldo As Int64 = 0



            Saldo = Saldox - Compras + Cobranzas
            Saldox = Saldo

            DataGridView1.Rows.Add(Fecha.ToString, Descripcion.ToString, Compras.ToString, Cobranzas.ToString, Saldo.ToString)
            TextBox2.Text = ""
            TextBox3.Text = "0"
            TextBox4.Text = "0"

        Catch ex As Exception
            MsgBox("Ingrese los datos de forma correcta porfavor")
            TextBox2.Text = ""
            TextBox3.Text = "0"
            TextBox4.Text = "0"

        End Try
    End Sub
    Private Sub Editar_Click(sender As Object, e As EventArgs) Handles Editar.Click
        Try
            Dim FilaActual = DataGridView1.CurrentRow.Index
            Dim Fecha As DateTime = DateTimePicker1.Value
            Dim Descripcion As String = TextBox2.Text
            Dim Compras As Int64 = TextBox3.Text
            Dim Cobranzas As Int64 = TextBox4.Text
            DataGridView1.Rows(FilaActual).Cells("Fecha").Value = Fecha.ToString
            DataGridView1.Rows(FilaActual).Cells("Descripcion").Value = Descripcion.ToString
            DataGridView1.Rows(FilaActual).Cells("Compras").Value = Compras.ToString
            DataGridView1.Rows(FilaActual).Cells("Cobranzas").Value = Cobranzas.ToString
            Dim Saldo As Int64 = DataGridView1.Rows(FilaActual - 1).Cells("Saldo").Value - Compras + Cobranzas
            DataGridView1.Rows(FilaActual).Cells("Saldo").Value = Saldo.ToString
            Saldox = DataGridView1.Rows(DataGridView1.Rows.Count() - 2).Cells("Saldo").Value
            MsgBox("Modificado exitosamente")


            reccorer(FilaActual)

            TextBox2.Text = ""
            TextBox3.Text = "0"
            TextBox4.Text = "0"
            ActivarBotones()

        Catch ex As Exception
            MsgBox("Ingrese los datos de forma correcta porfavor")
            TextBox2.Text = ""
            TextBox3.Text = "0"
            TextBox4.Text = "0"

        End Try
    End Sub
    Private Sub Eliminar_Click(sender As Object, e As EventArgs) Handles Eliminar.Click
        Dim pregunta As String
        pregunta = MsgBox("Esta seguro que desea eliminar esta fila?", MsgBoxStyle.YesNo, "INFORMACION DEL SISTEMA")
        If pregunta = vbYes Then
            Try
                Dim filActual = DataGridView1.CurrentRow.Index
                reccorereliminar(filActual, DataGridView1.Rows(filActual).Cells("Cobranzas").Value - DataGridView1.Rows(filActual).Cells("Compras").Value)
                DataGridView1.Rows.Remove(DataGridView1.Rows(filActual))
                Saldox = DataGridView1.Rows(DataGridView1.Rows.Count() - 2).Cells("Saldo").Value
                MsgBox("ELIMINADO exitosamente")
            Catch ex As Exception
                MsgBox("No se puede eliminar la ultima fila")
            End Try

            ActivarBotones()
            TextBox2.Text = ""
            TextBox3.Text = "0"
            TextBox4.Text = "0"
        End If
    End Sub
    Private Sub ActivarBotones()

        Agregar.Enabled = True
        Guardar.Enabled = True
        Editar.Enabled = False
        Eliminar.Enabled = False
    End Sub
    Private Sub DesactivarBotones()
        Agregar.Enabled = False
        Guardar.Enabled = False
        Editar.Enabled = True
        Eliminar.Enabled = True
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Dim filaActual = DataGridView1.CurrentRow.Index
        TextBox2.Text = DataGridView1.Rows(filaActual).Cells("Descripcion").Value
        TextBox3.Text = DataGridView1.Rows(filaActual).Cells("Compras").Value
        TextBox4.Text = DataGridView1.Rows(filaActual).Cells("Cobranzas").Value

        DesactivarBotones()
    End Sub
    Private Sub Leerdata(pathfile As String)
        Dim fileName As String = pathfile




        Dim fi As New IO.FileInfo(fileName)
        Try
            Dim archivo_leer As StreamReader
            archivo_leer = New StreamReader(pathfile)
            Dim stringToCleanUp As String = fi.Name
            Dim characterToRemove As String = ".txt"
            Dim cleanString As String = Replace(stringToCleanUp, characterToRemove, "")

            While Not archivo_leer.EndOfStream
                Dim cadena As String = archivo_leer.ReadLine
                Dim leer As String() = cadena.Split(New Char() {";"})
                DataGridView1.Rows.Add(leer)
            End While
            DataGridView1.Rows.Remove(DataGridView1.Rows(DataGridView1.Rows.Count() - 2))
            Saldox = Convert.ToInt64(DataGridView1.Rows(DataGridView1.Rows.Count() - 1 - 1).Cells(4).Value)
            archivo_leer.Close()
        Catch ex As Exception
            MsgBox("No se pudo encontrar su archivo en la carpeta")
            Saldox = 0
        End Try


    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox3.Text = "0"
        TextBox4.Text = "0"
        OpenFileOnStart()

    End Sub

    Private Sub ChoosePathFile()
        Dim objFolderDlg As System.Windows.Forms.FolderBrowserDialog
        objFolderDlg = New System.Windows.Forms.FolderBrowserDialog
        objFolderDlg.SelectedPath = "C:\Test"
        If objFolderDlg.ShowDialog() = DialogResult.OK Then


            GuardarDatos(objFolderDlg.SelectedPath)


        End If


    End Sub
    Private Sub OpenFileOnStart()
        'Dim objFolderDlg As System.Windows.Forms.FolderBrowserDialog
        'objFolderDlg = New System.Windows.Forms.FolderBrowserDialog
        'objFolderDlg.SelectedPath = "C:\Test"

        'If objFolderDlg.ShowDialog() = DialogResult.OK Then
        '    DataGridView1.Rows.Clear()

        '    Leerdata(objFolderDlg.SelectedPath)
        'End If
        Using openFileDialog1 As OpenFileDialog = New OpenFileDialog()
            If openFileDialog1.ShowDialog() = DialogResult.OK Then
                DataGridView1.Rows.Clear()
                Leerdata(openFileDialog1.FileName)
            End If
        End Using



    End Sub

    Private Sub GuardarDatos(path As String)
        Dim archivo_escritura As StreamWriter
        Dim linea As String
        archivo_escritura = New StreamWriter(path + "\" + "OrionContabilidad" + ".txt")


        With DataGridView1
            For y = 0 To DataGridView1.RowCount - 1
                linea = .Rows(y).Cells("Fecha").Value & ";" &
                      .Rows(y).Cells("Descripcion").Value & ";" &
                         .Rows(y).Cells("Compras").Value & ";" &
                          .Rows(y).Cells("Cobranzas").Value & ";" &
                          .Rows(y).Cells("Saldo").Value & ";"

                archivo_escritura.WriteLine(linea)
            Next
            MsgBox("Datos guardados exitosamente")
        End With

        archivo_escritura.Close()



    End Sub
    Private Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click
        ChoosePathFile()
    End Sub


    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim dialog As DialogResult
        dialog = MessageBox.Show("Guardar antes de salir?", "Orionsoft", MessageBoxButtons.YesNoCancel)
        If dialog = DialogResult.Yes Then
            ChoosePathFile()
        ElseIf dialog = DialogResult.No Then
            Application.Exit()
        ElseIf dialog = DialogResult.Cancel Then
            e.Cancel = True
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub ElegirCaminoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ElegirCaminoToolStripMenuItem.Click
        ChoosePathFile()
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub MenuStrip2_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip2.ItemClicked

    End Sub
End Class
