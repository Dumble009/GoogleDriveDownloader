using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleDriveからスプレッドシートを読み込んでディクショナリに変換するクラス
    /// </summary>
    public class SheetLoader : ISheetLoader
    {
        /// <summary>
        /// 列数の最大値
        /// この列数よりも列の数が多いシートは正常に読み込むことが出来ない。
        /// </summary>
        const int COL_LIMIT = 1000;

        /// <summary>
        /// スプレッドシートへのアクセスを提供するオブジェクト
        /// </summary>
        ISpreadSheetsService sheetsService;

        public SheetLoader(ISpreadSheetsService _sheetsService)
        {
            sheetsService = _sheetsService;
        }

        public SheetData LoadSheetData(
            string sheetID,
            string sheetName
        )
        {
            SheetData retVal = new SheetData(); // 返り値

            // シート名が指定されている場合は、範囲クエリに加えられる形に変換する
            if (!string.IsNullOrEmpty(sheetName))
            {
                sheetName = $"{sheetName}!";
            }
            else if (sheetName == null)
            {
                sheetName = "";
            }

            // まずは1行目を読み込んで、パラメータ名を取得する。
            var limitColName = ColIdxToColName(COL_LIMIT);
            var metaDataRange = $"{sheetName}B1:{limitColName}1"; // 1列目はIDで決まりなので2列目から取得する
            var values = sheetsService.Get(sheetID, metaDataRange);

            var parameterNames = values[0]; // リストのリストになっており、0番目の要素が1行目を表している
            var parameterCount = parameterNames.Count; // 空欄は無視されるので、1行目の要素の数がそのままパラメータの数になる
            var colEdge = ColIdxToColName(parameterCount);

            // 続いて2行目以降(データ本体)を読み込む。
            int rowIdx = 2;
            while (true)
            {
                var range = $"{sheetName}A{rowIdx}:{colEdge}{rowIdx}";
                values = sheetsService.Get(sheetID, range);

                // 空の行を読み込んだ場合はValuesがnullになるので、それで全ての行を読み込んだかどうか判定
                if (values == null)
                {
                    break;
                }

                // 空でなかった場合は、データを読み込んでパースする。
                var rowData = values[0];
                var id = (string)rowData[0];

                Dictionary<string, string> rowDic = new Dictionary<string, string>();
                for (int i = 0; i < parameterCount; i++)
                {
                    // parameterNamesはB列から始まっているが、
                    // rowDataはA列から始まっているので、1個ずらす必要がある
                    rowDic.Add((string)parameterNames[i], (string)rowData[i + 1]);
                }

                retVal.SetRow(id, rowDic);

                rowIdx++;
            }

            return retVal;
        }

        /// <summary>
        /// 列のインデックス番号を、シート上の列名(A, B, AA等)に変換する
        /// </summary>
        /// <param name="idx">0-indexedな列のインデックス番号。idx=0はAに変換される。</param>
        /// <returns>idxから変換した列名の文字列</returns>
        private string ColIdxToColName(int idx)
        {
            const int ALPHABET_COUNT = 26;
            idx++;
            string colName = "";

            while (idx > 0)
            {
                int mod = (idx - 1) % ALPHABET_COUNT;
                colName = (char)(mod + 'A') + colName;

                idx -= mod;
                idx /= ALPHABET_COUNT;
            }

            return colName;
        }
    }
}