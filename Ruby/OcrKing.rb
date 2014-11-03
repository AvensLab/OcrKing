#coding:utf-8

require 'net/http'
require 'uri'
require 'rexml/document'

# you need to modify CAPTCHAURL to original image url to optimize the result
CAPTCHAURL = 'https://www.bestpay.com.cn/api/captcha/getCode?1408294248050'

# OcrKing Api Url #
APIURL = 'http://lab.ocrking.com/ok.html'

# replace the word KEY below with your apiKey obtained by Email #
APIKEY = 'KEY'

# Token used to terminate the file in the post body. Make sure it is not
# present in the file you're uploading.
BOUNDARY = "----------AaB03x"
CRLF = "\r\n"

# get vcode by uploading a local file
def get_vcode_file(filepath)
	
	# you need to modify parameters according to OcrKing Api Document #
	fields = []
	fields << ['url', CAPTCHAURL]
	fields << ['service', 'OcrKingForCaptcha']
	fields << ['language', 'eng']
	fields << ['charset', '4']
	fields << ['apiKey', APIKEY]
	fields << ['type', CAPTCHAURL]

	files = []
	files << ['ocrfile', File.basename(filepath), File.read(filepath,  {mode:'rb'})]

	response = post_multipart(APIURL, fields, files)

	xml = REXML::Document.new(response.body)
	result = xml.get_elements('//Result')[0].text
	status = xml.get_elements('//Status')[0].text

	return status, result
end

### post data with file uploading ###
def post_multipart(url, fields, files)

	uri = URI.parse(url)
	http = Net::HTTP.new(uri.host, uri.port)
	request = Net::HTTP::Post.new(uri.request_uri)
	content_type, body = encode_multipart(fields, files)
	request.body = body

	request["Content-Type"] = content_type
	request["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0"
	#request["Content-Length"] = body.length
	#request["Accept-Encoding"] = 'gzip, deflate'
	#request["Referer"] = 'http://lab.ocrking.com'
	#request["Accept"] = '*/*'
	#request["Connection"] = 'keep-alive'

	return http.request(request)

end

### format the data into multipart/form-data ###	
def encode_multipart(fields, files)

	post_body = []
	fields.each do |key, value|
		post_body << "--#{BOUNDARY}"
		post_body << "Content-Disposition: form-data; name=\"#{key}\""
		post_body << ""
		post_body << value
	end
	files.each do |key, filename, value|
		post_body << "--#{BOUNDARY}"
		post_body << "Content-Disposition: form-data; name=\"#{key}\"; filename=\"#{filename}\""
		post_body << "Content-Type: application/octet-stream"
		post_body << ""
		post_body << value
	end

	post_body << "--#{BOUNDARY}--"
	post_body << ""

	body = post_body.join(CRLF)

	content_type = "multipart/form-data; boundary=#{BOUNDARY}"
	
	return content_type, body
end

# you need to specify the full path of image you wanna OCR #
# e.x. D:\\Program Files\\004.png #
filepath = './pic/1414926666-0.5764729631112064.jpg'

#post and get result
status, result = get_vcode_file(filepath)

#puts result
puts status
puts result






