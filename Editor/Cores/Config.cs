using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// コンフィグファイルを読み込み、その内容を外部のクラスに提供する
    /// </summary>
    public class Config : IConfig
    {
        /// <summary>
        /// このソースコードが置かれているパスからコンフィグファイルまでの相対パス
        /// </summary>
        const string CONFIG_RELATIVE_PATH = "../../Config/Config.json";

        /// <summary>
        /// コンフィグファイル内の、メタシートのIDを持つ値の名前
        /// </summary>
        const string META_SHEET_ID_KEY = "MetaSheetID";

        /// <summary>
        /// コンフィグファイル内の、エクスポート先のルートパスを持つ値の名前
        /// </summary>
        const string EXPORT_ROOT_PATH_KEY = "ExportRootPath";

        /// <summary>
        /// Googleドライブ上におけるメタシートのID。
        /// コンフィグファイルから読み込まれる設定項目の一つ
        /// </summary>
        string metaSheetID = "";

        /// <summary>
        /// ファイルのエクスポート先のルートフォルダのパス。
        /// 絶対パス形式で持つ
        /// </summary>
        string exportRootPath = "";

        /// <summary>
        /// オブジェクトの作成と同時に、コンフィグファイルの読み込み、内容のパースを行う
        /// </summary>
        /// <param name="sourceCodeLocator">
        /// コンフィグファイルのパスを計算するために使用する、ソースコードのパスを返してくれるオブジェクト
        /// </param>
        public Config(ISourceCodeLocator sourceCodeLocator)
        {
            string configFilePath = Path.Combine(
                sourceCodeLocator.GetDirectoryOfSourceCodePath(),
                CONFIG_RELATIVE_PATH
            );

            using (StreamReader file = File.OpenText(configFilePath))
            {
                string jsonString = file.ReadToEnd();
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

                metaSheetID = dic[META_SHEET_ID_KEY];

                // コンフィグファイル内のルートパスはAssetsからの相対パスで書かれているので
                // Assetsのパスと結合してパスを完成させる。
                exportRootPath = Path.Combine(
                    Application.dataPath,
                    dic[EXPORT_ROOT_PATH_KEY]
                );
            }
        }

        public string GetExportRootPath()
        {
            return exportRootPath;
        }

        public string GetMetaSheetID()
        {
            return metaSheetID;
        }
    }
}