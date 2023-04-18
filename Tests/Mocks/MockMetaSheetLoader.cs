using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// IMetaSheetLoaderインタフェースのモッククラス
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
