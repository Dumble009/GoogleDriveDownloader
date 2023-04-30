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

    string passedSheetID;
    /// <summary>
    /// LoadSheetData関数に引数として渡された、シートのID
    /// </summary>
    public string PassedSheetID
    {
        get => passedSheetID;
    }

    string passedSheetName;
    /// <summary>
    /// LoadSheetData関数に引数として渡された、スプレッドシート内のシート名
    /// </summary>
    public string PassedSheetName
    {
        get => passedSheetName;
    }

    public SheetData LoadSheetData(string sheetID, string sheetName)
    {
        passedSheetID = sheetID;
        return sheet;
    }
}
