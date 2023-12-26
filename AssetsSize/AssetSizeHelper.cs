using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetStudio;
using Newtonsoft.Json;

namespace AssetStudioGUI
{
    public class AssetSizeHelper
    {
        public static AssetsManager assetsManager = new AssetsManager();
        public static string Statis(string[] paths)
        {
            assetsManager.LoadFiles(paths);
            return BuildAssetData();
        }

        public static void Test()
        {
            string path = "G:/1101/luclover_svn_39006_0.0.1_1_20231101_094957-apk-ab/Bundles/Android";
            string[] files = Directory.GetFiles(path, "*.ab", SearchOption.AllDirectories);
            Statis(files);
        }

        private static string BuildAssetData()
        {
            string productName = null;
            var objectCount = assetsManager.assetsFileList.Sum(x => x.Objects.Count);
            var objectAssetItemDic = new Dictionary<Object, AssetSize>(objectCount);
            var containers = new List<(PPtr<Object>, string)>();
            int i = 0;
            foreach (var assetsFile in assetsManager.assetsFileList)
            {
                foreach (var asset in assetsFile.Objects)
                {
                    var assetItem = new AssetSize();
                    assetItem.originalPath = assetsFile.originalPath;
                    assetItem.bundle = Path.GetFileName(assetsFile.originalPath);
                    assetItem.UniqueID = " #" + i;
                    assetItem.type = asset.type.ToString();
                    assetItem.size = asset.byteSize;
                    objectAssetItemDic.Add(asset, assetItem);
                    switch (asset)
                    {
                        case GameObject m_GameObject:
                            assetItem.name = m_GameObject.m_Name;
                            break;
                        case Texture2D m_Texture2D:
                            if (!string.IsNullOrEmpty(m_Texture2D.m_StreamData?.path))
                                assetItem.size = asset.byteSize + m_Texture2D.m_StreamData.size;
                            assetItem.name = m_Texture2D.m_Name;
                            break;
                        case AudioClip m_AudioClip:
                            if (!string.IsNullOrEmpty(m_AudioClip.m_Source))
                                assetItem.size = asset.byteSize + m_AudioClip.m_Size;
                            assetItem.name = m_AudioClip.m_Name;
                            break;
                        case VideoClip m_VideoClip:
                            if (!string.IsNullOrEmpty(m_VideoClip.m_OriginalPath))
                                assetItem.size = asset.byteSize + (long)m_VideoClip.m_ExternalResources.m_Size;
                            assetItem.name = m_VideoClip.m_Name;
                            break;
                        case Shader m_Shader:
                            assetItem.name = m_Shader.m_ParsedForm?.m_Name ?? m_Shader.m_Name;
                            break;
                        case Mesh _:
                        case TextAsset _:
                        case AnimationClip _:
                        case Font _:
                        case MovieTexture _:
                        case Sprite _:
                            assetItem.name = ((NamedObject)asset).m_Name;
                            break;
                        case Animator m_Animator:
                            if (m_Animator.m_GameObject.TryGet(out var gameObject))
                            {
                                assetItem.name = gameObject.m_Name;
                            }
                            break;
                        case MonoBehaviour m_MonoBehaviour:
                            if (m_MonoBehaviour.m_Name == "" && m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                            {
                                assetItem.name = m_Script.m_ClassName;
                            }
                            else
                            {
                                assetItem.name = m_MonoBehaviour.m_Name;
                            }
                            break;
                        case PlayerSettings m_PlayerSettings:
                            productName = m_PlayerSettings.productName;
                            break;
                        case AssetBundle m_AssetBundle:
                            foreach (var m_Container in m_AssetBundle.m_Container)
                            {
                                var preloadIndex = m_Container.Value.preloadIndex;
                                var preloadSize = m_Container.Value.preloadSize;
                                var preloadEnd = preloadIndex + preloadSize;
                                for (int k = preloadIndex; k < preloadEnd; k++)
                                {
                                    containers.Add((m_AssetBundle.m_PreloadTable[k], m_Container.Key));
                                }
                            }
                            assetItem.name = m_AssetBundle.m_Name;
                            break;
                        case ResourceManager m_ResourceManager:
                            foreach (var m_Container in m_ResourceManager.m_Container)
                            {
                                containers.Add((m_Container.Value, m_Container.Key));
                            }
                            break;
                        case NamedObject m_NamedObject:
                            assetItem.name = m_NamedObject.m_Name;
                            break;
                    }
                    if (assetItem.name == "")
                    {
                        assetItem.name = asset.type.ToString() + assetItem.UniqueID;
                    }
                    
                }
            }
            foreach ((var pptr, var container) in containers)
            {
                if (pptr.TryGet(out var obj))
                {
                    objectAssetItemDic[obj].container = container;
                }
            }
            
            containers.Clear();

            return SaveSize(objectAssetItemDic);
        }

        private static string SaveSize(Dictionary<Object, AssetSize> objectAssetItemDic)
        {
            if (objectAssetItemDic == null || objectAssetItemDic.Count == 0) return string.Empty;

            string path = string.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("name,bundle,container,type,size");
            sb.AppendLine();
            List<AssetSize> result = new List<AssetSize>();
            foreach (var item in objectAssetItemDic)
            {
                var info = item.Value;
                result.Add(info);
                sb.Append(info.ToCSV());
                sb.AppendLine();
                if (path == string.Empty)
                {
                    path = info.originalPath;
                }
            }
            objectAssetItemDic.Clear();

            string file_name = "/AssetSize";// + System.DateTime.Now.ToString("MM-dd-HH-mm-ss")
            File.WriteAllText(Path.GetDirectoryName(path) + file_name + ".csv", sb.ToString());
            File.WriteAllText(Path.GetDirectoryName(path) + file_name + ".json", JsonConvert.SerializeObject(result, Formatting.Indented));
            return file_name;
        }

        private class AssetSize
        {
            public string name;
            public string bundle;
            public string container;
            public string type;
            public float size;
            public string UniqueID;
            public string originalPath;

            public AssetSize() { }
            public AssetSize(string name, string bundle, string container, string type, float size)
            {
                this.name = name;
                this.bundle = bundle;
                this.container = container;
                this.type = type;
                this.size = size;
            }

            public string ToCSV()
            {
                return $"{name},{bundle},{container},{type},{size}";
            }
        }
    }
}
