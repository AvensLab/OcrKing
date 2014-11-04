// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="Aven's Lab">
//   QQ 2069798977
// </copyright>
// <summary>
//   The http client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvenLab
{
    #region using region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;

    #endregion

    /// <summary>
    ///     The http client.
    /// </summary>
    internal class HttpClient
    {
        #region Fields

        /// <summary>
        ///     The files.
        /// </summary>
        private readonly List<HttpUploadingFile> files = new List<HttpUploadingFile>();

        /// <summary>
        ///     The posting data.
        /// </summary>
        private readonly Dictionary<string, string> postingData = new Dictionary<string, string>();

        /// <summary>
        ///     The accept.
        /// </summary>
        private string accept = "*/*";

        /// <summary>
        ///     The context.
        /// </summary>
        private HttpClientContext context;

        /// <summary>
        ///     The default encoding.
        /// </summary>
        private Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        ///     The default language.
        /// </summary>
        private string defaultLanguage = "zh-CN";

        /// <summary>
        ///     The end point.
        /// </summary>
        private int endPoint;

        /// <summary>
        ///     The keep context.
        /// </summary>
        private bool keepContext;

        /// <summary>
        ///     The referer.
        /// </summary>
        private string referer;

        /// <summary>
        ///     The response headers.
        /// </summary>
        private WebHeaderCollection responseHeaders;

        /// <summary>
        ///     The start point.
        /// </summary>
        private int startPoint;

        /// <summary>
        ///     The status code.
        /// </summary>
        private HttpStatusCode statusCode;

        /// <summary>
        ///     The url.
        /// </summary>
        private string url;

        /// <summary>
        ///     The user agent.
        /// </summary>
        private string userAgent = "Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0";

        /// <summary>
        ///     The verb.
        /// </summary>
        private HttpVerb verb = HttpVerb.GET;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpClient" /> class.
        ///     构造新的HttpClient实例
        /// </summary>
        public HttpClient()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        ///     构造新的HttpClient实例
        /// </summary>
        /// <param name="url">
        /// 要获取的资源的地址
        /// </param>
        public HttpClient(string url)
            : this(url, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        ///     构造新的HttpClient实例
        /// </summary>
        /// <param name="url">
        /// 要获取的资源的地址
        /// </param>
        /// <param name="context">
        /// Cookie及Referer
        /// </param>
        public HttpClient(string url, HttpClientContext context)
            : this(url, context, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        ///     构造新的HttpClient实例
        /// </summary>
        /// <param name="url">
        /// 要获取的资源的地址
        /// </param>
        /// <param name="context">
        /// Cookie及Referer
        /// </param>
        /// <param name="keepContext">
        /// 是否自动在不同的请求间保留Cookie, Referer
        /// </param>
        public HttpClient(string url, HttpClientContext context, bool keepContext)
        {
            this.url = url;
            this.context = context;
            this.keepContext = keepContext;
            if (this.context == null)
            {
                this.context = new HttpClientContext();
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The status update.
        /// </summary>
        public event EventHandler<StatusUpdateEventArgs> StatusUpdate;

        #endregion

        #region Public Properties

        /// <summary>
        ///     获取或设置期望的资源类型
        /// </summary>
        public string Accept
        {
            get
            {
                return this.accept;
            }

            set
            {
                this.accept = value;
            }
        }

        /// <summary>
        ///     获取或设置Cookie及Referer
        /// </summary>
        public HttpClientContext Context
        {
            get
            {
                return this.context;
            }

            set
            {
                this.context = value;
            }
        }

        /// <summary>
        ///     GetString()如果不能从HTTP头或Meta标签中获取编码信息,则使用此编码来获取字符串
        /// </summary>
        public Encoding DefaultEncoding
        {
            get
            {
                return this.defaultEncoding;
            }

            set
            {
                this.defaultEncoding = value;
            }
        }

        /// <summary>
        ///     期望的回应的语言
        /// </summary>
        public string DefaultLanguage
        {
            get
            {
                return this.defaultLanguage;
            }

            set
            {
                this.defaultLanguage = value;
            }
        }

        /// <summary>
        ///     获取或设置获取内容的结束点,用于断点续传,多下程下载等.
        ///     如果为0,表示获取资源从StartPoint开始的剩余内容
        /// </summary>
        public int EndPoint
        {
            get
            {
                return this.endPoint;
            }

            set
            {
                this.endPoint = value;
            }
        }

        /// <summary>
        ///     要上传的文件.如果不为空则自动转为Post请求
        /// </summary>
        public List<HttpUploadingFile> Files
        {
            get
            {
                return this.files;
            }
        }

        /// <summary>
        ///     是否自动在不同的请求间保留Cookie, Referer
        /// </summary>
        public bool KeepContext
        {
            get
            {
                return this.keepContext;
            }

            set
            {
                this.keepContext = value;
            }
        }

        /// <summary>
        ///     要发送的Form表单信息
        /// </summary>
        public Dictionary<string, string> PostingData
        {
            get
            {
                return this.postingData;
            }
        }

        /// <summary>
        ///     Gets or sets the referer.
        /// </summary>
        public string Referer
        {
            get
            {
                return this.referer;
            }

            set
            {
                this.referer = value;
            }
        }

        /// <summary>
        ///     用于在获取回应后,暂时记录回应的HTTP头
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get
            {
                return this.responseHeaders;
            }
        }

        /// <summary>
        ///     获取或设置获取内容的起始点,用于断点续传,多线程下载等
        /// </summary>
        public int StartPoint
        {
            get
            {
                return this.startPoint;
            }

            set
            {
                this.startPoint = value;
            }
        }

        /// <summary>
        ///     获取或设置请求资源的地址
        /// </summary>
        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
            }
        }

        /// <summary>
        ///     获取或设置请求中的Http头User-Agent的值
        /// </summary>
        public string UserAgent
        {
            get
            {
                return this.userAgent;
            }

            set
            {
                this.userAgent = value;
            }
        }

        /// <summary>
        ///     指示发出Get请求还是Post请求
        /// </summary>
        public HttpVerb Verb
        {
            get
            {
                return this.verb;
            }

            set
            {
                this.verb = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 在请求中添加要上传的文件
        /// </summary>
        /// <param name="fileName">
        /// 要上传的文件路径
        /// </param>
        /// <param name="fieldName">
        /// 文件字段的名称(相当于&lt;input type=file name=fieldName&gt;)里的fieldName)
        /// </param>
        public void AttachFile(string fileName, string fieldName)
        {
            HttpUploadingFile file = new HttpUploadingFile(fileName, fieldName);
            this.files.Add(file);
        }

        /// <summary>
        /// 在请求中添加要上传的文件
        /// </summary>
        /// <param name="data">
        /// 要上传的文件内容
        /// </param>
        /// <param name="fileName">
        /// 文件名
        /// </param>
        /// <param name="fieldName">
        /// 文件字段的名称(相当于&lt;input type=file name=fieldName&gt;)里的fieldName)
        /// </param>
        public void AttachFile(byte[] data, string fileName, string fieldName)
        {
            HttpUploadingFile file = new HttpUploadingFile(data, fileName, fieldName);
            this.files.Add(file);
        }

        /// <summary>
        ///     发出一次新的请求,并以字节数组形式返回回应的内容
        ///     调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <returns>包含回应主体内容的字节数组</returns>
        public byte[] GetBytes()
        {
            HttpWebResponse res = this.GetResponse();
            int length = (int)res.ContentLength;

            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[0x100];
            Stream rs = res.GetResponseStream();
            for (int i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
            {
                memoryStream.Write(buffer, 0, i);
                this.OnStatusUpdate(new StatusUpdateEventArgs((int)memoryStream.Length, length));
            }

            rs.Close();

            return memoryStream.ToArray();
        }

        /// <summary>
        ///     发出一次新的请求,并返回获得的回应
        ///     调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>相应的HttpWebResponse</returns>
        public HttpWebResponse GetResponse()
        {
            HttpWebResponse res = null;
            try
            {
                HttpWebRequest req = this.CreateRequest();
                req.KeepAlive = true;
                res = (HttpWebResponse)req.GetResponse();
                this.responseHeaders = res.Headers;
                this.statusCode = res.StatusCode;
                if (this.keepContext)
                {
                    this.context.Cookies = res.Cookies;
                    this.context.Referer = this.url;
                }
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw;
                }

                res = (HttpWebResponse)webException.Response;
            }

            return res;
        }

        /// <summary>
        ///     发出一次新的请求,并返回回应内容的流
        ///     调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>包含回应主体内容的流</returns>
        public Stream GetStream()
        {
            return this.GetResponse().GetResponseStream();
        }

        /// <summary>
        ///     发出一次新的请求,以Http头,或Html Meta标签,或DefaultEncoding指示的编码信息对回应主体解码
        ///     调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <returns>解码后的字符串</returns>
        public string GetString()
        {
            byte[] data = this.GetBytes();
            string encodingName = this.GetEncodingFromHeaders();

            if (encodingName == null)
            {
                encodingName = this.GetEncodingFromBody(data);
            }

            Encoding encoding;
            if (encodingName == null)
            {
                encoding = this.defaultEncoding;
            }
            else
            {
                try
                {
                    encoding = Encoding.GetEncoding(encodingName);
                }
                catch (ArgumentException)
                {
                    encoding = this.defaultEncoding;
                }
            }

            return encoding.GetString(data);
        }

        /// <summary>
        /// 发出一次新的请求,对回应的主体内容以指定的编码进行解码
        ///     调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <param name="encoding">
        /// 指定的编码
        /// </param>
        /// <returns>
        /// 解码后的字符串
        /// </returns>
        public string GetString(Encoding encoding)
        {
            byte[] data = this.GetBytes();
            return encoding.GetString(data);
        }

        /// <summary>
        ///     发出一次新的Head请求,获取资源的长度
        ///     此请求会忽略PostingData, Files, StartPoint, EndPoint, Verb
        /// </summary>
        /// <returns>返回的资源长度</returns>
        public int HeadContentLength()
        {
            this.Reset();
            HttpVerb lastVerb = this.verb;
            this.verb = HttpVerb.HEAD;
            using (HttpWebResponse res = this.GetResponse())
            {
                this.verb = lastVerb;
                return (int)res.ContentLength;
            }
        }

        /// <summary>
        ///     The is http error.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool IsHttpError()
        {
            return this.statusCode != HttpStatusCode.OK;
        }

        /// <summary>
        ///     清空PostingData, Files, StartPoint, EndPoint, ResponseHeaders, 并把Verb设置为Get.
        ///     在发出一个包含上述信息的请求后,必须调用此方法或手工设置相应属性以使下一次请求不会受到影响.
        /// </summary>
        public void Reset()
        {
            this.verb = HttpVerb.GET;
            this.files.Clear();
            this.postingData.Clear();
            this.responseHeaders = null;
            this.startPoint = 0;
            this.endPoint = 0;
            this.referer = null;
        }

        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件
        ///     调用此方法会触发StatusUpdate事件
        ///     如果指定的文件存在,它会被覆盖
        /// </summary>
        /// <param name="fileName">
        /// 要保存的文件路径
        /// </param>
        public void SaveAsFile(string fileName)
        {
            this.SaveAsFile(fileName, FileExistsAction.Overwrite);
        }

        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件
        ///     调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <param name="fileName">
        /// 要保存的文件路径
        /// </param>
        /// <param name="existsAction">
        /// 指定的文件存在时的选项
        /// </param>
        /// <returns>
        /// 是否向目标文件写入了数据
        /// </returns>
        public bool SaveAsFile(string fileName, FileExistsAction existsAction)
        {
            byte[] data = this.GetBytes();
            switch (existsAction)
            {
                case FileExistsAction.Overwrite:
                    using (
                        BinaryWriter writer =
                            new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))) writer.Write(data);
                    return true;

                case FileExistsAction.Append:
                    using (
                        BinaryWriter writer =
                            new BinaryWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write))) writer.Write(data);
                    return true;

                default:
                    if (!File.Exists(fileName))
                    {
                        using (
                            BinaryWriter writer =
                                new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write))) writer.Write(data);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The create request.
        /// </summary>
        /// <returns>
        ///     The <see cref="HttpWebRequest" />.
        /// </returns>
        private HttpWebRequest CreateRequest()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.url);
            req.AllowAutoRedirect = false;
            req.CookieContainer = new CookieContainer();
            req.Headers.Add("Accept-Language", this.defaultLanguage);
            req.Accept = this.accept;
            req.UserAgent = this.userAgent;
            req.KeepAlive = false;

            if (this.context.Cookies != null)
            {
                req.CookieContainer.Add(this.context.Cookies);
            }

            if (!string.IsNullOrEmpty(this.context.Referer))
            {
                req.Referer = this.context.Referer;
            }
            else if (!string.IsNullOrEmpty(this.referer))
            {
                req.Referer = this.referer;
            }

            if (this.verb == HttpVerb.HEAD)
            {
                req.Method = "HEAD";
                return req;
            }

            if (this.postingData.Count > 0 || this.files.Count > 0)
            {
                this.verb = HttpVerb.POST;
            }

            if (this.verb == HttpVerb.POST)
            {
                req.Method = "POST";

                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);

                if (this.files.Count > 0)
                {
                    const string NewLine = "\r\n";
                    string boundary = Guid.NewGuid().ToString().Replace("-", string.Empty);
                    req.ContentType = "multipart/form-data; boundary=" + boundary;

                    foreach (string key in this.postingData.Keys)
                    {
                        writer.Write("--" + boundary + NewLine);
                        writer.Write("Content-Disposition: form-data; name=\"{0}\"{1}{1}", key, NewLine);
                        writer.Write(this.postingData[key] + NewLine);
                    }

                    foreach (HttpUploadingFile file in this.files)
                    {
                        writer.Write("--" + boundary + NewLine);
                        writer.Write(
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", 
                            file.FieldName, 
                            file.FileName, 
                            NewLine);
                        writer.Write("Content-Type: application/octet-stream" + NewLine + NewLine);
                        writer.Flush();
                        memoryStream.Write(file.Data, 0, file.Data.Length);
                        writer.Write(NewLine);
                        writer.Write("--" + boundary + NewLine);
                    }
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                    StringBuilder sb = new StringBuilder();
                    foreach (string key in this.postingData.Keys)
                    {
                        sb.AppendFormat(
                            "{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(this.postingData[key]));
                    }

                    if (sb.Length > 0)
                    {
                        sb.Length--;
                    }

                    writer.Write(sb.ToString());
                }

                writer.Flush();

                using (Stream stream = req.GetRequestStream())
                {
                    memoryStream.WriteTo(stream);
                    this.postingData.Clear();
                }

                memoryStream.Dispose();
            }

            if (this.startPoint != 0 && this.endPoint != 0)
            {
                req.AddRange(this.startPoint, this.endPoint);
            }
            else if (this.startPoint != 0 && this.endPoint == 0)
            {
                req.AddRange(this.startPoint);
            }

            return req;
        }

        /// <summary>
        /// The get encoding from body.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetEncodingFromBody(byte[] data)
        {
            string encodingName = null;
            string dataAsAscii = Encoding.ASCII.GetString(data);
            if (dataAsAscii != null)
            {
                int i = dataAsAscii.IndexOf("charset=");
                if (i != -1)
                {
                    int j = dataAsAscii.IndexOf("\"", i);
                    if (j != -1)
                    {
                        int k = i + 8;
                        encodingName = dataAsAscii.Substring(k, (j - k) + 1);
                        char[] chArray = new[] { '>', '"' };
                        encodingName = encodingName.TrimEnd(chArray);
                    }
                }
            }

            return encodingName;
        }

        /// <summary>
        ///     The get encoding from headers.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private string GetEncodingFromHeaders()
        {
            string encoding = null;
            string contentType = this.responseHeaders["Content-Type"];
            if (contentType != null)
            {
                int i = contentType.IndexOf("charset=");
                if (i != -1)
                {
                    encoding = contentType.Substring(i + 8);
                }
            }

            return encoding;
        }

        /// <summary>
        /// The on status update.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnStatusUpdate(StatusUpdateEventArgs e)
        {
            EventHandler<StatusUpdateEventArgs> temp = this.StatusUpdate;

            if (temp != null)
            {
                temp(this, e);
            }
        }

        #endregion
    }

    /// <summary>
    ///     The http client context.
    /// </summary>
    public class HttpClientContext
    {
        #region Fields

        /// <summary>
        ///     The cookies.
        /// </summary>
        private CookieCollection cookies;

        /// <summary>
        ///     The referer.
        /// </summary>
        private string referer;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the cookies.
        /// </summary>
        public CookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }

            set
            {
                this.cookies = value;
            }
        }

        /// <summary>
        ///     Gets or sets the referer.
        /// </summary>
        public string Referer
        {
            get
            {
                return this.referer;
            }

            set
            {
                this.referer = value;
            }
        }

        #endregion
    }

    /// <summary>
    ///     The http verb.
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        ///     The get.
        /// </summary>
        GET, 

        /// <summary>
        ///     The post.
        /// </summary>
        POST, 

        /// <summary>
        ///     The head.
        /// </summary>
        HEAD, 
    }

    /// <summary>
    ///     The file exists action.
    /// </summary>
    public enum FileExistsAction
    {
        /// <summary>
        ///     The overwrite.
        /// </summary>
        Overwrite, 

        /// <summary>
        ///     The append.
        /// </summary>
        Append, 

        /// <summary>
        ///     The cancel.
        /// </summary>
        Cancel, 
    }

    /// <summary>
    ///     The http uploading file.
    /// </summary>
    public class HttpUploadingFile
    {
        #region Fields

        /// <summary>
        ///     The data.
        /// </summary>
        private byte[] data;

        /// <summary>
        ///     The field name.
        /// </summary>
        private string fieldName;

        /// <summary>
        ///     The file name.
        /// </summary>
        private string fileName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUploadingFile"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        public HttpUploadingFile(string fileName, string fieldName)
        {
            this.fileName = fileName;
            this.fieldName = fieldName;
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                byte[] inBytes = new byte[stream.Length];
                stream.Read(inBytes, 0, inBytes.Length);
                this.data = inBytes;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUploadingFile"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        public HttpUploadingFile(byte[] data, string fileName, string fieldName)
        {
            this.data = data;
            this.fileName = fileName;
            this.fieldName = fieldName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.data = value;
            }
        }

        /// <summary>
        ///     Gets or sets the field name.
        /// </summary>
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }

            set
            {
                this.fieldName = value;
            }
        }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
            }
        }

        #endregion
    }

    /// <summary>
    ///     The status update event args.
    /// </summary>
    public class StatusUpdateEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        ///     The bytes got.
        /// </summary>
        private readonly int bytesGot;

        /// <summary>
        ///     The bytes total.
        /// </summary>
        private readonly int bytesTotal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusUpdateEventArgs"/> class.
        /// </summary>
        /// <param name="got">
        /// The got.
        /// </param>
        /// <param name="total">
        /// The total.
        /// </param>
        public StatusUpdateEventArgs(int got, int total)
        {
            this.bytesGot = got;
            this.bytesTotal = total;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     已经下载的字节数
        /// </summary>
        public int BytesGot
        {
            get
            {
                return this.bytesGot;
            }
        }

        /// <summary>
        ///     资源的总字节数
        /// </summary>
        public int BytesTotal
        {
            get
            {
                return this.bytesTotal;
            }
        }

        #endregion
    }
}