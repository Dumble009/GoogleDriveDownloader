using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using GoogleDriveDownloader;

public class ConfigTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    Config target;

    const string DUMMY_CONFIG_RELATIVE_PATH = "DummyResources/Config/DummyFolder";

    [SetUp]
    public void Setup()
    {
        // ダミーのコンフィグファイルを読み込ませるために、
        // MockSourceCodeLocatorが返すパスを作成
        var frame = new StackFrame(true);
        var dirPathOfThisFile = Path.GetDirectoryName(frame.GetFileName());
        var dummyConFigPath = Path.Combine(
            dirPathOfThisFile,
            DUMMY_CONFIG_RELATIVE_PATH
        );
        var mockSystemFileLocator = new MockSystemFileLocator();

        target = new Config(mockSystemFileLocator);
    }

    /// <summary>
    /// ダミーのコンフィグファイルを読み込んだ結果が想定通りになっているか調べるテスト
    /// </summary>
    [Test]
    public void LoadDummyConfigAndCheckTest()
    {
        const string EXPECTED_META_SHEET_ID = "dummy-meta-sheet-id";

        Assert.AreEqual(EXPECTED_META_SHEET_ID, target.GetMetaSheetID());

        string expectedExportRootPath = Path.Combine(Application.dataPath, "/DummyFolder");

        Assert.That(expectedExportRootPath, Is.SamePath(target.GetExportRootPath()));
    }
}
