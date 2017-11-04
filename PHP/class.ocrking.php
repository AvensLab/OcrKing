<?php
/*
*########     [Aven's Lab] (C) 2017 Aven's Lab     ########
*########     Time:        2014-11-03                    ########
*########      File:        GBK class.ocrking.php                 ########
*########     Author:     Email: ts@OcrKing.CoM     ########
*########     WebSite:    http://WwW.OcrKing.CoM                ########
*########     注意：使用此脚本需要开启php curl扩展！     ########
*########     注意：如果不熟悉，请匆修改此文件，否则可能不能正常工作     ########
*/

/**
 * OcrKing Api For PHP
 * @author ts@OcrKing.CoM
 *
 */
class OcrKing {
	
	//API_URL
	const API_URL = 'http://lab.ocrking.com/ok.html';
	protected $postdata = array('outputFormat' => 'html','app' => 'phpsdk','ver' => '1.0.0.1', 'gbk' => false,'imagestr' =>'') ;
	protected $apikey = '';	
	protected $status;
	protected $ocrfile = '';
	protected $content = '';
	protected $errormsg = '';
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
		
		if (empty($this->postdata ['url']) && empty($this->ocrfile) && empty($this->postdata ['ocrfile'])) {
			$this->status = false;
			$this->errormsg = 'url and ocrfile can not be null at the same time ';return ;
		}
		
		if (!empty($this->postdata ['url']) && !preg_match('/^(http|https|ftp).*?/i', $this->postdata ['url'])) {
			$this->status = false;
			$this->errormsg = 'url must be http/https/ftp ';return ;
		}
		
		if (!empty($this->ocrfile)) {
			if (class_exists('\CURLFile')) {				
					$this->postdata ['ocrfile'] = new \CURLFile($this->ocrfile);
			 } else {
					$this->postdata ['ocrfile'] = '@'.$this->ocrfile; 
			 }
			if (empty($this->postdata ['type'])) {
				$this->status = false;
				$this->errormsg = 'type must be set with the original url of captcha image';return ;
			}else{
			if (!preg_match('/^(http|https|ftp).*?/i', $this->postdata ['type'])) {
				$this->status = false;
				$this->errormsg = 'type must be set with the original url of captcha image ';return ;
				}
			}
		}
		
		$this->doPost();
		$this->result = $this->XML2Array($this->content);
		$this->status = $this->_bool($this->result['ResultList']['Item']['Status']);
		$this->errormsg = $this->result['ResultList']['Item']['Result'];
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
	public function getRawResult() {
		return $this->content;
	}
	//
	public function getStatus() {
		return $this->status;
	}
	
	//set file path
	public function setFilePath($file) {
		if (file_exists($file)) {
			$this->ocrfile = $file;
		}		
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
				if ($this->postdata ['gbk']) {
					$newArray [$key] =  iconv('utf-8', 'gbk', trim($value [0]));
				}
				else
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
		curl_setopt($ch, CURLOPT_HEADER, 0);   curl_setopt($ch, CURLOPT_REFERER, $this->getSiteUrl());		curl_setopt($ch, CURLOPT_USERAGENT, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");$this->getContent('http://lab.ocrking.com/?php0.1');		curl_setopt($ch, CURLOPT_ENCODING, 'gzip,deflate');	curl_setopt($ch, CURLOPT_POST, 1);
		curl_setopt($ch, CURLOPT_POSTFIELDS, $this->postdata);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
		$this->content = curl_exec($ch);
		curl_close($ch);	
	}
	
	//HTML
	protected function sHtmlSpecialChars($string) {
		if (is_array($string)) {
			foreach ($string as $key => $val) {
				$string [$key] = $this->shtmlspecialchars($val);
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

