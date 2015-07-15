// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="Aven's Lab">
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
//   目前可以识别的语种.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service.Models
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     目前可以识别的语种.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         目前支持英文,中文,繁体中文,日语，韩语
    ///     </para>
    /// </remarks>
    [Serializable]
    public enum Language
    {
        /// <summary>
        ///     英 语
        /// </summary>
        Eng = 0, 

        /// <summary>
        ///     简 体
        /// </summary>
        Sim, 

        /// <summary>
        ///     繁 体
        /// </summary>
        Tra, 

        /// <summary>
        ///     日 语
        /// </summary>
        Jpn, 

        /// <summary>
        ///     韩 语
        /// </summary>
        Kor
    }
}