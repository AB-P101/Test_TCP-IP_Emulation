Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class Form1

    Private mSocket As Socket = Nothing
    'Private IP As String = txt_host.Text
    'Private Port As UShort = txt_port.Text

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Connect()


        Me.mSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        Me.mSocket.ReceiveBufferSize = 256
        Me.mSocket.SendBufferSize = 256
        Dim server As IPEndPoint = New IPEndPoint(IPAddress.Parse(txt_host.Text), txt_port.Text)
        Me.mSocket.Connect(server)
    End Sub

    Private Sub Disconnect()
        If Not IsDBNull(Me.mSocket) Then
            Me.mSocket.Close()
        End If
    End Sub


    Private Function WriteSingleCoil(ByVal id As UShort, ByVal slaveAddress As Byte, ByVal startAddress As UShort, ByVal functionCode As Byte, ByVal data As UShort) As Byte()
        Dim frame As Byte() = New Byte(11) {} ' Total 12 Bytes.

        'Convert ushort to byte aray
        Dim byteArray As Byte() = BitConverter.GetBytes(data)

        frame(0) = CByte(id / 256) ' Transaction Identifier High. 
        frame(1) = CByte(id Mod 256) ' Transaction Identifier Low.
        frame(2) = 0 ' Protocol Identifier High.
        frame(3) = 0 ' Protocol Identifier Low.
        frame(4) = 0 ' Message Length High.
        frame(5) = 6 ' Message Length Low(6 bytes to follow).
        frame(6) = 1 ' The Unit Identifier.
        frame(7) = 6 ' Function.
        frame(8) = CByte(startAddress / 256) ' Starting Address High.
        frame(9) = CByte(startAddress Mod 256) ' Starting Address Low.
        frame(10) = byteArray(1) ' Write Data High.
        frame(11) = byteArray(0) ' Write Data Low.
        Return frame
    End Function

    Private Function Write(ByVal frame As Byte()) As Integer
        Return Me.mSocket.Send(frame, frame.Length, SocketFlags.None)
    End Function

    Private Sub btn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1.Click
        Try
            Dim slaveAddress As Byte = 1 'The Unit Identifier.
            Dim functionCode As Byte = 6 ' Function.
            Dim id As UShort = functionCode ' Transaction Identifier.
            Dim startAddress As UShort = txt_add.Text ' Starting Address.
            Dim data As UShort = UShort.Parse(TextBox1.Text) ' Write Data.
            Dim frame As Byte() = Me.WriteSingleCoil(id, slaveAddress, startAddress, functionCode, data)
            Me.Write(frame) ' Write frame to device.
            Label8.Text = "succeed."
            Thread.Sleep(100)
        Catch ex As Exception
            Label8.Text = "no succeed."
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Me.Connect()
            Label4.Text = "connect."
        Catch ex As Exception
            Label4.Text = "no connect."
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
End Class


