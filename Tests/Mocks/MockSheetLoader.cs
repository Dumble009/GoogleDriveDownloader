using GoogleDriveDownloader;

/// <summary>
/// ISheetLoaderのモッククラス
/// 事前に設定されたSheetDataオブジェクトを返す
/// </summary>
public class MockSheetLoader : ISheetLoader
{
    SheetData sheet;
    /// <summary>
    /// LoadSheetData関数で返り値として返却するSheetDataオブジェクト
    /// </summary>
    public SheetData Sheet
    {
        set => sheet = value;
    }

    string lastPassedSheetID;
    /// <summary>
    /// LoadSheetData関数に
    /// </summary>
    public string LastPassedSheetID
    {
        get => lastPassedSheetID;
    }

    public SheetData LoadSheetData(string sheetID)
    {
        lastPassedSheetID = sheetID;
        return sheet;
    }
}
