// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Aven's Lab">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    ///     The http helper.
    /// </summary>
    internal class HttpHelper
    {
        /// <summary>
        /// Url encodes the specified string.
        /// </summary>
        /// <param name="s">
        /// The string to url encode.
        /// </param>
        /// <returns>
        /// The url encoded string.
        /// </returns>
        public static string UrlEncode(string s)
        {
            const int MaxLimit = 1000;
            if (s == null || s.Length <= MaxLimit)
            {
                return Uri.EscapeDataString(s);
            }

            var sb = new StringBuilder();
            var loops = s.Length / MaxLimit;

            for (var i = 0; i <= loops; i++)
            {
                sb.Append(
                    i < loops
                        ? Uri.EscapeDataString(s.Substring(MaxLimit * i, MaxLimit))
                        : Uri.EscapeDataString(s.Substring(MaxLimit * i)));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Url decodes the specified string.
        /// </summary>
        /// <param name="s">
        /// The string to url decode.
        /// </param>
        /// <returns>
        /// The url decoded string.
        /// </returns>
        public static string UrlDecode(string s)
        {
            return UrlDecode(s, Encoding.UTF8);
        }

        /// <summary>
        /// The url decode.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string UrlDecode(string s, Encoding e)
        {
            if (null == s)
            {
                return null;
            }

            if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
            {
                return s;
            }

            if (e == null)
            {
                e = Encoding.UTF8;
            }

            long len = s.Length;
            var bytes = new List<byte>();

            for (var i = 0; i < len; i++)
            {
                var ch = s[i];
                if (ch == '%' && i + 2 < len && s[i + 1] != '%')
                {
                    int xchar;
                    if (s[i + 1] == 'u' && i + 5 < len)
                    {
                        // unicode hex sequence
                        xchar = GetChar(s, i + 2, 4);
                        if (xchar != -1)
                        {
                            WriteCharBytes(bytes, (char)xchar, e);
                            i += 5;
                        }
                        else
                        {
                            WriteCharBytes(bytes, '%', e);
                        }
                    }
                    else if ((xchar = GetChar(s, i + 1, 2)) != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, e);
                        i += 2;
                    }
                    else
                    {
                        WriteCharBytes(bytes, '%', e);
                    }

                    continue;
                }

                WriteCharBytes(bytes, ch == '+' ? ' ' : ch, e);
            }

            var buf = bytes.ToArray();
            bytes = null;
            return e.GetString(buf, 0, buf.Length);
        }

        /// <summary>
        /// The write char bytes.
        /// </summary>
        /// <param name="buf">
        /// The buf.
        /// </param>
        /// <param name="ch">
        /// The ch.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void WriteCharBytes(IList buf, char ch, Encoding e)
        {
            if (ch > 255)
            {
                foreach (var b in e.GetBytes(new[] { ch }))
                {
                    buf.Add(b);
                }
            }
            else
            {
                buf.Add((byte)ch);
            }
        }

        /// <summary>
        /// The get char.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int GetChar(string str, int offset, int length)
        {
            var val = 0;
            var end = length + offset;
            for (var i = offset; i < end; i++)
            {
                var c = str[i];
                if (c > 127)
                {
                    return -1;
                }

                var current = GetInt((byte)c);
                if (current == -1)
                {
                    return -1;
                }

                val = (val << 4) + current;
            }

            return val;
        }

        /// <summary>
        /// The get int.
        /// </summary>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int GetInt(byte b)
        {
            var c = (char)b;
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }

            return -1;
        }
    }
}