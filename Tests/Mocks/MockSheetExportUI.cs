using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// ISheetExportUIのモッククラス
/// 任意のタイミングでエクスポート操作の発生をエミュレートすることが出来る
/// </summary>
public class MockSheetExportUI : ISheetExportUI, IUIElement
{
    /// <summary>
    /// エクスポート時に呼び出されるイベント
    /// </summary>
    OnExportSheetHandler handler;

    public void RegisterOnExportSheet(OnExportSheetHandler _handler)
    {
        handler += _handler;
    }

    /// <summary>
    /// エクスポート操作を行った際のイベントを発行する
    /// </summary>
    /// <param name="metaSheetDatas">
    /// エクスポート対象のシートについてのデータ
    /// </param>
    public void Export(List<MetaSheetData> metaSheetDatas)
    {
        handler(metaSheetDatas);
    }

    public void Draw()
    {
    }
}
