<?php
/*
*########     [Aven's Lab] (C) 2015 Aven's Lab     ########
*########     Time:        2014-11-03                    ########
*########      File:        GBK OcrKing.php                 ########
*########     Author:   真视觉   Email: ts@OcrKing.CoM     ########
*########     WebSite:    http://WwW.OcrKing.CoM                ########
*########     注意：此API为临时,新版本组件发布时会停用！！！     ########
*########     注意：使用此脚本需要开启php curl扩展！     ########
*########     注意：默认示例需要下载图片并保存,请给 下载图片存放目录 写权限     ########
*/

//当前脚本路径
define ( 'S_ROOT', dirname ( __FILE__ ) . DIRECTORY_SEPARATOR );

/*************************以下内容请根据需要修改***************************/
//屏蔽错误
error_reporting ( E_ALL || ~ E_NOTICE );

//请用邮件回复内容中的内容替换下面的    key
define ( 'API_KEY', 'key' );

//图片存放目录
define ( 'DOWN_DIR', S_ROOT . 'down/' );
/*************************以上内容请根据需要修改***************************/

// 引入必须类库
include (S_ROOT .'./class.ocrking.php');


//最新文档说明地址 https://github.com/AvensLab/OcrKing/blob/master/%E7%BA%BF%E4%B8%8A%E8%AF%86%E5%88%ABhttp%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E.txt

// service 识别类型可以为以下value中的值
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
//<option value="jpn">日 语</option>
//<option value="kor">韩 语</option>

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


//注意
// 当识别类型为非长篇内容时 识别结果直接返回
// 长篇内容识别时返回为结果页面的url地址
//此接口支持所有功能，支持网络文档识别
//也支持本地图片识别
//网络文件url应为以http/https/ftp开头的协议


//以下为识别网络图片过程
//识别参数请根据上面的进行修改
$var = array (
				//非验证码类网络图片直接用图片url提交识别
				//如 http://t.51chuli.com/contact/d827323fa035a7729w060771msa9211z.gif
				'url' =>  'http://t.51chuli.com/contact/d827323fa035a7729w060771msa9211z.gif',
				'language' => 'eng', 
				'service' => 'OcrKingForPhoneNumber', 
				'charset' => 11,
				// 服务器返回内容为utf-8  当需要在gbk页面输出时 为true 否则请设置false
				//或者把接口文件另存为utf-8
				'gbk'     => true,
);

//实例化OcrKing识别
$ocrking = new OcrKing(API_KEY);

//提交识别
$ocrking->doOcrKing($var);

//检查识别状态
if (!$ocrking->getStatus()) {
	die ($ocrking->getError());
}

//获取识别结果
$result = $ocrking->getResult();

//原始结果 xml格式,一般用于出错时调试
//echo $ocrking->getRawResult();


//打印识别结果数组
//print_r($result);

//仅显示需要的内容
//echo $result['ResultList']['Item']['Result'];
//echo $result['ResultList']['Item']['Status'];
//echo $result['ResultList']['Item']['DesFile'];

echo '网络图片识别开始：';
echo '<br /><br />识别状态：'.($result['ResultList']['Item']['Status']? '成功' : '失败');
if ($result['ResultList']['Item']['Status'] == 'true') {
		echo '<br /><br />原始图片： <br /><br /><img src="' . $result['ResultList']['Item']['SrcFile'] . '">' ;
		echo '<br /><br />处理后的图片是： <br /><br /><img src="' . $result['ResultList']['Item']['DesFile'] . '">' ;
		echo '<br /><br />识别的结果是：'.$result['ResultList']['Item']['Result'] ;
}





//以下为识别本地验证码图片过程
//如验证码图片的原始地址  https://www.bestpay.com.cn/api/captcha/getCode?1408294248050
//识别参数请根据上面的进行修改
$var = array (				
				'language' => 'eng', 
				'service' => 'OcrKingForCaptcha', 
				'charset' => 7,
				// 服务器返回内容为utf-8  当需要在gbk页面输出时 为true 否则请设置false
				//或者把接口文件另存为utf-8
				'gbk'     => true,
				//使用上传方式识别验证码图片时 
				//这种情况下请传递验证码图片原始url值到type 
				//以便服务端根据url进行优化规则匹配
				//此demo中type值只针对此demo中的图片，其它网站图片请不要用此值
				// 如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的
                // 如果想禁止后台做任何预处理  type可以设为 http://www.nopreprocess.com
                // 如果确实不确定验证码图片的原url  type可以设为 http://www.unknown.com  此时后台只进行常用预处理
				'type'    => 'https://www.bestpay.com.cn/api/captcha/getCode?1408294248050'
);




//保存验证码图片到本地，同时保存对应的cookie
$down = getRemoteFile('https://www.bestpay.com.cn/api/captcha/getCode?1408294248050','.png');


//实例化OcrKing识别
$ocrking = new OcrKing(API_KEY);

//上传图片识别 请在doOcrKing方法前调用
$ocrking->setFilePath($down['imagepath']);

//提交识别
$ocrking->doOcrKing($var);

//检查识别状态
if (!$ocrking->getStatus()) {
	die ($ocrking->getError());
}

//获取识别结果
$result = $ocrking->getResult();

//原始结果 xml格式,一般用于出错时调试
//echo $ocrking->getRawResult();


//打印识别结果数组
//print_r($result);

//仅显示需要的内容
//echo $result['ResultList']['Item']['Result'];
//echo $result['ResultList']['Item']['Status'];
//echo $result['ResultList']['Item']['DesFile'];


