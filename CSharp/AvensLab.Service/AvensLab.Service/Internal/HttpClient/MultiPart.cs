// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiPart.cs" company="Aven's Lab">
//   Copyright (c) 2009 - 2015, Aven's Lab. All rights reserved.
//   //                        http://www.ocrking.com
//   //   DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//   //   The c# script for OcrKing Api
//   //   Welcome to follow us 
//   //   http://weibo.com/OcrKing
//   //   http://t.qq.com/OcrKing
//   //   Just Enjoy The Fun Of OcrKing !
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service.Internal.HttpClient
{
    #region

    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;

    #endregion

    /// <summary>
    ///     The multi part.
    /// </summary>
    internal static class MultiPart
    {
        /// <summary>
        ///     版本
        /// </summary>
        private const string Version = "1.0.0.6";

        /// <summary>
        ///     The referer.
        /// </summary>
        private static readonly string Referer = string.Format("{0}{1}", "http://lab.ocrking.com/?r=cs", Version);

        /// <summary>
        ///     The encoding.
        /// </summary>
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        ///     The random boundary.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string RandomBoundary()
        {
            return string.Format("----------{0:N}", Guid.NewGuid());
        }

        /// <summary>
        /// The form data content type.
        /// </summary>
        /// <param name="boundary">
        /// The boundary.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string FormDataContentType(string boundary)
        {
            return "multipart/form-data; boundary=" + boundary;
        }

        /// <summary>
        /// The multi post.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="formData">
        /// The form data.
        /// </param>
        /// <returns>
        /// The <see cref="CallResponse"/>.
        /// </returns>
        public static CallResponse MultiPost(string url, NameValueCollection formData)
        {
            return MultiPost(url, formData, null);
        }

        /// <summary>
        /// The multi post.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="formData">
        /// The form data.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="CallResponse"/>.
        /// </returns>
        public static CallResponse MultiPost(string url, NameValueCollection formData, string fileName)
        {
            var boundary = RandomBoundary();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            // don't modify code below
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; zh-CN; rv:1.9.1.3) Gecko/20100101 Firefox/8.0";
            webRequest.Referer = Referer;
            webRequest.KeepAlive = true;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);

                using (var fileStream = fileInfo.OpenRead())
                {
                    var postDataStream = GetPostStream(fileStream, fileName, formData, boundary);
                    webRequest.ContentLength = postDataStream.Length;
                    var reqStream = webRequest.GetRequestStream();
                    postDataStream.Position = 0;

                    var buffer = new byte[1024];
                    var bytesRead = 0;

                    while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        reqStream.Write(buffer, 0, bytesRead);
                    }

                    postDataStream.Close();
                    reqStream.Close();
                }
            }
            else
            {
                var postDataStream = GetPostStream(fileName, formData, boundary);
                webRequest.ContentLength = postDataStream.Length;
                var reqStream = webRequest.GetRequestStream();
                postDataStream.Position = 0;

                var buffer = new byte[1024];
                var bytesRead = 0;

                while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    reqStream.Write(buffer, 0, bytesRead);
                }

                postDataStream.Close();
                reqStream.Close();
            }

            try
            {
                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    return HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// The get post stream.
        /// </summary>
        /// <param name="putStream">
        /// The put stream.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="formData">
        /// The form data.
        /// </param>
        /// <param name="boundary">
        /// The boundary.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetPostStream(
            Stream putStream, 
            string fileName, 
            NameValueCollection formData, 
            string boundary)
        {
            Stream postDataStream = new MemoryStream();

            // adding form data
            var formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine
                                         + "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine
                                         + Environment.NewLine + "{1}";

            foreach (string key in formData.Keys)
            {
                var formItemBytes = Encoding.GetBytes(string.Format(formDataHeaderTemplate, key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                // adding file,Stream data
                var fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine
                                         + "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\""
                                         + Environment.NewLine + "Content-Type: application/octet-stream"
                                         + Environment.NewLine + Environment.NewLine;
                var fileHeaderBytes = Encoding.GetBytes(string.Format(fileHeaderTemplate, "file", fileName));
                postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

                var buffer = new byte[1024];
                var bytesRead = 0;
                while ((bytesRead = putStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postDataStream.Write(buffer, 0, bytesRead);
                }

                putStream.Close();
            }

            var endBoundaryBytes = Encoding.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

            return postDataStream;
        }

        /// <summary>
        /// The get post stream.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="formData">
        /// The form data.
        /// </param>
        /// <param name="boundary">
        /// The boundary.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetPostStream(string filePath, NameValueCollection formData, string boundary)
        {
            Stream postDataStream = new MemoryStream();

            // adding form data
            var formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine
                                         + "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine
                                         + Environment.NewLine + "{1}";

            foreach (string key in formData.Keys)
            {
                var formItemBytes = Encoding.GetBytes(string.Format(formDataHeaderTemplate, key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                // adding file data
                var fileInfo = new FileInfo(filePath);
                var fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine
                                         + "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\""
                                         + Environment.NewLine + "Content-Type: application/octet-stream"
                                         + Environment.NewLine + Environment.NewLine;
                var fileHeaderBytes = Encoding.GetBytes(string.Format(fileHeaderTemplate, "file", fileInfo.FullName));
                postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
                var fileStream = fileInfo.OpenRead();
                var buffer = new byte[1024];
                var bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postDataStream.Write(buffer, 0, bytesRead);
                }

                fileStream.Close();
            }

            var endBoundaryBytes = Encoding.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);

            return postDataStream;
        }

        /// <summary>
        /// The handle result.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="CallResponse"/>.
        /// </returns>
        private static CallResponse HandleResult(HttpWebResponse response)
        {
            var statusCode = response.StatusCode;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseStr = reader.ReadToEnd();
                return new CallResponse(statusCode, responseStr);
            }
        }
    }
}