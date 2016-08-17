[General]
SyntaxVersion=2
MacroID=2811fb4a-9f42-41fc-80f3-022282e5706b
[Comment]

[Script]
//分享作者:红妆
//QQ:394053565
//关于type  如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的
//http://www.unknown.com  后台只进行常用预处理
//http://www.nopreprocess.com  禁止后台做任何预处理
//http://user.56.com/api/get_auth.php?t=56_reg&rnd=0.5675823935307562 随便给个验证地址做预处理,否则报错
//语言="eng"   英文=eng  简体=sim  繁体=tra
//验证码类型=7  所有英文字符=0 所有纯数字=1 小写英文字母=2 大写英文字母=3 数字小写字母=4 数字大写字母=5 大写小写字母=6 数字大写小写=7 常用英文字符=8 网址和邮件类=9 $￥商城价格=10 手机电话号类=11 数学公式计算=12
//-----示例----------
//key="邮件获取的key"
//接口="http://lab.ocrking.com/ok.html"
//答案 =OCRKING("C:/1.png",7,"eng",key,接口)
//本识别服务为免费，唯一获取 KEY 的途径->邮箱申请  发邮件到 ok@ocrking.com 获取key
//----------邮件格式---------------------
//	以 key 或 apiKey 为邮件标题(2选1即可) 
//	邮件内容 以下内容仅供参考，请按实际情况修改
//	用途：文档电子化，自动登录，扫二维码.....
//	环境：PC，APP，WEB
//	工具或语言: eclipse,VS,ZS c++,c#,java,php,python,js .....
//	类型：个人，公司，开源，免费
//	人群数量：0-10,10-50,50-100,100+ .....
//---------------------------------------
//虚构编造或不符合要求的不会回复
//无意思的乱输入会进入垃圾箱
//可能会有延迟，请匆多次重复发送，
//多次发送系统可能会判断为垃圾信件
//可能会有延迟，请匆重复发送
//授权apiKey,请注意区分大小写
//网站主页 http://lab.ocrking.com/
//网站主页加QQ交流群,获取例子模板
Function OCRKING(路径,验证码类型,语言,key,接口)
    Dim xmlBody,BytesToBstr,strBoundary
    strBoundary=Class_Initialize()
    Call AddForm("service", "OcrKingForCaptcha",strBoundary)
    Call AddForm("language",语言,strBoundary)
    Call AddForm("charset",验证码类型,strBoundary)
    Call AddForm("type","http://www.nopreprocess.com",strBoundary)
    Call AddForm("apiKey",key,strBoundary)
    Call AddFile("ocrfile", "www.baidu.com", "image/jpg", 路径,strBoundary)
    xmlBody=Upload(接口,strBoundary)
    Set ObjStream = CreateObject("Adodb.Stream")
    With ObjStream
        .Type = 1
        .Mode = 3
        .Open
        .Write xmlBody
        .Position = 0
        .Type = 2
        .Charset = "utf-8"
        BytesToBstr = .ReadText
        .Close
    End With
    BytesToBstr=mid(BytesToBstr,instr(BytesToBstr,"<Result>")+len("<Result>"))
    BytesToBstr=left(BytesToBstr,instr(BytesToBstr,"<")-1)
    If InStr(BytesToBstr, "utf-8") > 0 Then 
        OCRKING=""
    Else
        OCRKING=BytesToBstr
    End If
End Function
Function StringToBytes(ByVal strData, ByVal strCharset)
    Dim objFile
    Set objFile = CreateObject("ADODB.Stream")
    objFile.Type = 2
    objFile.Charset = strCharset
    objFile.Open
    objFile.WriteText strData
    objFile.Position = 0
    objFile.Type = 1
    If UCase(strCharset) = "UNICODE" Then
        objFile.Position = 2
    ElseIf UCase(strCharset) = "UTF-8" Then
        objFile.Position = 3
    End If
    StringToBytes = objFile.Read(-1)
    objFile.Close
    Set objFile = Nothing
End Function
Function GetFileBinary(ByVal strPath)
    Dim objFile
    Set objFile = CreateObject("ADODB.Stream")
    objFile.Type = 1
    objFile.Open
    objFile.LoadFromFile strPath
    GetFileBinary = objFile.Read(-1)
    objFile.Close
    Set objFile = Nothing
End Function
Function GetBoundary()
    Dim ret(12)
    Dim table
    Dim i
    table = "abcdefghijklmnopqrstuvwxzy0123456789"
    Randomize
    For i = 0 To UBound(ret)
        ret(i) = Mid(table, Int(Rnd() * Len(table) + 1), 1)
    Next
    GetBoundary = "---------------------------" & Join(ret, Empty)
End Function
Sub AddForm(ByVal strName, ByVal strValue,ByVal strBoundary)
    Dim tmp
    tmp = "\r\n--$1\r\nContent-Disposition: form-data; name=""$2""\r\n\r\n$3"
    tmp = Replace(tmp, "\r\n", vbCrLf)
    tmp = Replace(tmp, "$1", strBoundary)
    tmp = Replace(tmp, "$2", strName)
    tmp = Replace(tmp, "$3", strValue)
    objTemp.Write StringToBytes(tmp, "UTF-8")
End Sub
Sub AddEnd(ByVal strBoundary)
    Dim tmp
    tmp = "\r\n--$1--\r\n" 
    tmp = Replace(tmp, "\r\n", vbCrLf) 
    tmp = Replace(tmp, "$1", strBoundary)
    objTemp.Write StringToBytes(tmp, "UTF-8")
    objTemp.Position = 2
End Sub
Function Upload(ByVal strURL,ByVal strBoundary)
    Call AddEnd(strBoundary)
    xmlHttp.Open "POST", strURL, False
    xmlHttp.setRequestHeader "Content-Type", "multipart/form-data; boundary=" & strBoundary
    xmlHttp.setRequestHeader "Content-Length", objTemp.size
    xmlHttp.setRequestHeader "Host", "lab.ocrking.com"
    xmlHttp.setRequestHeader "Expect", "100-continue"
    xmlHttp.Send objTemp
    Upload = xmlHttp.ResponseBody
End Function
Sub AddFile(ByVal strName, ByVal strFileName, ByVal strFileType, ByVal strFilePath,ByVal strBoundary)
    Dim tmp
    tmp = "\r\n--$1\r\nContent-Disposition: form-data; name=""$2""; filename=""$3""\r\nContent-Type: $4\r\n\r\n"
    tmp = Replace(tmp, "\r\n", vbCrLf)
    tmp = Replace(tmp, "$1", strBoundary)
    tmp = Replace(tmp, "$2", strName)
    tmp = Replace(tmp, "$3", strFileName)
    tmp = Replace(tmp, "$4", strFileType)
    objTemp.Write StringToBytes(tmp, "UTF-8")
    objTemp.Write GetFileBinary(strFilePath)
End Sub
Function Class_Initialize()
    Set xmlHttp = CreateObject("Msxml2.XMLHTTP")
    Set objTemp = CreateObject("ADODB.Stream")
    objTemp.Type =1
    objTemp.Open
    Class_Initialize=GetBoundary()
End Function

