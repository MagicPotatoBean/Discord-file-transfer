Imports System.Diagnostics.Contracts
Imports System.IO
Public Class Form1
    Private Structure ConstFileSizes
        Const nonNitro As Integer = 8388607
        Const nitro As Integer = 52428799
    End Structure
    Dim fileSize As Integer = ConstFileSizes.nonNitro
    Private Sub FromDTP(sender As Object, e As EventArgs) Handles ConvertFromDTP.Click


FromDTP:
        Dim readPaths() As String
        With OpenFileDialog1
            .AddExtension = False
            .Title = "DTP files to convert"
            .ValidateNames = True
            .CheckFileExists = False
            .Multiselect = True
            .ShowDialog()
            readPaths = .FileNames
        End With
        If sort(readPaths) Then
            GoTo FromDTP
        End If
        If String.IsNullOrEmpty(readPaths(0)) Then
            Application.Exit()
        End If
        Dim msgBoxChoice As MsgBoxResult
        Dim firstPath As String = ""
        For pathSection = 0 To readPaths(0).Split(".").Length - 2
            firstPath &= readPaths(0).Split(".")(pathSection) & "."
        Next
        firstPath = Strings.Left(firstPath, firstPath.Length - 1)
        Dim filePath As String = ""
        For Each file In readPaths
            filePath = ""
            For pathSection = 0 To file.Split(".").Length - 2
                filePath &= file.Split(".")(pathSection) & "."
            Next
            filePath = Strings.Left(filePath, filePath.Length - 1)

            If firstPath <> filePath Then
                msgBoxChoice = MsgBox("Files do not have the same name, so will likely cause an error when recreating the original file.", MsgBoxStyle.AbortRetryIgnore, "Files likely to form corrupt output")
                Select Case msgBoxChoice
                    Case Is = vbAbort
                        Application.Exit()
                    Case Is = vbRetry
                        GoTo FromDTP
                    Case Is = vbIgnore
                        Exit For
                End Select
            End If
        Next
        Dim fileWriter As FileStream = File.Create(firstPath)
        For Each path In readPaths
            Dim fileReader As FileStream = File.OpenRead(path)
            Dim buffer() As Byte

            If path.Split(".")(path.Split(".").Length - 1)(0) = "n" Then
                ReDim buffer(ConstFileSizes.nitro)
            Else
                ReDim buffer(ConstFileSizes.nonNitro)
            End If

            Dim bufferLen As UInteger
            bufferLen = fileReader.Read(buffer, 0, buffer.Length - 1)
            fileWriter.Write(buffer, 0, bufferLen)
            fileReader.Close()
        Next
        fileWriter.Close()

    End Sub
    Private Function sort(ByRef Arr() As String) As Boolean

        Dim returnArr(Arr.Length - 1) As String
        For Each path In Arr
            Try
                returnArr(Integer.Parse(path.Split("p")(path.Split("p").Length - 1)) - 1) = path
            Catch ex As Exception
                Select Case MsgBox("Files aren't in the correct format, this is likely because they are not .dtp files, e.g. file.txt.dtp1", MsgBoxStyle.RetryCancel, "Files likely incompatible")
                    Case Is = vbCancel
                        Dim emptyString(0) As String
                        Arr = emptyString
                        Return False
                    Case Is = vbRetry
                        Return True
                End Select
            End Try
        Next
        Arr = returnArr
    End Function

    Private Sub ToDTP(sender As Object, e As EventArgs) Handles ConvertToDTP.Click
ToDTP:
        Dim readPath As String = ""
        With OpenFileDialog1
            .AddExtension = False
            .Title = "Files to convert to DTP"
            .ValidateNames = True
            .CheckFileExists = True
            .ShowDialog()
            readPath = .FileName
        End With
        If File.Exists(readPath) Then
            Dim readStream As FileStream = File.OpenRead(readPath)
            Dim loopIndex As UInteger = 0
            Dim readFileName As String = readPath.Split("\")(readPath.Split("\").Length - 1)
            Dim buffer(fileSize) As Byte
            Dim bufferLen As UInteger = 0
            Dim writePath As String = ""
            Dim writeName As String = ""
            writePath = readPath
            For pathSection = 0 To writePath.Split("\").Length - 2
                writeName &= writePath.Split("\")(pathSection) & "\"
            Next
            Do
                Try
                    loopIndex += 1
                    bufferLen = readStream.Read(buffer, 0, fileSize)
                    If 0 <> bufferLen Then
                        Dim writeStream As FileStream
                        If CheckBox1.Checked Then
                            writeStream = File.Create(writeName & readFileName & ".ndtp" & loopIndex)
                        Else
                            writeStream = File.Create(writeName & readFileName & ".dtp" & loopIndex)
                        End If

                        writeStream.Write(buffer, 0, bufferLen)
                        writeStream.Close()
                    Else
                        Exit Do
                    End If
                Catch ex As Exception
                    Select Case MsgBox("Sorry, the converted files could not be saved in this directory, try moving the file to be converted to another directory.", MsgBoxStyle.AbortRetryIgnore, "Issue trying to write to file")
                        Case Is = vbAbort
                            Application.Exit()
                        Case Is = vbRetry
                            GoTo ToDTP
                        Case Is = vbIgnore
                            Exit Try
                    End Select

                End Try
            Loop
            readStream.Close()
        Else
            If readPath = "OpenFileDialog1" Then
                MsgBox("Sorry, no file was selected.", MsgBoxStyle.Critical, "Issue trying to read file")
            Else
                MsgBox("Sorry, the file""" & readPath & """either does not exist,or is inaccessible.", MsgBoxStyle.Critical, "Issue trying to read file")
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            fileSize = ConstFileSizes.nitro
        Else
            fileSize = ConstFileSizes.nonNitro
        End If
    End Sub
End Class
