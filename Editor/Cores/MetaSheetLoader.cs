using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDrive上のメタシートを読み込んで、使いやすいように変換したデータを返すクラス
    /// </summary>
    public class MetaSheetLoader : IMetaSheetLoader
    {
        /// <summary>
        /// このソースコードのファイルから設定ファイルまでの相対パス
        /// </summary>
        const string CONFIG_RELATIVE_PATH = "../../Config/Config.json";

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
        /// コンフィグファイルから読み込んだ設定項目を保持しているオブジェクト
        /// </summary>
        IConfig config;

        public MetaSheetLoader(
            ISheetLoader _sheetLoader,
            IConfig _config
        )
        {
            sheetLoader = _sheetLoader;
            config = _config;
        }

        public List<MetaSheetData> LoadMetaSheet()
        {
            List<MetaSheetData> datas = new List<MetaSheetData>();

            // メタシートを読み込んだ結果を辞書として貰い、イテレートする
            var metaSheetDataDic = sheetLoader
            .LoadSheetData(config.GetMetaSheetID())
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