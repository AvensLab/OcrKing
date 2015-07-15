// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Aven's Lab">
//   © 2015 Aven's Lab
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.ServiceDemoGui
{
    #region

    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;
    using System.Xml;

    using AvensLab.Service;
    using AvensLab.Service.Models;

    #endregion

    /// <summary>
    ///     The main form.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        ///     编译前请替换成你自己的
        /// </summary>
        private const string ApiKey = "";

        /// <summary>
        ///     根目录
        /// </summary>
        private static readonly string BaseDir = Application.StartupPath + "\\";

        /// <summary>
        ///     The doc.
        /// </summary>
        private readonly XmlDocument doc;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            this.doc = new XmlDocument();
            this.InitializeComponent();
        }

        /// <summary>
        /// The bt net file_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btNetFile_Click(object sender, EventArgs e)
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
                                  FileUrl = this.tbNetFile.Text.Trim()
                              };

            // 网络文件识别时FileUrl传图片url  此时type可以省略
            // 服务端根据url进行匹配
            ocrKing.DoService();

            this.ParseResult(ocrKing.OcrResult, ocrKing.ProcessStatus);
        }

        /// <summary>
        /// The bt local file_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void btLocalFile_Click(object sender, EventArgs e)
        {
            var newDialog = new OpenFileDialog { Title = "请选择验证码图片", InitialDirectory = BaseDir };

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

            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                // 设置要上传的图片路径, 必须是绝对路径 如 c:\test.png
                ocrKing.FilePath = newDialog.FileName;
                this.tbLocalFile.Text = newDialog.FileName;
            }

            // 上传识别时url可以省略 但此时请传原始验证码的地址到type
            // 方便服务端进行优化匹配  不然结果可能完全不同
            // 如果传递原始url到type 结果是 ckkq 正确的, 
            ocrKing.Type = "https://www.bestpay.com.cn/api/captcha/getCode?1408294248050";

            // 如果不传递原始url到type或乱传一个地址到type 结果很可能就是错的
            // ocrKing.Type = "http://www.e-fa.cn/extend/image.php?auth=EXcQdVFjIkcOPxd1DW4wAF5iNAUJPQ";
            // 如果想禁止后台做任何预处理  type可以设为 http://www.nopreprocess.com
            // 如果确实不确定验证码图片的原url  type可以设为 http://www.unknown.com 此时后台只进行常用预处理
            ocrKing.DoService();

            this.ParseResult(ocrKing.OcrResult, ocrKing.ProcessStatus);
        }

        /// <summary>
        /// The get file.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetFile(string url)
        {
            using (var client = new WebClient())
            {
                return client.OpenRead(url);
            }
        }

        /// <summary>
        /// The get bitmap.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="Bitmap"/>.
        /// </returns>
        private static Bitmap GetBitmap(string url)
        {
            return StreamToBitmap(GetFile(url));
        }

        /// <summary>
        /// The stream to bitmap.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="Bitmap"/>.
        /// </returns>
        private static Bitmap StreamToBitmap(Stream stream)
        {
            using (stream)
            {
                return new Bitmap(stream);
            }
        }

        /// <summary>
        /// The parse result.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="processStats">
        /// processStats
        /// </param>
        private void ParseResult(string result, bool processStats)
        {
            if (processStats)
            {
                // 解析结果
                this.doc.LoadXml(result);

                // 识别结果
                this.tbResult.Text = this.doc.SelectSingleNode("//Results/ResultList/Item/Result").InnerText;

                // 原始图片
                this.pbSrcFile.Image =
                    GetBitmap(this.doc.SelectSingleNode("//Results/ResultList/Item/SrcFile").InnerText);

                // 处理后图片
                this.pbDesFile.Image =
                    GetBitmap(this.doc.SelectSingleNode("//Results/ResultList/Item/DesFile").InnerText);
            }
            else
            {
                // 识别结果
                this.tbResult.Text = result;
            }
        }

        /// <summary>
        /// The pb src file_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void pbSrcFile_Click(object sender, EventArgs e)
        {
            this.pbSrcFile.Image.Save(BaseDir + "test_" + Guid.NewGuid() + ".png");
        }
    }
}