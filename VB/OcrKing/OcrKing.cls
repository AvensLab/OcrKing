VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "OcrKing"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit
'//////////////////////////////////////////////////////////////////////////////
'@@summary
'@@require
'@@reference
'@@license
'@@author
'@@create
'@@modify
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有声明
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       接口继承
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有常量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有数据类型
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有变量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       公有API
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       事件声明
'------------------------------------------------------------------------------


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有声明
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       私有常量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       私有数据类型
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       私有变量
'------------------------------------------------------------------------------
Private mWinHttpReq As WinHttp.WinHttpRequest '声明一个对象
Private mList As CList
Private mKey As String

'------------------------------------------------------------------------------
'       属性变量
'------------------------------------------------------------------------------


'------------------------------------------------------------------------------
'       私有API
'------------------------------------------------------------------------------


'//////////////////////////////////////////////////////////////////////////////
'//
'//      类
'//
'//////////////////////////////////////////////////////////////////////////////


'------------------------------------------------------------------------------
'       初始化
'------------------------------------------------------------------------------
Private Sub Class_Initialize()
  Set mWinHttpReq = New WinHttp.WinHttpRequest
  Set mList = New CList
End Sub


'------------------------------------------------------------------------------
'       销毁
'------------------------------------------------------------------------------
Private Sub Class_Terminate()
  Set mWinHttpReq = Nothing
  Set mList = Nothing
End Sub


'//////////////////////////////////////////////////////////////////////////////
'//
'//      事件处理
'//
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有属性
'//
'//////////////////////////////////////////////////////////////////////////////


'//////////////////////////////////////////////////////////////////////////////
'//
'//      私有方法
'//
'//////////////////////////////////////////////////////////////////////////////
Private Sub JoinBin(dest() As Byte, source() As Byte)
'合成字节数组
  Dim baseSize As Long, i As Long
  
  If UBound(dest) > 0 Then
    baseSize = UBound(dest) + 1
    ReDim Preserve dest(UBound(dest) + UBound(source) + 1)
  Else
    baseSize = 0
    ReDim dest(UBound(source))
  End If
  For i = 0 To UBound(source)
    dest(baseSize + i) = source(i)
  Next i
End Sub

Private Function BytesToBstr(strBody As Variant, ByVal CodeBase As String)
'编码转换("UTF-8"或者"GB2312"或者"GBK")
  Dim ObjStream
  Set ObjStream = CreateObject("Adodb.Stream")
  With ObjStream
    .Type = 1
    .Mode = 3
    .Open
    .Write strBody
    .position = 0
    .Type = 2
    .Charset = CodeBase
    BytesToBstr = .ReadText
    .Close
  End With
  Set ObjStream = Nothing
End Function

Private Function BinString(ByVal content As String) As Byte()
'获取字符串的字节数组
  Dim source() As Byte, i As Long, countUp As Long, dest() As Byte
  source = content
  countUp = 0
  
  If Len(content) = 0 Then
    Exit Function
  End If
  
  ReDim dest(UBound(source))
  
  For i = 0 To UBound(source)
    If source(i) <> 0 Then
      dest(countUp) = source(i)
      countUp = countUp + 1
    End If
  Next i
  
  ReDim Preserve dest(countUp - 1)
  BinString = dest
End Function

Private Function LoadData(ByVal FilePath As String) As Byte()
'加载文件到字节数组
  Dim Bin() As Byte, FileLength As Long
  Open FilePath For Binary As #1                   '以二进制方式打开文件
    FileLength = LOF(1) ''获取文件长度’该大小以字节为单位
    ReDim Bin(FileLength - 1)                       '用于为动态数组变量重新分配存储空间。
    Get #1, , Bin                                 '将一个已打开的磁盘文件读入一个变量之中。
  Close #1
  LoadData = Bin
End Function

