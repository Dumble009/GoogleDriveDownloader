namespace GoogleDriveDownloader
{
    /// <summary>
    /// メタシートの1行分にあたるデータを保持するクラス
    /// </summary>
    public class MetaSheetData
    {
        int id;
        /// <summary>
        /// メタシート内における、データのID
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }
        }

        string sheetID;
        /// <summary>
        /// このデータが対象とするシートのシートID
        /// </summary>
        public string SheetID
        {
            get
            {
                return sheetID;
            }
        }

        string sheetName;
        /// <summary>
        /// このデータが対象とするシート内における、参照シートの名称
        /// </summary>
        public string SheetName
        {
            get
            {
                return sheetName;
            }
        }

        string savePath;
        /// <summary>
        /// このデータが対象とするシートから変換した内容を出力するパス
        /// </summary>
        public string SavePath
        {
            get
            {
                return savePath;
            }
        }

        string displayName;
        /// <summary>
        /// このデータが対象とするシートをUI上で表示する際の名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        public MetaSheetData(
            int _id,
            string _sheetID,
            string _sheetName,
            string _savePath,
            string _displayName
        )
        {
            id = _id;
            sheetID = _sheetID;
            sheetName = _sheetName;
            savePath = _savePath;
            displayName = _displayName;
        }
    }
}