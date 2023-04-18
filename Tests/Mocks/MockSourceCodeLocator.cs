using GoogleDriveDownloader;

/// <summary>
/// ISourceCodeLocatorインタフェースのモッククラス
/// 事前に渡されたパスをそのまま返す
/// </summary>
public class MockSourceCodeLocator : ISourceCodeLocator
{
    string returnPath;
    /// <summary>
    /// GetDirectoryOfSourceCodePathの返り値として返すパス
    /// </summary>
    public string ReturnPath
    {
        set => returnPath = value;
    }

    public string GetDirectoryOfSourceCodePath()
    {
        return returnPath;
    }
}
