using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    public interface ISpreadSheetsService
    {
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
        IList<IList<object>> Get(string sheetID, string range);
    }
}