using System.Collections.Generic;

using Google.Apis.Sheets.v4;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// GoogleAPIを通じて、Googleドライブ上のスプレッドシートを読み込む
    /// </summary>
    public class SpreadSheetsService : ISpreadSheetsService
    {
        /// <summary>
        /// GoogleAPIが提供するスプレッドシートサービスのオブジェクト
        /// </summary>
        SheetsService sheetsService;

        public SpreadSheetsService()
        {
            sheetsService = new GoogleAuthAgent().CreateSheetsService();
        }

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