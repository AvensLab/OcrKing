// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Charset.cs" company="Aven's Lab">
//   QQ 2069798977
// </copyright>
// <summary>
//   The charset.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvenLab
{
    /// <summary>
    ///     The charset.
    /// </summary>
    public enum Charset
    {
        /// <summary>
        ///     所有英文字符
        /// </summary>
        DEFAULT = 0, 

        /// <summary>
        ///     数字
        /// </summary>
        DIGIT = 1, 

        /// <summary>
        ///     小写英文字母
        /// </summary>
        LOWER = 2, 

        /// <summary>
        ///     大写英文字母
        /// </summary>
        UPPER = 3, 

        /// <summary>
        ///     数字和小写字母
        /// </summary>
        DIGIT_LOWER = 4, 

        /// <summary>
        ///     数字和大写字母
        /// </summary>
        DIGIT_UPPER = 5, 

        /// <summary>
        ///     大写和小写字母
        /// </summary>
        LOWER_UPPER = 6, 

        /// <summary>
        ///     数字大写小写字母
        /// </summary>
        DIGIT_LOWER_UPPER = 7, 

        /// <summary>
        ///     常用英文字符
        /// </summary>
        COMMON_WORDS = 8, 

        /// <summary>
        ///     电子邮件和网址
        /// </summary>
        EMAIL_URL = 9, 

        /// <summary>
        ///     商城价格
        /// </summary>
        PRICE = 10, 

        /// <summary>
        ///     电话号码
        /// </summary>
        PHONENUMBER = 11, 

        /// <summary>
        ///     数学运算
        /// </summary>
        MATH = 12
    }
}