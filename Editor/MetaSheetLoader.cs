using System.IO;
using System.Diagnostics;
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
        const string CONFIG_RELATIVE_PATH = "../Config/config.json";

        /// <summary>
        /// 設定ファイル中のID情報のキー
        /// </summary>
        const string CONFIG_FILE_KEY_ID = "ID";

        /// <summary>
        /// メタシートにおける、IDのパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_ID = "ID";

        /// <summary>
        /// メタシートにおける、シートIDのパラメータ名
        /// </summary>
        const string META_SHEET_PARAMETER_NAME_SHEET_ID = "ShetID";

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

        public MetaSheetLoader()
        {

        }

        /// <summary>
        /// コンフィグファイルのパスを取得する
        /// </summary>
        /// <returns>このソースコードのファイルパスを元に計算された設定ファイルのパス</returns>
        private string GetConfigPath()
        {
            // スタックトレースからこのソースファイルのパスを取得する
            var stackFrame = new StackFrame(true);
            string sourceFilePath = stackFrame.GetFileName();


            // このソースコードが格納されているディレクトリのパスを取得し、
            // そこからの相対パスとして設定ファイルのパスを作成
            string sourceDirPath = Path.GetDirectoryName(sourceFilePath);

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

                    var dic = JsonConvert.DeserializeObject
                                <Dictionary<string, string>>
                                (jsonString);

                    return dic["ID"];
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Loading Meta Sheet ID failed." + e.Message);
            }
        }

        /// <summary>
        /// Googleドライブからメタシートを読み込んで、SheetDataに変換する関数
        /// </summary>
        /// <returns>メタシートのSheetData</returns>
        private SheetData LoadMetaSheetAsSheetData()
        {
            // Googleドライブからのシートの読み込みにはSheetLoaderを使用する
            SheetLoader sheetLoader = new SheetLoader();
            return sheetLoader.LoadSheetData(LoadMetaSheetID());
        }

        /// <summary>
        /// メタシートを読み込んで、各行のデータを要素に持つリストを返す
        /// </summary>
        /// <returns>各行のデータを1つの要素として持つリスト</returns>
        public List<MetaSheetData> LoadMetaSheet()
        {
            List<MetaSheetData> datas = new List<MetaSheetData>();

            // メタシートを読み込んだ結果を辞書として貰い、イテレートする
            var metaSheetDataDic = LoadMetaSheetAsSheetData().Data;

            foreach (var key in metaSheetDataDic.Keys)
            {
                var row = metaSheetDataDic[key];

                int id = int.Parse(row[META_SHEET_PARAMETER_NAME_ID]);
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

            return null;
        }
    }
}