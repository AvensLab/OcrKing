// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Service.cs" company="Aven's Lab">
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
//   目前支持的服务类型.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.Service.Models
{
    #region

    using System;

    #endregion

    /// <summary>
    ///     目前支持的服务类型.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         目前支持的服务有 长篇识别 PDF识别 手机电话号
    ///         纯数字 商城价格 验证码类 条形二维码 PDF转图片
    ///     </para>
    /// </remarks>
    [Serializable]
    public enum Service
    {
        /// <summary>
        ///     长篇内容
        /// </summary>
        OcrKingForPassages, 

        /// <summary>
        ///     PDF识别
        /// </summary>
        OcrKingForPDF, 

        /// <summary>
        ///     手机电话
        /// </summary>
        OcrKingForPhoneNumber, 

        /// <summary>
        ///     商城价格
        /// </summary>
        OcrKingForPrice, 

        /// <summary>
        ///     纯数字类
        /// </summary>
        OcrKingForNumber, 

        /// <summary>
        ///     验证码类
        /// </summary>
        OcrKingForCaptcha, 

        /// <summary>
        ///     条形二维码
        /// </summary>
        BarcodeDecode, 

        /// <summary>
        ///     PDF转图片
        /// </summary>
        PDFToImage
    }
}