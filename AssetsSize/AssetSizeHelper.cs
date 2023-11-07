using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetStudio;

namespace AssetStudioGUI
{
    public class AssetSizeHelper
    {
        public static void Statis(string[] paths)
        //public async static void Statis(string[] paths)
        {
            Studio.assetsManager.LoadFiles(paths);
            Studio.BuildAssetData_();
            //await Task.Run(() => Studio.assetsManager.LoadFiles(paths));
            //await Task.Run(() => Studio.BuildAssetData_());
        }

        public static void Test()
        {
            string path = "G:/1101/luclover_svn_39006_0.0.1_1_20231101_094957-apk-ab/Bundles/Android";
            string[] files = Directory.GetFiles(path, "*.ab", SearchOption.AllDirectories);
            Statis(files);
        }
    }
}
