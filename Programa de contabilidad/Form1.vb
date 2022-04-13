
Imports System.ComponentModel
Imports System.IO
Imports System.Data.SqlClient
Public Class Form1

    Private Sub Agregar_Click(sender As Object, e As EventArgs) Handles Agregar.Click
        Try
            Dim Fecha As DateTime = DateTimePicker1.Value
            Dim Descripcion As String = TextBox2.Text
            Dim Compras As Int64 = TextBox3.Text
            Dim Cobranzas As Int64 = TextBox4.Text
            Dim Saldo As Int64 = Saldo - Compras + Cobranzas
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
            Dim Saldo As Int64 = Saldo - Compras + Cobranzas
            DataGridView1.Rows(FilaActual).Cells("Saldo").Value = Saldo.ToString
            MsgBox("Modificado exitosamente")

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
            Dim filActual = DataGridView1.CurrentRow.Index
            DataGridView1.Rows.Remove(DataGridView1.Rows(filActual))
            MsgBox("ELIMINADO exitosamente")
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
        DateTimePicker1.Value = DataGridView1.Rows(filaActual).Cells("Fecha").Value
        TextBox2.Text = DataGridView1.Rows(filaActual).Cells("Descripcion").Value
        TextBox3.Text = DataGridView1.Rows(filaActual).Cells("Compras").Value
        TextBox4.Text = DataGridView1.Rows(filaActual).Cells("Cobranzas").Value
        DesactivarBotones()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TextBox3.Text = "0"
        TextBox4.Text = "0"
        Dim archivo_leer As StreamReader
        archivo_leer = New StreamReader("./datos.txt")
        While Not archivo_leer.EndOfStream
            Dim cadena As String = archivo_leer.ReadLine
            Dim leer As String() = cadena.Split(New Char() {";"})
            DataGridView1.Rows.Add(leer)
        End While
        archivo_leer.Close()
    End Sub

    Private Sub GuardarDatos()
        Dim archivo_escritura As StreamWriter
        Dim linea As String
        archivo_escritura = New StreamWriter("./datos.txt")
        With DataGridView1
            For y = 0 To DataGridView1.RowCount - 1
                linea = .Rows(y).Cells("Fecha").Value & ";" &
                      .Rows(y).Cells("Descripcion").Value & ";" &
                         .Rows(y).Cells("Compras").Value & ";" &
                          .Rows(y).Cells("Cobranzas").Value & ";"
                archivo_escritura.WriteLine(linea)
            Next
            MsgBox("Datos guardados exitosamente")
        End With

        archivo_escritura.Close()



    End Sub
    Private Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click
        GuardarDatos()
    End Sub


    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim dialog As DialogResult
        dialog = MessageBox.Show("Guardar antes de salir?", "Orionsoft", MessageBoxButtons.YesNoCancel)
        If dialog = DialogResult.Yes Then
            GuardarDatos()
        ElseIf dialog = DialogResult.No Then
            Application.Exit()
        ElseIf dialog = DialogResult.Cancel Then
            e.Cancel = True
        End If
    End Sub
End Class
