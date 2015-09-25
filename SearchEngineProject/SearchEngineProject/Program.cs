using System;
using System.Windows.Forms;

namespace SearchEngineProject
{
    internal static class Program
    {
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}