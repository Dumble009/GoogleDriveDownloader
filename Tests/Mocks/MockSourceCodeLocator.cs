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
    string ReturnPath
    {
        get => returnPath;
    }

    public string GetDirectoryOfSourceCodePath()
    {
        return returnPath;
    }
}