echo '<br /><br /><br /><br />本地图片识别开始：<br /><br />';
echo '识别状态：'.($result['ResultList']['Item']['Status']? '成功' : '失败');
if ($result['ResultList']['Item']['Status'] == 'true') {
		echo '<br /><br />原始图片： <br /><br /><img src="' . $result['ResultList']['Item']['SrcFile'] . '">' ;
		echo '<br /><br />处理后图片： <br /><br /><img src="' . $result['ResultList']['Item']['DesFile'] . '">' ;
		echo '<br /><br />识别结果：'.$result['ResultList']['Item']['Result'] ;
		echo '<br /><br /><a href="'.$down['cookieurl'].'" target="_blank">对应cookie</a>' ;
}








/*以下为本地图片上传识别时使用方法，可以自行修改删除*/
//随机字串
function random($length, $numeric = 0) {
	PHP_VERSION < '4.2.0' ? mt_srand ( ( double ) microtime () * 1000000 ) : mt_srand ();
	$seed = base_convert ( md5 ( print_r ( $_SERVER, 1 ) . microtime () ), 16, $numeric ? 10 : 35 );
	$seed = $numeric ? (str_replace ( '0', '', $seed ) . '012340567890') : ($seed . 'zZ' . strtoupper ( $seed ));
	$hash = '';
	$max = strlen ( $seed ) - 1;
	for($i = 0; $i < $length; $i ++) {
		$hash .= $seed [mt_rand ( 0, $max )];
	}
	return $hash;
}

//获取随机文件夹
function getRandDir() {
	$dir = DOWN_DIR;
	$dir1 = date ( 'Ymd', time () );
	$dir2 = substr ( sprintf ( "%09d", rand ( 0, 128 ) ), 6, 3 );
	$dir3 = substr ( sprintf ( "%09d", rand ( 0, 128 ) ), 6, 3 );
	! is_dir ( $dir ) && mkdir ( $dir , 0777 );
	! is_dir ( $dir . '/' . $dir1 ) && mkdir ( $dir . '/' . $dir1, 0777 );
	! is_dir ( $dir . '/' . $dir1 . '/' . $dir2 ) && mkdir ( $dir . '/' . $dir1 . '/' . $dir2, 0777 );
	! is_dir ( $dir . '/' . $dir1 . '/' . $dir2 . '/' . $dir3 ) && mkdir ( $dir . '/' . $dir1 . '/' . $dir2 . '/' . $dir3, 0777 );
	return $dir . $dir1 . '/' . $dir2 . '/' . $dir3 . '/';
}

//下载远程文件 
function getRemoteFile($url, $fileExt) {
	$result = array ();
	$randName = random ( 32 );
	$fileName = getRandDir () . $randName . $fileExt;
	$cookieFile = getRandDir () . $randName .'.txt';
	$ch = curl_init ( $url );
	$fp = fopen ( $fileName, "wb" );
	curl_setopt ( $ch, CURLOPT_FILE, $fp );
	curl_setopt ( $ch, CURLOPT_HEADER, 0 );
	curl_setopt ( $ch, CURLOPT_FOLLOWLOCATION, 1 );
	curl_setopt ( $ch, CURLOPT_SSL_VERIFYHOST, 1 );
	curl_setopt ( $ch, CURLOPT_SSL_VERIFYPEER, 0 );
	curl_setopt ( $ch, CURLOPT_ENCODING, 'gzip,deflate' );
	curl_setopt ( $ch, CURLOPT_COOKIE, '' );
	curl_setopt ( $ch, CURLOPT_COOKIEJAR, $cookieFile);
	curl_setopt ( $ch, CURLOPT_REFERER, getSiteUrl () );
	curl_setopt ( $ch, CURLOPT_USERAGENT, '"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)' );
	curl_setopt ( $ch, CURLOPT_TIMEOUT, 60 );
	curl_exec ( $ch );
	curl_close ( $ch );
	fclose ( $fp );
	$result ['imageurl'] = str_ireplace ( S_ROOT, getSiteUrl (), $fileName );
	$result ['imagepath'] = $fileName;
	$result ['cookieurl'] = str_ireplace ( S_ROOT, getSiteUrl (), $cookieFile );
	return $result;
}

//
function sWriteFile ($fileName, $writetext, $utf8 = false, $openmod = 'w')
{
    if (@$fp = fopen($fileName, $openmod)) {
        flock($fp, 2);
        if ($utf8) {
            fwrite($fp, chr(0xEF) . chr(0xBB) . chr(0xBF) . $writetext);
        } else {
            fwrite($fp, $writetext);
        }
        fclose($fp);
        return true;
    } else {
        return false;
    }
}

function sHtmlSpecialChars($string) {
		if (is_array($string)) {
			foreach ($string as $key => $val) {
				$string [$key] = shtmlspecialchars($val);
			}
		} else {
			$string = preg_replace('/&amp;((#(\d{3,5}|x[a-fA-F0-9]{4})|[a-zA-Z][a-z0-9]{2,5});)/', '&\\1', str_replace(array ('&', '"', '<', '>'), array ('&amp;', '&quot;', '&lt;', '&gt;'), $string));
		}
		return $string;
	}

function getSiteUrl($all = false) {
		$uri = $_SERVER ['PHP_SELF'] ? $_SERVER ['PHP_SELF'] : $_SERVER ['SCRIPT_NAME'];
		if ($all) {
			return sHtmlSpecialChars('http://' . $_SERVER ['HTTP_HOST'] . substr($uri, 0, strrpos($uri, '/')) . $uri);
		}
		return sHtmlSpecialChars('http://' . $_SERVER ['HTTP_HOST'] . substr($uri, 0, strrpos($uri, '/') + 1));
	}