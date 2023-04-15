using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDrive上のメタシートを読み込んで、使いやすいように変換したデータを返すクラス
    /// </summary>
    public class MetaSheetLoader
    {
        /// <summary>
        /// このソースコードのファイルから設定ファイルまでの相対パス
        /// </summary>
        const string CONFIG_RELATIVE_PATH = "../Config/Config.json";

        /// <summary>
        /// 設定ファイル中のID情報のキー
        /// </summary>
        const string CONFIG_FILE_KEY_ID = "ID";

        /// <summary>
        /// メタシートにおける、シートIDのパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_SHEET_ID = "SheetID";

        /// <summary>
        /// メタシートにおける、スプレッドシート内の参照シートの名前のパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_SHEET_NAME = "SheetName";

        /// <summary>
        /// メタシートにおける、シートからダウンロードした結果を保存するパスのパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_SAVE_PATH = "SavePath";

        /// <summary>
        /// メタシートにおける、データの表示名のパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_DISPLAY_NAME = "DisplayName";

        /// <summary>
        /// スプレッドシートを読み込み変換する処理を行うオブジェクト
        /// </summary>
        ISheetLoader sheetLoader;

        /// <summary>
        /// このコードが書かれているファイルのパスを計算するオブジェクト
        /// </summary>
        ISourceCodeLocator sourceCodeLocator;

        public MetaSheetLoader(
            ISheetLoader _sheetLoader,
            ISourceCodeLocator _sourceCodeLocator
        )
        {
            sheetLoader = _sheetLoader;
            sourceCodeLocator = _sourceCodeLocator;
        }

        /// <summary>
        /// コンフィグファイルのパスを取得する
        /// </summary>
        /// <returns>このソースコードのファイルパスを元に計算された設定ファイルのパス</returns>
        private string GetConfigPath()
        {
            // このソースコードが格納されているディレクトリのパスを取得し、
            // そこからの相対パスとして設定ファイルのパスを作成
            string sourceDirPath = sourceCodeLocator.GetDirectoryOfSourceCodePath();

            return Path.Combine(sourceDirPath, CONFIG_RELATIVE_PATH);
        }

        /// <summary>
        /// 設定ファイルからメタシートのシートIDを読み込んで返す
        /// </summary>
        /// <returns>メタシートのシートIDの文字列</returns>
        private string LoadMetaSheetID()
        {
            try
            {
                // jsonファイルをテキスト形式で開き、読み込んだ文字列をパースする
                using (StreamReader file = File.OpenText(GetConfigPath()))
                {
                    string jsonString = file.ReadToEnd();

                    var dic = JsonConvert
                    .DeserializeObject<Dictionary<string, string>>(jsonString);

                    return dic[CONFIG_FILE_KEY_ID];
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Loading Meta Sheet ID failed." + e.Message);
            }
        }

        /// <summary>
        /// メタシートを読み込んで、各行のデータを要素に持つリストを返す
        /// </summary>
        /// <returns>各行のデータを1つの要素として持つリスト</returns>
        public List<MetaSheetData> LoadMetaSheet()
        {
            List<MetaSheetData> datas = new List<MetaSheetData>();

            // メタシートを読み込んだ結果を辞書として貰い、イテレートする
            var metaSheetDataDic = sheetLoader
            .LoadSheetData(LoadMetaSheetID())
            .Data;

            foreach (var key in metaSheetDataDic.Keys)
            {
                var row = metaSheetDataDic[key];

                int id = int.Parse(key);
                string sheetID = row[META_SHEET_PARAMETER_NAME_SHEET_ID];
                string sheetName = row[META_SHEET_PARAMETER_NAME_SHEET_NAME];
                string savePath = row[META_SHEET_PARAMETER_NAME_SAVE_PATH];
                string displayName = row[META_SHEET_PARAMETER_NAME_DISPLAY_NAME];

                datas.Add(
                    new MetaSheetData(
                        id,
                        sheetID,
                        sheetName,
                        savePath,
                        displayName
                    )
                );
            }

            return datas;
        }
    }
}