using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetStudio;

namespace AssetStudioGUI
{
    public class AssetSizeHelper
    {
        public static AssetsManager assetsManager = new AssetsManager();
        public async static void Statis(string[] paths)
        {
            await Task.Run(() => assetsManager.LoadFiles(paths));
            (var productName, var treeNodeCollection) = await Task.Run(() => Studio.BuildAssetData());
        }
    }
}
