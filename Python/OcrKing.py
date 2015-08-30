####
# Copyright (c) 2009 - 2015, Aven's Lab. All rights reserved.
#                     http://www.ocrking.com
# DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
# Id: OcrKing.py,v 0.1 2014/10/29 23:40:18 
# The Python script for OcrKing Api
# By Aven <Aven@OcrKing.Com>
# Welcome to follow us 
# http://weibo.com/OcrKing
# http://t.qq.com/OcrKing
# Warning! 
# Before running this script , you should modify some parameter
# within the post data according to what you wanna do
# To run it manually,just type  python OcrKing.py
# the result will be shown soon
# Just Enjoy The Fun Of OcrKing !
####

# version for Python OcrKing Client #
__version__ = '0.1'
# version for Python OcrKing Client #

# OcrKing Api Url #
__api_url__ = 'http://lab.ocrking.com/ok.html'
# OcrKing Api Url #

### libs need to import ###
import urllib2
import httplib
### libs need to import ###

### just fix some known bug of Python , ignore this please ###
httplib.HTTPConnection._http_vsn = 10
httplib.HTTPConnection._http_vsn_str = 'HTTP/1.0'
### just fix some known bug of Python , ignore this please ###

### post data with file uploading ###
def post_multipart(fields, files):
	content_type, body = encode_multipart(fields, files)
	headers = {'Content-Type': content_type,
	'Content-Length': str(len(body)),
	'Accept-Encoding': 'gzip, deflate',
	'Referer': 'http://lab.ocrking.com/?pyclient' + __version__,
	'Accept':'*/*'}
	r = urllib2.Request(__api_url__, body, headers)
	return urllib2.urlopen(r).read()

### format the data into multipart/form-data ###	
def encode_multipart(fields, files):
	LIMIT = '----------OcrKing_Client_Aven_s_Lab_L$'
	CRLF = '\r\n'
	L = []
	for (key, value) in fields:
		L.append('--' + LIMIT)
		L.append('Content-Disposition: form-data; name="%s"' % key)
		L.append('')
		L.append(value)
	for (key, filename, value) in files:
		L.append('--' + LIMIT)
		L.append('Content-Disposition: form-data; name="%s"; filename="%s"' % (key, filename))
		L.append('Content-Type: application/octet-stream' )
		L.append('')
		L.append(value)
	L.append('--' + LIMIT + '--')
	L.append('')
	body = CRLF.join(L)
	content_type = 'multipart/form-data; boundary=%s' % LIMIT
	return content_type, body


# replace the word KEY below with your apiKey obtained by Email #	
key = 'KEY'
# you need to specify the full path of image you wanna OCR #
# e.x. D:\\Program Files\\004.png #
image = open('D:\\Program Files\\IronPython-2.7.3\\111.png', "rb") 
# you need to modify the filename according to you real filename #
# e.x 004.png #
file = [('ocrfile','004.png',image.read())]
# you need to modify parameters according to OcrKing Api Document #
# 如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的 #
# 如果想禁止后台做任何预处理  type可以设为 http://www.nopreprocess.com #
# 如果确实不确定验证码图片的原url  type可以设为 http://www.unknown.com  此时后台只进行常用预处理 #
# 此demo中type值只针对此demo中的图片，其它网站图片请不要用此值 #
fields = [('url',''),('service', 'OcrKingForCaptcha'),('language','eng'),('charset','7'),('apiKey', key),('type','https://www.bestpay.com.cn/api/captcha/getCode?1408294248050')]
# just fire the post action #
xml = post_multipart(fields,file)
# print the result #
print xml