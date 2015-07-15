// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Aven's Lab">
//   © 2015 Aven's Lab
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AvensLab.ServiceDemoGui
{
    #region

    using System;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}