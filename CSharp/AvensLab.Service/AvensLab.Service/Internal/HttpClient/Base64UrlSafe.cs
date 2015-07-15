// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Base64UrlSafe.cs" company="Aven's Lab">
//   Copyright (c) 2009 - 2015, Aven's Lab. All rights reserved.
//                        http://www.ocrking.com
//   DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//   The c# script for OcrKing Api
//   Welcome to follow us 
//   http://weibo.com/OcrKing
//   http://t.qq.com/OcrKing
//   Just Enjoy The Fun Of OcrKing !
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service.Internal.HttpClient
{
    #region

    using System;
    using System.Text;

    #endregion

    /// <summary>
    ///     The base 64 url safe.
    /// </summary>
    public static class Base64UrlSafe
    {
        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var bs = Encoding.UTF8.GetBytes(text);
            var encodedStr = Convert.ToBase64String(bs);
            encodedStr = encodedStr.Replace('+', '-').Replace('/', '_');
            return encodedStr;
        }

        /// <summary>
        /// string扩展方法，生成base64UrlSafe
        /// </summary>
        /// <param name="str">
        /// string
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToBase64UrlSafe(string str)
        {
            return Encode(str);
        }

        /// <summary>
        /// The encode.
        /// </summary>
        /// <param name="bs">
        /// The bs.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encode(byte[] bs)
        {
            if (bs == null || bs.Length == 0)
            {
                return string.Empty;
            }

            var encodedStr = Convert.ToBase64String(bs);
            encodedStr = encodedStr.Replace('+', '-').Replace('/', '_');
            return encodedStr;
        }
    }
}