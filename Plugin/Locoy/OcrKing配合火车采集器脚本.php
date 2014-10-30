<?php
/*
*########     [Aven's Lab] (C) 2014 Aven's Lab     ########
*########      File:        GBK OcrKing.php                 ########
*########     Author:   真视觉   Email: ts@OcrKing.CoM     ########
*########     WebSite:    http://WwW.OcrKing.CoM                ########
*########     注意：此API为临时,新版本组件发布时会停用！！！     ########
*########     注意：使用此脚本需要开启php curl扩展！     ########
*/

/*************************以下内容请根据需要修改***************************/
//屏蔽错误
error_reporting ( E_ALL || ~ E_NOTICE );

//本识别服务为免费，没有apiKey可以 key为标题
// 任意内容为正文发邮件到 ok@ocrking.com获取
//可能会有延迟，请匆重复发送
//授权apiKey,请注意此值区分大小写
//请用邮件回复内容中的内容替换下面的 key
define ( 'API_KEY', 'key' );

/*************************以上内容请根据需要修改***************************/


// service 识别类型可以为以下value中的值 注意此值区分大小写
//<option value="OcrKingForPassages">长篇内容</option>
//<option value="OcrKingForPDF">PDF识别</option>
//<option value="OcrKingForPhoneNumber">手机电话</option>
//<option value="OcrKingForPrice">商城价格</option>
//<option value="OcrKingForNumber">纯数字类</option>
//<option value="OcrKingForCaptcha">验证码类</option>
//<option value="BarcodeDecode">条形二维码</option>
//<option value="PDFToImage">PDF转图片</option>


//language 识别语种可以为以下值  eng(英文)   sim(简体) tra(繁体)
//<option value="eng">英 语</option>												
//<option value="sim">简 体</option>
//<option value="tra">繁 体</option>


// charset 可以为以下选项 ,仅当service为 OcrKingForCaptcha时 且 language 为 eng有效，其它情况下无效
//<option value=""></option>
//<option value="0">所有英文字符</option>
//<option value="1">所有纯数字</option>
//<option value="2">小写英文字母</option>
//<option value="3">大写英文字母</option>
//<option value="4">数字小写字母</option>
//<option value="5">数字大写字母</option>
//<option value="6">大写小写字母</option>
//<option value="7">数字大写小写</option>
//<option value="8">常用英文字符</option>
//<option value="9">网址和邮件类</option>
//<option value="10">$￥商城价格</option>
//<option value="11">手机电话号类</option>
//<option value="12">数学公式计算</option>

// 当识别类型为非长篇内容时 识别结果直接返回
// 长篇内容识别时返回为结果页面的url地址


//注意
//此接口支持所有功能，但仅支持网络文档识别
//url 要识别文档的地址  
//url应为以http/https/ftp开头的协议
//应为公网可以正常访问的url



//可以用 http://yourname.com/OcrKing.php?u=图片的url   形式提交
//如http://yourname.com/OcrKing.php?u=http://www.e-fa.cn/extend/image.php?auth=EXcQdVFjIkcOPxd1DW4wAF5iNAUJPQ

$url = str_replace('&amp;', '&', substr($_SERVER['QUERY_STRING'], 2));

//识别参数请依要识别的内容根据上面选项进行修改
//下面示例中参数仅适用于电话号码类识别
$var = array (
				//非验证码图片直接用图片url提交识别
				'url' => empty($url) ? 'http://www.e-fa.cn/extend/image.php?auth=EXcQdVFjIkcOPxd1DW4wAF5iNAUJPQ' : $url,
				'language' => 'eng', 
				'service' => 'OcrKingForPhoneNumber', 
				'charset' => 11,
				// 返回内容为utf-8 当需要输出为gbk时 为true 否则请设置false
				'gbk'     => false
);

//实例化OcrKing
$ocrking = new OcrKing(API_KEY);

//提交识别
$ocrking->doOcrKing($var);

//检查识别状态是否成功，出错时会提示
if (!$ocrking->getStatus()) {
	echo $ocrking->getError();
}

//获取识别结果
$result = $ocrking->getResult();

//打印识别结果数组
//print_r($result);

//仅需要识别结果
echo '<ocrking>'.$result['ResultList']['Item']['Result'].'</ocrking>';



/**
 * Enter description here ...
 * @author 
 *
 */
class OcrKing {
	
	//API_URL
	const API_URL = 'http://lab.ocrking.com/ok.html';
	protected $postdata = array('outputFormat' => 'html','app' => 'phpsdk','ver' => '1.0.0.1', 'gbk' => false) ;
	protected $apikey = '';	
	protected $status;
	protected $content = '';
	protected $errormsg;
	protected $result;	
	
	//construct
	public function __construct($apikey) {
		if (isset($apikey) && !empty($apikey)) {
			$this->apikey = $apikey;
		}else{
			$this->status = false;
			$this->errormsg = 'apiKey can not be null ';return ;
		}	
	}
	
