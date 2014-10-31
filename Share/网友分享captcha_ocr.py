#-*- coding:utf-8 -*-
import urllib2
import urllib
import cookielib
import time
import socket
import httplib
  
timeout=5
socket.setdefaulttimeout(timeout)
url = 'http://lab.ocrking.com/ok.html'
captcha_url = 'http://210.37.41.198/getImg?t={0}'.format(int(time.time()))
key='ccad8dcb58d213506bBmAxAvWFmHew/+0lB2mSjgb4CeWeO3UMa5PuoGzUz6rxQqFthDZ7olLb'

cj = cookielib.CookieJar()
httpHandler = urllib2.HTTPHandler(debuglevel=1)
cookie_support=urllib2.HTTPCookieProcessor(cj)
opener=urllib2.build_opener(cookie_support,httpHandler)
urllib2.install_opener(opener)

httplib.HTTPConnection._http_vsn = 10
httplib.HTTPConnection._http_vsn_str = 'HTTP/1.0'

headers={  'User-Agent':'curl/7.19.7 (i386-pc-win32) libcurl/7.19.7 OpenSSL/0.9.8q zlib/1.2.3',
           'Content-Type':'application/x-www-form-urlencoded',
           'Referer':'http://lab.ocrking.com',
           'Accept-Encoding':'gzip',
           'Accept-Language':'zh-cn,zh,en',
           'Connection':'Keep-Alive'}

post_data = urllib.urlencode({'url': captcha_url,
                              'service': 'OcrKingForCaptcha',
                              'language':'eng',
                              'charset':'1',
                              'apiKey':key,
                              'type':''}
                            );

request=urllib2.Request(url,headers)

rep = urllib2.urlopen(request,post_data)

html_page=rep.read()
index=html_page.find('<Result>')
captcha_text=html_page[index+8:index+12]
print rep.code
print html_page
print captcha_text
