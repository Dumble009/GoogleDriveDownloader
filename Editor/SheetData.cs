using System.Collections.Generic;

namespace googleDriveDownloader
{
    /// <summary>
    /// スプレッドシートから読み込まれたデータをパースしたもの
    /// </summary>
    public class SheetData
    {
        Dictionary<string, Dictionary<string, string>> data;
        /// <summary>
        /// データの実体
        /// 1列目の値をキー、2列目以降の列名と値の辞書をバリューに持つ辞書
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Data
        {
            get
            {
                return data;
            }
        }

        public SheetData()
        {
            data = new Dictionary<string, Dictionary<string, string>>();
        }


        /// <summary>
        /// 1列目の値をキーとして、行データを取得する
        /// </summary>
        /// <param name="key">
        /// 読み込みたい行データの1列目の値
        /// </param>
        /// <returns>
        /// 行データの2列目以降の値の辞書。
        /// 列名をキー、その列の値をバリューに持つ。
        /// </returns>
        public Dictionary<string, string> GetRow(
            string key
        )
        {
            // 辞書の中にキーが無い場合は例外を投げる。
            if (!data.ContainsKey(key))
            {
                throw new KeyNotFoundException(
                    $"A key '{key}' doesn't exist."
                );
            }

            return data[key];
        }

        /// <summary>
        /// 新しい行データを追加する
        /// </summary>
        /// <param name="key">
        /// 1列目の値
        /// </param>
        /// <param name="rowData">
        /// 2列目以降のデータを、列名をキー、セルの値をバリューにした辞書
        /// </param>
        public void SetRow(
            string key,
            Dictionary<string, string> rowData
        )
        {
            // 既に存在するキーが指定された場合は例外を投げる
            if (data.ContainsKey(key))
            {
                throw new System.Exception(
                    $"A key '{key}' already exists."
                );
            }

            data.Add(key, rowData);
        }
    }
}