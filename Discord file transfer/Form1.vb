Imports System.Diagnostics.Contracts
Imports System.IO
Imports System.Linq.Expressions
Imports System.Text

Public Class Form1
    Private Structure ConstFileSizes
        Const nonNitro As Integer = 26214399
        Const nitroBasic As Integer = 52428799
        Const nitro As Integer = 524287999
    End Structure
    Dim fileSize As Integer = ConstFileSizes.nonNitro
    Private Sub FromDTP(sender As Object, e As EventArgs) Handles ConvertFromDTP.Click
NoFilesSelected:
        Dim usingDTPInfo As MsgBoxResult = MsgBox("Use DTPInfo file?", MsgBoxStyle.YesNoCancel, "Convert from DTP")
        Dim readPaths() As String
        Select Case usingDTPInfo
            Case Is = MsgBoxResult.Yes
                Dim DTPInfoPath As String
                With OpenFileDialog1
                    .AddExtension = False
                    .Title = "DTP DTPInfo"
                    .ValidateNames = True
                    .CheckFileExists = False
                    .Multiselect = False
                    .ShowDialog()
                    DTPInfoPath = .FileName
                End With
                If DTPInfoPath.Length = 0 Then
                    MsgBox("Please select a DTPInfo file, or all of the .DTP files")
                    GoTo NoFilesSelected
                End If
                Dim DTPInfoData() As String = File.ReadAllLines(DTPInfoPath)
                ReDim readPaths(DTPInfoData.GetUpperBound(0))
                Dim folderLocation As String = Strings.Left(DTPInfoPath, InStrRev(DTPInfoPath, "\"))
                For pathIndex = 0 To DTPInfoData.GetUpperBound(0)
                    readPaths(pathIndex) = folderLocation & DTPInfoData(pathIndex)
                Next
            Case Is = MsgBoxResult.No
                With OpenFileDialog1
                    .AddExtension = False
                    .Title = "DTP files to convert"
                    .ValidateNames = True
                    .CheckFileExists = False
                    .Multiselect = True
                    .ShowDialog()
                    readPaths = .FileNames
                End With
            Case Is = MsgBoxResult.Cancel
                Exit Sub
        End Select
FromDTP:
        Try
            If Not File.Exists(readPaths(0)) Then
                MsgBox("Please select a DTPInfo file, or all of the .DTP files")
                GoTo NoFilesSelected
            End If
        Catch ex As IndexOutOfRangeException
            MsgBox("Please select a DTPInfo file, or all of the .DTP files")
            GoTo NoFilesSelected
        End Try
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
            Dim fileReader As FileStream
            Try
                fileReader = File.OpenRead(path)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Missing file")
                GoTo NoFilesSelected
            End Try
            Dim buffer() As Byte

            If path.Split(".")(path.Split(".").Length - 1)(0) = "n" Then
                ReDim buffer(ConstFileSizes.nitro)
            ElseIf path.Split(".")(path.Split(".").Length - 1)(0) = "b" Then
                ReDim buffer(ConstFileSizes.nitroBasic)
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
                Select Case MsgBox("Files aren't in the correct format, this is likely because they are not .dtp, .bdtp or .ndtp files, e.g. file.txt.dtp1", MsgBoxStyle.RetryCancel, "Files likely incompatible")
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
            Dim writeDTPInfo As FileStream
            Dim writePath As String = ""
            Dim writeName As String = ""
            writePath = readPath
            For pathSection = 0 To writePath.Split("\").Length - 2
                writeName &= writePath.Split("\")(pathSection) & "\"
            Next
            writeName = writeName.Replace(" ", "_")
            readFileName = readFileName.Replace(" ", "_")
            writeDTPInfo = File.Create(writeName & readFileName & ".DTPInfo")
            Do
                Try
                    loopIndex += 1
                    bufferLen = readStream.Read(buffer, 0, fileSize)
                    If 0 <> bufferLen Then
                        Dim writeStream As FileStream
                        If RadioButton1.Checked Then
                            writeStream = File.Create(writeName & readFileName & ".dtp" & loopIndex)
                            writeDTPInfo.Write(ASCIIEncoding.ASCII.GetBytes(readFileName & ".dtp" & loopIndex & vbCrLf), 0, ASCIIEncoding.ASCII.GetBytes(readFileName & ".dtp" & loopIndex).Length + 1)
                        ElseIf RadioButton2.Checked Then
                            writeStream = File.Create(writeName & readFileName & ".bdtp" & loopIndex)
                            writeDTPInfo.Write(ASCIIEncoding.ASCII.GetBytes(readFileName & ".bdtp" & loopIndex & vbCrLf), 0, ASCIIEncoding.ASCII.GetBytes(readFileName & ".bdtp" & loopIndex).Length + 1)
                        Else
                            writeStream = File.Create(writeName & readFileName & ".ndtp" & loopIndex)
                            writeDTPInfo.Write(ASCIIEncoding.ASCII.GetBytes(readFileName & ".ndtp" & loopIndex & vbCrLf), 0, ASCIIEncoding.ASCII.GetBytes(readFileName & ".ndtp" & loopIndex).Length + 1)
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
            writeDTPInfo.Close()
            readStream.Close()
        Else
            If readPath = "OpenFileDialog1" Then
                MsgBox("Sorry, no file was selected.", MsgBoxStyle.Critical, "Issue trying to read file")
            Else
                MsgBox("Sorry, the file""" & readPath & """either does not exist,or is inaccessible.", MsgBoxStyle.Critical, "Issue trying to read file")
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            fileSize = ConstFileSizes.nonNitro
        End If
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            fileSize = ConstFileSizes.nitroBasic
        End If
    End Sub
    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            fileSize = ConstFileSizes.nitro
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MsgBox("Here's a quick guide on how to use my app

First, you need to select your version of discord, either standard, nitro basic, or nitro.
Now, you can click ""To DTP"" which will open a window to select the file (only one, but you can zip multiple together first)
Then, you click ""select file"" and it will create .dtp (or .bdtp or .ndtp) files and a .DTPInfo file which will make converting back far easier.

To convert back, dont worry about the discord version, as that is done automatically, simply click ""From DTP"" and select either the files, or the DTPInfo, depending on if you have one.
DTP will now convert them back to the zip file(or whatever file you converted)")
    End Sub
End Class
