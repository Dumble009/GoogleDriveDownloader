using System.IO;
using System.Diagnostics;
using GoogleDriveDownloader;

/// <summary>
/// Tests以下のダミーの設定ファイル等が格納されているフォルダのパスを返すモッククラス
/// </summary>
public class MockSystemFileLocator : ISystemFileLocator
{
    public string GetConfigFolderPath()
    {
        // このファイルは`Tests/Mocks`以下にあるので、
        // 一階層上の`DummyResources`の下に設定フォルダ等がある

        var frame = new StackFrame(true);
        var thisCodeDirName = Path.GetDirectoryName(frame.GetFileName());

        return Path.Combine(thisCodeDirName, "..", "DummyResources", "Config");
    }

    public string GetCredentialsFolderPath()
    {
        throw new System.NotImplementedException();
    }
}
