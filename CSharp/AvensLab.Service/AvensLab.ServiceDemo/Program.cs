// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Aven's Lab">
//   © 2015 Aven's Lab
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.ServiceDemo
{
    #region

    using System;
    using System.Diagnostics;


    using AvensLab.Service;
    using AvensLab.Service.Models;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    internal class Program
    {
        #region Constants

        /// <summary>
        ///     编译前请替换成你自己的 apiKey.
        /// </summary>
        private const string ApiKey = "";

        #endregion

        #region Methods

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            var loops = 0;
            if (args.Length >= 1)
            {
                if (!int.TryParse(args[0], out loops))
                {
                    loops = 10;
                }
            }
            else
            {
                loops = 10;
            }

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < loops; i++)
            {
                // NetFileOcr();
                UploadFileOcr();
                Console.WriteLine("第 {0} 次 ", i + 1);
            }

            sw.Stop();
            Console.WriteLine(
                "共请求 {2}次 总时间 {0}ms  平均用时 {1}ms ", 
                sw.ElapsedMilliseconds, 
                sw.ElapsedMilliseconds / loops, 
                loops);
        }

        /// <summary>
        ///     The net file ocr.
        /// </summary>
        private static void NetFileOcr()
        {
            // 默认请求提交url为 http://lab.ocrking.com/ok.html
            // 后台不定时增加或减少apiUrl，当您使用的apiUrl无法正常请求时
            // 请通过 http://api.ocrking.com/server.html 获取
            // 使用其它请求apiUrl可以这样初始化
            // var ocrKing = new OcrKing(ApiKey, "http://www.ocrking.com/")
            var ocrKing = new OcrKing(ApiKey)
                              {
                                  Language = Language.Eng, 
                                  Service = Service.OcrKingForCaptcha, 
                                  Charset = Charset.DigitLowerUpper, 
                                  FileUrl =
                                      "https://www.bestpay.com.cn/api/captcha/getCode?1408294248050"
                              };

            // 网络文件识别时FileUrl传图片url  此时type可以省略
            // 服务端根据url进行匹配
            ocrKing.DoService();

            // 识别请求状态及结果
            // 检查是不是请求成功
            if (ocrKing.ProcessStatus)
            {
                Console.WriteLine(ocrKing.OcrResult);
            }
            else
            {
                Console.WriteLine("有错误发生， {0}", ocrKing.OcrResult);
            }
        }

        /// <summary>
        ///     The upload file ocr.
        /// </summary>
        private static void UploadFileOcr()
        {
            // 默认请求提交url为 http://lab.ocrking.com/ok.html
            // 后台不定时增加或减少apiUrl，当您使用的apiUrl无法正常请求时
            // 请通过 http://api.ocrking.com/server.html 获取
            // 使用其它请求apiUrl可以这样初始化
            // var ocrKing = new OcrKing(ApiKey, "http://www.ocrking.com/")
            var ocrKing = new OcrKing(ApiKey)
                              {
                                  Language = Language.Eng, 
                                  Service = Service.OcrKingForCaptcha, 
                                  Charset = Charset.DigitLowerUpper
                              };

            // 设置要上传的图片路径, 必须是绝对路径 如 c:\test.png
            ocrKing.FilePath = Environment.CurrentDirectory + "\\test.png";

            // 上传识别时url可以省略 但此时请传原始验证码的地址到type
            // 方便服务端进行优化匹配  不然结果可能完全不同
            // 如果传递原始url到type 结果是 ckkq 正确的, 
            ocrKing.Type = "https://www.bestpay.com.cn/api/captcha/getCode?1408294248050";

            // 如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的
            // ocrKing.Type = "http://www.e-fa.cn/extend/image.php?auth=EXcQdVFjIkcOPxd1DW4wAF5iNAUJPQ";
            // 如果想禁止后台做任何预处理  type可以设为 http://www.nopreprocess.com
            // 如果确实不确定验证码图片的原url  type可以设为 http://www.unknown.com  此时后台只进行常用预处理
            ocrKing.DoService();


            // 识别请求状态及结果
            // 检查是不是请求成功
            if (ocrKing.ProcessStatus)
            {
                Console.WriteLine(ocrKing.OcrResult);
            }
            else
            {
                Console.WriteLine("有错误发生， {0}", ocrKing.OcrResult);
            }
        }

        #endregion
    }
}