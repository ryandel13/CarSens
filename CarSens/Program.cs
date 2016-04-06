using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarSens
{
    static class Program
    {
        static public Boolean fullscreen = false;
        static public int appWidth = 1024;
        static public int appHeight = 600;

        public const int sensorTimeout = 3; 

        public const String project = "CarSens 1.2";

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainScreen());
        }
    }
}
