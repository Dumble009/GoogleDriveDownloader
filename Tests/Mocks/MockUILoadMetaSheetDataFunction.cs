using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// IUILoadMetaSheetDataFunctionのモッククラス
/// PassNewMetaSheetDatasで渡されたMetaSheetDataのリストを外部に公開する。
/// また、RegisterOnTriggeredで渡されたイベントを任意のタイミングで呼び出す事が出来る。
/// </summary>
public class MockUILoadMetaSheetDataFunction : IUILoadMetaSheetDataFunction
{
    List<MetaSheetData> passedMetaSheetDatas;
    /// <summary>
    /// PassNewMetaSheetDatas関数で引数として渡されたMetaSheetDataのリスト
    /// </summary>
    public List<MetaSheetData> PassedMetaSheetDatas
    {
        get => passedMetaSheetDatas;
    }

    UILoadMetaSheetDataFunctionHandler handlers;

    public void PassNewMetaSheetDatas(List<MetaSheetData> metaSheetDatas)
    {
        passedMetaSheetDatas = metaSheetDatas;
    }

    public void RegisterOnTriggered(UILoadMetaSheetDataFunctionHandler _handler)
    {
        handlers += _handler;
    }

    /// <summary>
    /// メタシートの読み込み指示。
    /// この関数を呼ぶと、PassedMetaSheetDatasにメタシートのデータのリストが渡されるはず
    /// </summary>
    public void Load()
    {
        handlers();
    }
}
