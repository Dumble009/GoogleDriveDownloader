using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// スプレッドシートから読み込んだデータをJSON形式の文字列に変換する
    /// </summary>
    public class JSONConverter : ISheetDataConverter
    {
        public List<byte> Convert(SheetData sheetData)
        {
            throw new System.NotImplementedException();
        }
    }
}