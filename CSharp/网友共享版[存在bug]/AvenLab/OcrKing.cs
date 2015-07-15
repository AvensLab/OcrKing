// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OcrKing.cs" company="Aven's Lab">
//   QQ 2069798977
// </copyright>
// <summary>
//   The ocr king.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvenLab
{
    #region using region

    using System;
    using System.Globalization;
    using System.IO;

    #endregion

    /// <summary>
    ///     The ocr king.
    /// </summary>
    public class OcrKing
    {
        #region Constants

        /// <summary>
        ///     The api url.
        /// </summary>
        private const string ApiUrl = "http://lab.ocrking.com/ok.html";

        /// <summary>
        ///     The version.
        /// </summary>
        private const string Version = "0.1";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The referer.
        /// </summary>
        private static readonly string Referer = string.Format("{0}{1}", "http://lab.ocrking.com/?r=cs", Version);

        #endregion

        #region Fields

        /// <summary>
        ///     The client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        ///     The api key.
        /// </summary>
        private string apiKey;

        /// <summary>
        ///     The charset.
        /// </summary>
        private Charset charset;

        /// <summary>
        ///     The have done service.
        /// </summary>
        private bool haveDoneService;

        /// <summary>
        ///     The image path.
        /// </summary>
        private string imagePath;

        /// <summary>
        ///     The image url.
        /// </summary>
        private string imageUrl;

        /// <summary>
        ///     The language.
        /// </summary>
        private Language language;

        /// <summary>
        ///     The result.
        /// </summary>
        private string ocrResult;

        /// <summary>
        ///     The service.
        /// </summary>
        private Service service;

        /// <summary>
        ///     The type.
        /// </summary>
        private string type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="OcrKing" /> class.
        /// </summary>
        /// <param name="apiKey">
        ///     The key.
        /// </param>
        public OcrKing()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OcrKing"/> class.
        /// </summary>
        /// <param name="apiKey">
        /// The api key.
        /// </param>
        public OcrKing(string apiKey)
        {
            this.ApiKey = apiKey;
            this.Language = Language.Eng;
            this.charset = Charset.DEFAULT;
            this.service = Service.OcrKingForCaptcha;
            this.client = new HttpClient(ApiUrl) { Referer = Referer };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The api key.
        /// </summary>
        public string ApiKey
        {
            set
            {
                this.apiKey = value;
            }
        }

        /// <summary>
        ///     The charset.
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
        ///     The image path.
        /// </summary>
        public string ImagePath
        {
            set
            {
                this.imagePath = value;
            }
        }

        /// <summary>
        ///     The image url.
        /// </summary>
        public string ImageUrl
        {
            set
            {
                this.imageUrl = value;
            }
        }

        /// <summary>
        ///     The language.
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
        ///     The ocr result.
        /// </summary>
        public string OcrResult
        {
            get
            {
                return this.haveDoneService ? this.ocrResult : "call DoService first ";
            }
        }

        /// <summary>
        ///     The service.
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
        ///     The type.
        /// </summary>
        public string Type
        {
            set
            {
                this.type = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The do service.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void DoService()
        {
            if (string.IsNullOrEmpty(this.apiKey))
            {
                throw new ArgumentException("apiKey can not be null ", "apiKey");
            }

            this.client.PostingData.Add("apiKey", this.apiKey);
            this.client.PostingData.Add("service", this.service.ToString());
            this.client.PostingData.Add("language", this.Language.ToString().ToLower());

            if (this.language == Language.Eng)
            {
                this.client.PostingData.Add("charset", ((int)this.charset).ToString(CultureInfo.InvariantCulture));
            }

            if (string.IsNullOrEmpty(this.imagePath) && string.IsNullOrEmpty(this.imageUrl))
            {
                throw new Exception(" imagePath and imageUrl can not be null at the same time ");
            }

            if (!string.IsNullOrEmpty(this.imagePath) && File.Exists(this.imagePath))
            {
                this.client.AttachFile(this.imagePath, "ocrfile");

                if (string.IsNullOrEmpty(this.type))
                {
                    throw new ArgumentException("type must be set with the original url of captcha image ", "type");
                }

                if (!this.type.StartsWith("http") && !this.type.StartsWith("ftp"))
                {
                    throw new ArgumentException("type must starts with http/https/ftp ", "type");
                }

                this.client.PostingData.Add("type", this.type);
            }
            else
            {
                if (string.IsNullOrEmpty(this.imageUrl))
                {
                    throw new Exception(" imagePath and imageUrl can not be null at the same time ");
                }

                this.client.PostingData.Add("url", this.imageUrl);
            }

            var result = this.client.GetString();

            this.ocrResult = !this.client.IsHttpError() ? result : "error";
            this.haveDoneService = true;
        }

        #endregion
    }
}