// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo
{
    #region using region

    using System;
    using System.Threading.Tasks;

    using AvenLab;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        #region Constants

        /// <summary>
        /// 编译前请替换成你自己的 apiKey.
        /// </summary>
        private const string ApiKey = "apiKey";

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
            NetFileOcr();
            UploadFileOcr();
        }

        /// <summary>
        /// The net file ocr.
        /// </summary>
        private static void NetFileOcr()
        {
            var ocrKing = new OcrKing(ApiKey);

            // 网络文件识别时ImageUrl传图片url  此时type可以省略
            // 服务端根据url进行匹配
            ocrKing.Language = Language.Eng;
            ocrKing.Service = Service.OcrKingForPhoneNumber;
            ocrKing.Charset = Charset.PHONENUMBER;
            ocrKing.ImageUrl = "http://t.51chuli.com/contact/d827323fa035a7729w060771msa9211z.gif";
            ocrKing.DoService();
            Console.WriteLine(ocrKing.OcrResult);
        }

        /// <summary>
        /// The upload file ocr.
        /// </summary>
        private static void UploadFileOcr()
        {
            var ocrKing = new OcrKing(ApiKey);

            // 上传识别时url可以省略 但此时请传原始验证码的地址到type
            // 方便服务端进行优化匹配  不然结果可能完全不同
            ocrKing.Language = Language.Eng;
            ocrKing.Service = Service.OcrKingForCaptcha;
            ocrKing.Charset = Charset.DIGIT_LOWER_UPPER;
            // 设置要上传的图片路径, 必须是绝对路径 如 c:\test.png
            ocrKing.ImagePath = Environment.CurrentDirectory + "\\test.png";

            // 如果传递原始url到type 结果是 ckkq 正确的, 
            ocrKing.Type = "https://www.bestpay.com.cn/api/captcha/getCode?1408294248050";

            // 如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的
            // ocrKing.Type = "http://www.e-fa.cn/extend/image.php?auth=EXcQdVFjIkcOPxd1DW4wAF5iNAUJPQ";

            ocrKing.DoService();
            Console.WriteLine(ocrKing.OcrResult);
        }

        #endregion
    }
}