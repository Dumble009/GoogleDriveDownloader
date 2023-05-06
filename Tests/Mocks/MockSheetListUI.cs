using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// ISheetUIインタフェースのモッククラス
/// UpdateList関数に渡されたリストを保持しておき外部に公開する機能と、
/// 任意のMetaSheetDataを引数として、シートのエクスポート操作をエミュレートすることが出来る
/// </summary>
public class MockSheetListUI : IMetaSheetDataReceiveUI, IUIElement
{
    List<MetaSheetData> passedMetaSheetData;
    /// <summary>
    /// UpdateList関数で引数として渡されたMetaSheetDataのリスト
    /// </summary>
    public List<MetaSheetData> PassedMetaSheetData
    {
        get => passedMetaSheetData;
    }

    public void UpdateList(List<MetaSheetData> metaSheetDatas)
    {
        passedMetaSheetData = metaSheetDatas;
    }

    public void Draw()
    {
    }
}
