using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// メタシートを読み込むクラスMetaSheetLoaderをテストするクラス
/// </summary>
public class MetaSheetLoaderTest
{

    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    MetaSheetLoader metaSheetLoader;

    [SetUp]
    public void Setup()
    {
        metaSheetLoader = new MetaSheetLoader();
    }

    /// <summary>
    /// MetaSheetLoaderがメタシートを読み込めるか調べるテスト。
    /// </summary>
    [Test]
    public void LoadMetaSheetTest()
    {
        // メタシートの中身は環境によって異なるので、
        // 途中で失敗してnullが返ってこないかだけ調べる

        var metaSheetData = metaSheetLoader.LoadMetaSheet();
        Assert.IsNotNull(metaSheetData);
    }
}