	//doOcrKing
	public function doOcrKing($setting = array()) {
		if (isset($setting) && !empty($setting)) {
			$this->postdata = array_merge($this->postdata,$setting);
			$this->postdata = array_unique($this->postdata);
			$this->postdata['apiKey'] = $this->apikey;
		}
		
		if (!array_key_exists('service',$this->postdata) || empty($this->postdata ['service'])) {
			$this->status = false;
			$this->errormsg = ' service can not be null';return ;
		}
		
		if (!array_key_exists('language',$this->postdata) || empty($this->postdata ['language'])) {
			$this->status = false;
			$this->errormsg = 'language can not be null';return ;
		}
		
		if (empty($this->postdata ['url'])) {
			$this->status = false;
			$this->errormsg = 'url can not be null ';return ;
		}
		
		if (!preg_match('/^(http|https|ftp).*?/i', $this->postdata ['url'])) {
			$this->status = false;
			$this->errormsg = 'url must be http/https/ftp ';return ;
		}
				
		$this->doPost();
		$this->result = $this->XML2Array($this->content);
		if ($this->_bool($this->result['ResultList']['Item']['Status']) == false) {
			$this->status = false;
			$this->errormsg = $this->result['ResultList']['Item']['Result'];return ;
		}
	}
	
	
	//destruct
	public function __destruct() {
		foreach ($this as $index => $value)
			unset($this->$index);
	}

	//
	public function getResult() {
		return $this->result;
	}
	//
	public function getError() {
		return $this->errormsg;
	}
	
	//
	public function getStatus() {
		return $this->status;
	}
	
	//bool
	protected function _bool($var) {
		if (is_bool($var)) {
			return $var;
		} else if ($var === NULL || $var === 'NULL' || $var === 'null') {
			return false;
		} else if (is_string($var)) {
			$var = trim($var);
			if ($var == 'false') {
				return false;
			} else if ($var == 'true') {
				return true;
			} else if ($var == 'no') {
				return false;
			} else if ($var == 'yes') {
				return true;
			} else if ($var == 'off') {
				return false;
			} else if ($var == 'on') {
				return true;
			} else if ($var == '') {
				return false;
			} else if (ctype_digit($var)) {
				if ((int) $var)
					return true;
				else
					return false;
			} else {
				return true;
			}
		} else if (ctype_digit((string) $var)) {
			if ((int) $var)
				return true;
			else
				return false;
		} else if (is_array($var)) {
			if (count($var))
				return true;
			else
				return false;
		} else if (is_object($var)) {
			return true;
		} else {
			return true;
		}
	}
	
	//parse xml into array
	protected function XML2Array($xml, $recursive = false) {
		if (!$recursive) {
			$array = simplexml_load_string($xml);
		} else {
			$array = $xml;
		}
		
		$newArray = array ();
		$array = (array) $array;
		foreach ($array as $key => $value) {
			$value = (array) $value;
			if (isset($value [0])) {
				$newArray [$key] = trim($value [0]);
			} else {
				$newArray [$key] = $this->XML2Array($value, true);
			}
		}
		return $newArray;
	}
	
	//getContent
	protected function getContent($url) {
		$ch = curl_init($url);
		$options = array (CURLOPT_HEADER => 1, CURLOPT_FOLLOWLOCATION => 0, CURLOPT_RETURNTRANSFER => 1, CURLOPT_ENCODING => 'gzip,deflate', CURLOPT_REFERER => $this->getSiteUrl(), CURLOPT_USERAGENT => "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)", CURLOPT_TIMEOUT => 60); //
		curl_setopt_array($ch, $options);
		$html = curl_exec($ch);
		curl_close($ch);
		preg_match('/o_h=(.*?);/i', $html, $h);
		preg_match('/SID=(.*?);/i', $html, $sid);
		$cookie = 'o_h=' . $h [1] . '; SID=' . $sid [1] . ';';
		return $cookie;
	}
	
	//doPost
	protected function doPost() {
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, self::API_URL);
		curl_setopt($ch, CURLOPT_HEADER, 0);		curl_setopt($ch, CURLOPT_REFERER, $this->getSiteUrl());		curl_setopt($ch, CURLOPT_USERAGENT, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");$this->getContent('http://lab.ocrking.com/');		curl_setopt($ch, CURLOPT_ENCODING, 'gzip,deflate');		curl_setopt($ch, CURLOPT_POST, 1);
		curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($this->postdata));
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
		$this->content = curl_exec($ch);
		curl_close($ch);
		if ($this->postdata ['gbk']) {
			$this->content = iconv('utf-8', 'gbk', $this->content);
		}		
	}
	
	//HTML
	protected function sHtmlSpecialChars($string) {
		if (is_array($string)) {
			foreach ($string as $key => $val) {
				$string [$key] = shtmlspecialchars($val);
			}
		} else {
			$string = preg_replace('/&amp;((#(\d{3,5}|x[a-fA-F0-9]{4})|[a-zA-Z][a-z0-9]{2,5});)/', '&\\1', str_replace(array ('&', '"', '<', '>'), array ('&amp;', '&quot;', '&lt;', '&gt;'), $string));
		}
		return $string;
	}
	
	//urlpublic static 
	protected function getSiteUrl($all = false) {
		$uri = $_SERVER ['PHP_SELF'] ? $_SERVER ['PHP_SELF'] : $_SERVER ['SCRIPT_NAME'];
		if ($all) {
			return $this->sHtmlSpecialChars('http://' . $_SERVER ['HTTP_HOST'] . substr($uri, 0, strrpos($uri, '/')) . $uri);
		}
		return $this->sHtmlSpecialChars('http://' . $_SERVER ['HTTP_HOST'] . substr($uri, 0, strrpos($uri, '/') + 1));
	}
}
?>