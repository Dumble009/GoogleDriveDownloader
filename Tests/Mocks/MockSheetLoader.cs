using System.Collections.Generic;
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

    /// <summary>
    /// LoadSheetData関数の引数として渡された、シートのIDのリスト
    /// </summary>
    List<string> passedSheetIDs;

    string lastPassedSheetID;
    /// <summary>
    /// LoadSheetData関数に引数として渡された、シートのID
    /// </summary>
    public string LastPassedSheetID
    {
        get => lastPassedSheetID;
    }

    string passedSheetName;
    /// <summary>
    /// LoadSheetData関数に引数として渡された、スプレッドシート内のシート名
    /// </summary>
    public string PassedSheetName
    {
        get => passedSheetName;
    }

    public MockSheetLoader()
    {
        passedSheetIDs = new List<string>();
    }

    public SheetData LoadSheetData(string sheetID, string sheetName)
    {
        lastPassedSheetID = sheetID;
        passedSheetIDs.Add(sheetID);
        return sheet;
    }

    /// <summary>
    /// あるシートIDをLoadSheetData関数の引数として渡されたかどうか
    /// </summary>
    /// <param name="sheetID">
    /// LoadSheetData関数の引数として渡されたか調べてほしいシートのID
    /// </param>
    /// <returns>
    /// sheetIDを今までにLoadSheetDataの引数として渡されていればtrue、
    /// そうでなければfalseを返す。
    /// </returns>
    public bool IsThiSheetIDPassed(string sheetID)
    {
        return passedSheetIDs.Contains(sheetID);
    }
}
