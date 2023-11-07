using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace AssetStudioGUI
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Test();
            //#if !NETFRAMEWORK
            //                        Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //#endif
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new AssetStudioGUIForm());
        }

        public static void Test()
        {
            string path = "G:/1101/luclover_svn_39006_0.0.1_1_20231101_094957-apk-ab/Bundles/Android";
            string[] files = Directory.GetFiles(path, "*.ab", SearchOption.AllDirectories);
            Studio.Statis(files);
        }
    }
}
