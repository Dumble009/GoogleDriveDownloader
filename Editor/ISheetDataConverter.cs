using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// SheetDataオブジェクトを、JSONやCSV等のテキスト形式に変換する
    /// </summary>
    public interface ISheetDataConverter
    {
        /// <summary>
        /// シートデータを何らかの形式に変換し、そのバイト配列を返す。
        /// </summary>
        /// <param name="sheetData">変換対象のシートデータ。非nullであること</param>
        /// <returns>
        /// sheetDataを変換した結果得られたバイト配列。そのままファイルへ書き込んだりすることが出来る。
        /// 文字列に該当するバイト列の場合、その文字コードはUTF-8
        /// エンディアンはリトルエンディアン
        /// </returns>
        List<byte> Convert(SheetData sheetData);
    }
}