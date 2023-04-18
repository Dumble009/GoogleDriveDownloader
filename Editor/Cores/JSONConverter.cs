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
            // SheetData内の辞書データを取り出してJSON文字列へ変換
            var convertTarget = sheetData.Data;
            var jsonString = JsonConvert.SerializeObject(convertTarget);

            // 得られたJSON文字列をバイト列へ変換
            var jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

            // 配列をListに変換して返却
            return new List<byte>(jsonBytes);
        }
    }
}