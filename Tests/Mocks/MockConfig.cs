using GoogleDriveDownloader;

public class MockConfig : IConfig
{
    string exportRootPath;
    /// <summary>
    /// GetExportRootPath関数で返されるエクスポート先のルートパス
    /// </summary>
    public string ExportRootPath
    {
        set => exportRootPath = value;
    }

    string metaSheetID;
    /// <summary>
    /// GetMetaSheetID関数で返されるメタシートのID
    /// </summary>
    public string MetaSheetID
    {
        set => metaSheetID = value;
    }

    public string GetExportRootPath()
    {
        return exportRootPath;
    }

    public string GetMetaSheetID()
    {
        return metaSheetID;
    }
}
