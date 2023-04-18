using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// IMetaSheetLoaderインタフェースのモッククラス
/// 事前に渡されたMetaSheetDataのリストをLoadMetaSheetで返す
/// </summary>
public class MockMetaSheetLoader : IMetaSheetLoader
{
    List<MetaSheetData> metaSheetDatas;
    /// <summary>
    /// LoadMetaSheet関数で返すメタシートのデータのリスト
    /// </summary>
    public List<MetaSheetData> MetaSheetDatas
    {
        set => metaSheetDatas = value;
    }

    public List<MetaSheetData> LoadMetaSheet()
    {
        return metaSheetDatas;
    }
}
