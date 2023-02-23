namespace googleDriveDownloader
{
    /// <summary>
    /// SheetDataオブジェクトを、JSONやCSV等のテキスト形式に変換する
    /// </summary>
    public interface ISheetDataConverter
    {
        /// <summary>
        /// シートデータを何らかの構造のテキスト形式に変換し、その文字列を返す
        /// </summary>
        /// <param name="sheetData">変換対象のシートデータ。非nullであること</param>
        /// <returns>sheetDataを変換した結果得られた文字列。そのままファイルへ書き込んだりすることが出来る。</returns>
        string Convert(SheetData sheetData);
    }
}