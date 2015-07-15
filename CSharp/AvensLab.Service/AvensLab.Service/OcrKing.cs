// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OcrKing.cs" company="Aven's Lab">
//   Copyright (c) 2009 - 2015, Aven's Lab. All rights reserved.
//   //   //   //                        http://www.ocrking.com
//   //   //   //   DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//   //   //   //   The c# script for OcrKing Api
//   //   //   //   Welcome to follow us 
//   //   //   //   http://weibo.com/OcrKing
//   //   //   //   http://t.qq.com/OcrKing
//   //   //   //   Just Enjoy The Fun Of OcrKing !
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service
{
    #region

    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;

    using AvensLab.Service.Internal.HttpClient;
    using AvensLab.Service.Models;

    #endregion

    /// <summary>
    ///     识别转换类.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         目前支持的服务有 长篇识别 PDF识别 手机电话号
    ///         纯数字 商城价格 验证码类 条形二维码 PDF转图片
    ///     </para>
    /// </remarks>
    public class OcrKing
    {
        /// <summary>
        ///     默认服务器请求地址
        /// </summary>
        private const string DefaultApiUrl = "http://lab.ocrking.com/ok.html";

        /// <summary>
        ///     请求提交地址
        /// </summary>
        private readonly string apiUrl;

        /// <summary>
        ///     提交数据
        /// </summary>
        private readonly NameValueCollection formData;

        /// <summary>
        ///     授权字串
        /// </summary>
        private string apiKey;

        /// <summary>
        ///     字符集
        /// </summary>
        private Charset charset;

        /// <summary>
        ///     图片本地路径
        /// </summary>
        private string filePath;

        /// <summary>
        ///     网络图片的url
        /// </summary>
        private string fileUrl;

        /// <summary>
        ///     提交状态
        /// </summary>
        private bool haveDoneService;

        /// <summary>
        ///     识别语种
        /// </summary>
        private Language language;

        /// <summary>
        ///     识别结果
        /// 当processStatus为true时 返回识别结果xml文本
        /// 当processStatus为false时 返回 错误消息 格式为   Error：错误消息
        /// </summary>
        private string ocrResult;

        /// <summary>
        ///     请求响应
        /// </summary>
        private CallResponse response;

        /// <summary>
        ///     服务类型
        /// </summary>
        private Service service;

        /// <summary>
        ///     type值
        /// </summary>
        private string type;

        /// <summary>
        /// 请求状态
        /// 请求成功且没有异常时才为true
        /// </summary>
        private bool processStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="OcrKing"/> class.
        /// </summary>
        /// <param name="apiKey">
        /// 授权ApiKey,获取方式请看群共享中的文档说明
        /// </param>
        public OcrKing(string apiKey)
            : this(apiKey, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OcrKing"/> class.
        /// </summary>
        /// <param name="apiKey">
        /// 授权ApiKey,获取方式请看群共享中的文档说明
        /// </param>
        /// <param name="apiUrl">
        /// 服务请求地址,当您使用的地址不能正常访问时
        ///     请通过 http://api.ocrking.com/server.html 获取
        ///     后台不定时增加或减少请求地址，
        /// </param>
        public OcrKing(string apiKey, string apiUrl)
        {
            this.ApiKey = apiKey;
            this.apiUrl = string.IsNullOrEmpty(apiUrl) ? DefaultApiUrl : apiUrl.Trim();
            this.Language = Language.Eng;
            this.charset = Charset.Default;
            this.service = Service.OcrKingForCaptcha;
            this.formData = new NameValueCollection();
        }

        /// <summary>
        ///     授权字串
        /// </summary>
        public string ApiKey
        {
            set
            {
                this.apiKey = value;
            }
        }

        /// <summary>
        ///     字符集
        /// </summary>
        public Charset Charset
        {
            get
            {
                return this.charset;
            }

            set
            {
                this.charset = value;
            }
        }

        /// <summary>
        ///     图片本地路径
        /// </summary>
        public string FilePath
        {
            set
            {
                this.filePath = value;
            }
        }

        /// <summary>
        ///     网络图片url
        /// </summary>
        public string FileUrl
        {
            set
            {
                this.fileUrl = value;
            }
        }

        /// <summary>
        ///     识别语种
        /// </summary>
        public Language Language
        {
            get
            {
                return this.language;
            }

            set
            {
                this.language = value;
            }
        }

        /// <summary>
        ///     识别结果
        /// 当processStatus为true时 返回识别结果xml文本
        /// 当processStatus为false时 返回 错误消息 格式为   Error： 错误消息
        /// </summary>
        public string OcrResult
        {
            get
            {
                return this.haveDoneService ? this.ocrResult : "请先调用 DoService 方法 ";
            }
        }

        /// <summary>
        ///     服务类型
        /// </summary>
        public Service Service
        {
            get
            {
                return this.service;
            }

            set
            {
                this.service = value;
            }
        }

        /// <summary>
        ///     type值
        /// </summary>
        public string Type
        {
            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// 请求状态
        /// 请求成功且没有异常时才为true
        /// </summary>
        public bool ProcessStatus
        {
            get
            {
                return this.processStatus;
            }
        }

        /// <summary>
        ///     发送请求
        /// </summary>
        public void DoService()
        {
            if (string.IsNullOrEmpty(this.apiKey))
            {
                this.haveDoneService = true;
                this.ocrResult = "Error：ApiKey 不能为空 ";
                return;
            }

            this.formData["apiKey"] = this.apiKey;
            this.formData["service"] = this.service.ToString();
            this.formData["language"] = this.Language.ToString().ToLower();

            if (this.language == Language.Eng)
            {
                this.formData["charset"] = ((int)this.charset).ToString(CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(this.filePath) && string.IsNullOrEmpty(this.fileUrl))
            {
                this.haveDoneService = true;
                this.ocrResult = "Error：filePath 和 fileUrl 不能同时为空 ";
                return;
            }

            if (!string.IsNullOrEmpty(this.filePath) && File.Exists(this.filePath))
            {
                if (string.IsNullOrEmpty(this.type))
                {
                    this.haveDoneService = true;
                    this.ocrResult = "Error：上传识别验证码时 type 必须传图片的原始url ";
                    return;
                }

                if (!this.type.StartsWith("http") && !this.type.StartsWith("ftp"))
                {
                    this.haveDoneService = true;
                    this.ocrResult = "Error：type 必须以 http/https/ftp 开头 ";
                    return;
                }

                this.formData["type"] = this.type;
                this.response = MultiPart.MultiPost(this.apiUrl, this.formData, this.filePath);
            }
            else
            {
                if (string.IsNullOrEmpty(this.fileUrl))
                {
                    this.haveDoneService = true;
                    this.ocrResult = "Error：filePath 和 fileUrl 不能同时为空  ";
                    return;
                }

                this.formData["url"] = this.fileUrl;
                this.response = MultiPart.MultiPost(this.apiUrl, this.formData);
            }

            // 检查服务器响应状态
            if (this.response.OK)
            {
                this.processStatus = true;
                this.ocrResult = this.response.Response;
            }
            else
            {
                this.ocrResult = string.Format("Error： {0}", this.response.StatusCode);
            }

            this.haveDoneService = true;
        }
    }
}