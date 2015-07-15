// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Charset.cs" company="Aven's Lab">
//   Copyright (c) 2009 - 2015, Aven's Lab. All rights reserved.
//                        http://www.ocrking.com
//   DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//   The c# script for OcrKing Api
//   Welcome to follow us 
//   http://weibo.com/OcrKing
//   http://t.qq.com/OcrKing
//   Just Enjoy The Fun Of OcrKing !
// </copyright>
// <summary>
//   识别内容字符集.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service.Models
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     识别内容字符集.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         对非CJK(中日韩)类内容的验证码识别时,正确使用
    ///         字符集可以大大提高识别结果的正确率
    ///     </para>
    /// </remarks>
    [Serializable]
    public enum Charset
    {
        /// <summary>
        ///     所有英文字符
        /// </summary>
        /// <para>
        /// </para>
        Default = 0, 

        /// <summary>
        ///     数字
        /// </summary>
        /// <para>
        ///     字符集包括 0-9
        /// </para>
        Digit = 1, 

        /// <summary>
        ///     小写英文字母
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         字符集包括 a-z
        ///     </para>
        /// </remarks>
        Lower = 2, 

        /// <summary>
        ///     大写英文字母
        /// </summary>
        /// <para>
        ///     字符集包括 A-Z
        /// </para>
        Upper = 3, 

        /// <summary>
        ///     数字和小写字母
        /// </summary>
        /// <para>
        ///     字符集包括 0-9 a-z
        /// </para>
        DigitLower = 4, 

        /// <summary>
        ///     数字和大写字母
        /// </summary>
        /// <para>
        ///     字符集包括 0-9 A-Z
        /// </para>
        DigitUpper = 5, 

        /// <summary>
        ///     大写和小写字母
        /// </summary>
        /// <para>
        ///     字符集包括 a-z A-Z
        /// </para>
        LowerUpper = 6, 

        /// <summary>
        ///     数字小写大写字母
        /// </summary>
        /// <para>
        ///     字符集包括 0-9 A-Z A-Z
        /// </para>
        DigitLowerUpper = 7, 

        /// <summary>
        ///     The common words.
        /// </summary>
        CommonWords = 8, 

        /// <summary>
        ///     电子邮件和网址
        /// </summary>
        /// ///
        /// <para>
        ///     字符集包括 0-9 A-Z A-Z @:/.-_#?%=
        /// </para>
        EmailUrl = 9, 

        /// <summary>
        ///     商城价格
        /// </summary>
        /// <para>
        ///     字符集包括 0-9.￥$
        /// </para>
        Price = 10, 

        /// <summary>
        ///     电话号码
        /// </summary>
        /// <para>
        ///     字符集包括 0123456789-
        /// </para>
        PhoneNumber = 11, 

        /// <summary>
        ///     数学运算
        /// </summary>
        /// <para>
        ///     字符集包括 0123456789+-*X/=?
        /// </para>
        Math = 12
    }
}