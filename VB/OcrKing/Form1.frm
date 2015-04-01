VERSION 5.00
Object = "{248DD890-BB45-11CF-9ABC-0080C7E7B78D}#1.0#0"; "MSWINSCK.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3930
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   6765
   LinkTopic       =   "Form1"
   ScaleHeight     =   3930
   ScaleWidth      =   6765
   StartUpPosition =   3  '窗口缺省
   Begin MSWinsockLib.Winsock Winsock2 
      Left            =   720
      Top             =   1440
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
   End
   Begin VB.CommandButton Command2 
      Caption         =   "网址方式"
      Height          =   615
      Left            =   3840
      TabIndex        =   1
      Top             =   1200
      Width           =   1815
   End
   Begin MSWinsockLib.Winsock Winsock1 
      Left            =   840
      Top             =   840
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
   End
   Begin VB.CommandButton Command1 
      Caption         =   "上传图片方式"
      Height          =   615
      Left            =   3840
      TabIndex        =   0
      Top             =   600
      Width           =   1815
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim B() As Byte

Dim Log As String
Dim Result As String
Dim Url As String
Dim Key As String



Function UploadOck(D() As Byte)
'B = D

UploadOck = UploadOck & "POST /ok.html HTTP/1.1" & vbCrLf
UploadOck = UploadOck & "User-Agent: Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0" & vbCrLf
UploadOck = UploadOck & "Host: lab.ocrking.com" & vbCrLf
UploadOck = UploadOck & "Accept: */*" & vbCrLf
UploadOck = UploadOck & "Content-Length: " & Len(UploadOck1) + Len(UploadOck2) + UBound(D) & vbCrLf
'UploadOck = UploadOck & "Expect: 100-continue" & vbCrLf
UploadOck = UploadOck & "Content-Type: multipart/form-data; boundary=----------------------------831b7f6f6a2d" & vbCrLf
UploadOck = UploadOck & "" & vbCrLf
UploadOck = UploadOck & UploadOck1()
'UploadOck = UploadOck & "Expect: 100-continue" & vbCrLf
End Function

Function UploadOck1()

UploadOck1 = UploadOck1 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck1 = UploadOck1 & "Content-Disposition: form-data; name=""ocrfile""; filename=""3.png""" & vbCrLf
UploadOck1 = UploadOck1 & "Content-Type: application/octet-stream" & vbCrLf
UploadOck1 = UploadOck1 & "" & vbCrLf


End Function

Function UploadOck2()

UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck2 = UploadOck2 & "Content-Disposition: form-data; name=""service""" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "OcrKingForCaptcha" & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck2 = UploadOck2 & "Content-Disposition: form-data; name=""language""" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "eng" & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck2 = UploadOck2 & "Content-Disposition: form-data; name=""type""" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & Url & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck2 = UploadOck2 & "Content-Disposition: form-data; name=""charset""" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "7" & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d" & vbCrLf
UploadOck2 = UploadOck2 & "Content-Disposition: form-data; name=""apiKey""" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & Key & vbCrLf
UploadOck2 = UploadOck2 & "------------------------------831b7f6f6a2d--" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf
UploadOck2 = UploadOck2 & "" & vbCrLf



End Function



Function Getdata()

Getdata = Result

End Function







Private Sub Command2_Click()

Log = UrlOcrking("http://", "key") '请把网址换成你想识别图片的连接，key换成你的apikey

'//本识别服务为免费，按格式发封邮件就可以获取apiKey
'//邮件格式请进群查看①: 296228446
'//                  ②: 296228429
'//授权apiKey,请注意区分大小写


Winsock2.Close
Winsock2.RemoteHost = "lab.ocrking.com"
Winsock2.RemotePort = 80
Winsock2.Connect

End Sub


Function UrlOcrking(ByVal Urls As String, ByVal Keys As String)
Dim D As String
D = "url=" & Urls & "&service=OcrKingForCaptcha&language=eng&charset=7&apiKey=" & Keys & "&type=" & Urls

UrlOcrking = UrlOcrking & "POST /ok.html HTTP/1.1" & vbCrLf
UrlOcrking = UrlOcrking & "User-Agent: Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0" & vbCrLf
UrlOcrking = UrlOcrking & "Host: lab.ocrking.com" & vbCrLf
UrlOcrking = UrlOcrking & "Accept: */*" & vbCrLf
UrlOcrking = UrlOcrking & "Content-Length: " & Len(D) & vbCrLf
UrlOcrking = UrlOcrking & "Content-Type: application/x-www-form-urlencoded" & vbCrLf
UrlOcrking = UrlOcrking & "Connection: Keep-Alive" & vbCrLf
UrlOcrking = UrlOcrking & "" & vbCrLf
UrlOcrking = UrlOcrking & D & vbCrLf
UrlOcrking = UrlOcrking & "" & vbCrLf
UrlOcrking = UrlOcrking & "" & vbCrLf


End Function







Private Sub Winsock1_Connect()
Winsock1.SendData UploadOck(B)
Winsock1.SendData B()
Winsock1.SendData UploadOck2()


End Sub

Private Sub Winsock1_DataArrival(ByVal bytesTotal As Long)
Dim DataRev As String
Dim aa, bb As Long


Winsock1.Getdata DataRev


aa = InStr(1, DataRev, "<Result>")
If aa <> 0 Then
bb = InStr(aa, DataRev, "</Result>")
 If bb <> 0 Then
Result = Mid(DataRev, aa + 8, bb - aa - 8)


 Else
 Result = "worng...."

End If
Else
Result = "worng!!!"
End If

Winsock1.Close

MsgBox Result
End Sub

Private Sub Command1_Click()
Key = "key"
Url = "http://"
'请把网址换成你想识别图片的连接，key换成你的apikey

'//本识别服务为免费，按格式发封邮件就可以获取apiKey
'//邮件格式请进群①: [296228446](http://shang.qq.com/wpa/qunwpa?idkey=8baf8f5b24d0a19b08a3a18fb5b2600c48fcde2abecf3528376a04059a72e3a6) 查看
'//                  ②: 296228429
'//授权apiKey,请注意区分大小写



Paths = App.Path & "\code.png"



'Dim I As Long
Open Paths For Binary As #1                   '以二进制方式打开文件
I = LOF(1) ''获取文件长度’该大小以字节为单位
ReDim B(I - 1)                       '用于为动态数组变量重新分配存储空间。
Get #1, , B                                 '将一个已打开的磁盘文件读入一个变量之中。
Close #1

'Log = UploadOck(B())


Winsock1.Close
Winsock1.RemoteHost = "lab.ocrking.com"
Winsock1.RemotePort = 80
Winsock1.Connect


End Sub

Private Sub Winsock2_Connect()
Winsock2.SendData Log

End Sub

Private Sub Winsock2_DataArrival(ByVal bytesTotal As Long)
Dim DataRev As String
Dim aa, bb As Long

Winsock2.Getdata DataRev

aa = InStr(1, DataRev, "<Result>")
If aa <> 0 Then
bb = InStr(aa, DataRev, "</Result>")
 If bb <> 0 Then
Result = Mid(DataRev, aa + 8, bb - aa - 8)


 Else
 Result = "worng...."

End If
Else
Result = DataRev
End If

Winsock1.Close

MsgBox Result



End Sub

