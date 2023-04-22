using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// ISheetUIインタフェースのモッククラス
/// UpdateList関数に渡されたリストを保持しておき外部に公開する機能と、
/// 任意のMetaSheetDataを引数として、シートのエクスポート操作をエミュレートすることが出来る
/// </summary>
public class MockSheetListUI : ISheetListUI, IUIElement
{
    List<MetaSheetData> passedMetaSheetData;
    /// <summary>
    /// UpdateList関数で引数として渡されたMetaSheetDataのリスト
    /// </summary>
    public List<MetaSheetData> PassedMetaSheetData
    {
        get => passedMetaSheetData;
    }

    OnExportSheetHandler handler;

    public void UpdateList(List<MetaSheetData> metaSheetDatas)
    {
        passedMetaSheetData = metaSheetDatas;
    }

    public void RegisterOnExportSheet(OnExportSheetHandler _handler)
    {
        handler += _handler;
    }

    /// <summary>
    /// 任意のMetaSheetDataを元に、エクスポート操作が行われた際のイベントを発行する
    /// </summary>
    /// <param name="sheetData">
    /// エクスポートイベントの引数にしたいMetaSheetDataオブジェクト
    /// </param>
    public void ExportSheet(MetaSheetData sheetData)
    {
        handler(sheetData);
    }

    public void Draw()
    {
    }
}
