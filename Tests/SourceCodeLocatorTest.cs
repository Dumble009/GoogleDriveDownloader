using System.IO;
using NUnit.Framework;
using GoogleDriveDownloader;

public class SourceCodeLocatorTest
{

    /// <summary>
    /// このテストコードが書かれているファイルのファイル名
    /// </summary>
    const string FILE_NAME = "SourceCodeLocatorTest.cs";

    /// <summary>
    /// SourceCodeLocatorが返すパスを元に、
    /// このテストコードのパスを正しく計算する事が出来るか調べるテスト
    /// </summary>
    [Test]
    public void GetSourceCodePathTest()
    {
        // SourceCodeLocatorは、このファイルが格納されているディレクトリのパスを
        // 返してくれるので、ちゃんと存在するディレクトリを返してくれるか、
        // そのディレクトリの中にこのファイルが格納されているかを確かめる
        var pathOfParentDirectory = SourceCodeLocator.GetDirectoryOfSourceCodePath();
        Assert.True(Directory.Exists(pathOfParentDirectory));

        var thisFilePath = Path.Combine(pathOfParentDirectory, FILE_NAME);
        Assert.True(File.Exists(thisFilePath));
    }
}
