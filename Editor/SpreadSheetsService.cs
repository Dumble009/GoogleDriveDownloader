using System.Collections.Generic;

using Google.Apis.Sheets.v4;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleAPIを通じて、Googleドライブ上のスプレッドシートを読み込む
    /// </summary>
    public class SpreadSheetsService
    {
        /// <summary>
        /// GoogleAPIが提供するスプレッドシートサービスのオブジェクト
        /// </summary>
        SheetsService sheetsService;

        public SpreadSheetsService()
        {
            sheetsService = new GoogleAuthAgent().CreateSheetsService();
        }

        /// <summary>
        /// スプレッドシートのIDとデータの範囲を指定し、データを取得する
        /// </summary>
        /// <param name="sheetID">
        /// 読み込みたいスプレッドシートのID
        /// </param>
        /// <param name="range">
        /// データを読み込みたい範囲
        /// "A1:B4"といった形式で指定する事
        /// </param>
        /// <returns>
        /// 指定したスプレッドシートの指定した範囲のデータ
        /// </returns>
        public IList<IList<object>> Get(string sheetID, string range)
        {
            var request = sheetsService
                        .Spreadsheets
                        .Values
                        .Get(sheetID, range);
            var response = request.Execute();

            return response.Values;
        }
    }
}