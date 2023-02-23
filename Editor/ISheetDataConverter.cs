namespace googleDriveDownloader
{
    /// <summary>
    /// SheetDataオブジェクトを、JSONやCSV等のテキスト形式に変換する
    /// </summary>
    public interface ISheetDataConverter
    {
        string Convert(SheetData sheetData);
    }
}