Private Function mGetFileName(ByVal mFilePath As String) As String
'从文件路径中获取文件名
  mFilePath = Replace(mFilePath, "\\", "\")
  mGetFileName = Mid(mFilePath, InStrRev(mFilePath, "\") + 1)
End Function

Private Function mGetCodeFromXml(ByVal xmlCode As String, value As String) As Boolean
  Dim xmlDom As New MSXML2.DOMDocument, xmlNode As IXMLDOMNode
  mGetCodeFromXml = False
  If Not xmlDom.loadXML(xmlCode) Then
    Debug.Print "XML格式错误"
    Exit Function
  End If
  Set xmlNode = xmlDom.selectSingleNode("//Results/ResultList/Item")
  If Not xmlNode Is Nothing Then
    If xmlNode.selectSingleNode("Status").Text = "true" Then
      value = xmlNode.selectSingleNode("Result").Text
      mGetCodeFromXml = True
      Exit Function
    End If
  End If
End Function
'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有属性
'//
'//////////////////////////////////////////////////////////////////////////////
Public Property Get Key() As String
  Key = mKey
End Property

Public Property Let Key(ByVal vNewValue As String)
  mKey = vNewValue
End Property

'//////////////////////////////////////////////////////////////////////////////
'//
'//      公有方法
'//
'//////////////////////////////////////////////////////////////////////////////

Public Function GetCodeFromFile(ByVal FilePath As String) As String
'获取验证码图片的文字
  Dim url As String, boundary As String
  Dim part1 As String, part3 As String, responseXml As String, code As String
  Dim pic() As Byte, bindata() As Byte

  If mKey = "" Then
    Err.Raise 10001, , "[SunSoft]未指定Key，如果没有Key，请联系http://www.ocrking.com申请！"
  End If
  
  url = "http://lab.ocrking.com/ok.html"
  boundary = "------------------------------831b7f6f6a2d"
  ReDim bindata(0)
  
  pic = LoadData(FilePath)

  With mList
    .Clear
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""ocrfile""; filename=""" & mGetFileName(FilePath) & """" & vbCrLf
    .Add "Content-Type: application/octet-stream" & vbCrLf
    .Add "" & vbCrLf
    part1 = .Join("")

    .Clear
    .Add "" & vbCrLf
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""service""" & vbCrLf
    .Add "" & vbCrLf
    .Add "OcrKingForCaptcha" & vbCrLf
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""language""" & vbCrLf
    .Add "" & vbCrLf
    .Add "eng" & vbCrLf
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""type""" & vbCrLf
    .Add "" & vbCrLf
    .Add "http://www.nopreprocess.com" & vbCrLf
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""charset""" & vbCrLf
    .Add "" & vbCrLf
    .Add "7" & vbCrLf
    .Add boundary & vbCrLf
    .Add "Content-Disposition: form-data; name=""apiKey""" & vbCrLf
    .Add "" & vbCrLf
    .Add mKey & vbCrLf
    .Add boundary & "--" & vbCrLf
    .Add "" & vbCrLf
    .Add "" & vbCrLf
    .Add "" & vbCr
    part3 = .Join("")
  End With
  
  JoinBin bindata, BinString(part1)
  JoinBin bindata, pic
  JoinBin bindata, BinString(part3)
  
  mWinHttpReq.Open "POST", url, True
  mWinHttpReq.SetTimeouts 30000, 30000, 30000, 30000
  mWinHttpReq.SetRequestHeader "User-Agent", "Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0"
  mWinHttpReq.SetRequestHeader "Host", "lab.ocrking.com"
  mWinHttpReq.SetRequestHeader "Accept", "*/*"
  mWinHttpReq.SetRequestHeader "Content-Type", "multipart/form-data; boundary=" & Mid(boundary, 3)

  mWinHttpReq.Send bindata        '发送
  mWinHttpReq.WaitForResponse  '异步发送
  responseXml = BytesToBstr(mWinHttpReq.ResponseBody, "UTF-8")
  If mGetCodeFromXml(responseXml, code) Then
    GetCodeFromFile = code
  Else
    GetCodeFromFile = ""
  End If
End Function

