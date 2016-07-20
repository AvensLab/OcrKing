local http = require("socket.http")
local ltn12 = require("socket.ltn12")

function SendFile(FilePath)
	local files = ""
    local file = io.open(FilePath,"rb")
    if file then
        files = file:read("*a")
        file:close()
    end
    local response_body = {}
		local boundary = "----------------------------831b7f6f6a2d"
		local datatable = {
            "--"..boundary.."\n",
            'Content-Disposition: form-data; name="url"\n\n',
            "",
            "\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="service"\n\n',
            "OcrKingForCaptcha",
            "\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="language"\n\n',
            "eng",
            "\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="charset"\n\n',
            "1",
            "\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="apiKey"\n\n',
            "APIKEY",
			"\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="type"\n\n',
            "http://www.unknown.com",
            "\n--"..boundary.."\n",
            'Content-Disposition: form-data; name="ocrfile"; filename="post.png"\n',
            'Content-Type: image/png\n\n',
            files,
            "\n--"..boundary.."--"
    }
    local data = table.concat( datatable)
    sysLog(data)

    local headers = {
				["Referer"] = "http://lab.ocrking.com",
        ["Accept"]= "*/*",--
        ["Accept-Language"] = "zh-cn,zh,en",----
        ["Content-Type"] = "multipart/form-data; boundary=----------------------------831b7f6f6a2d",------------OcrKing_Client_Aven_s_Lab_L$
        ["Host"] = "lab.ocrking.com" ,--
        ["Content-Length"] = #data,--
        ["Accept-Encoding"] = "gzip, deflate",----
        ["User-Agent"] = "Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0",--
        ["Connection"] = "Keep-Alive",--
        ["Expect"] = "100-continue"--
    }
		
	local rep , code = http.request{
			url = "http://lab.ocrking.com/ok.html",
			method = "POST",
			headers = headers  ,
			source = ltn12.source.string(data),
			sink = ltn12.sink.table(response_body),
	}
	local res = response_body[1]
	return res
end
