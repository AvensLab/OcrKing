// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Service.cs" company="Aven's Lab">
//   QQ 2069798977
// </copyright>
// <summary>
//   The services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvenLab
{
    /// <summary>
    ///     The services.
    /// </summary>
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
        PDFToImage, 
    }
}