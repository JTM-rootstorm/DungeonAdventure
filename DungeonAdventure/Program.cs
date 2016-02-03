using System;
using System.Windows.Forms;

namespace DungeonAdventure
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new CharGeneration().Show();
            Application.Run();
        }
    }
}
