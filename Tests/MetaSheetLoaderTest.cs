using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GoogleDriveDownloader;

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
    /// MetaSheetLoaderが正しく設定ファイルのパスを計算できるか確認するテスト
    /// </summary>
    [Test]
    public void MetaSheetLoaderTestConfigFilePath()
    {
        // 設定ファイルを読み込む関数はprivate
        object retVal = TestUtil.CallPrivateMethod(
                                metaSheetLoader,
                                "GetConfigPath",
                                null
                        );

        string retPath = (string)retVal;

        // 返り値が指すパスが存在すればOK
        Assert.True(File.Exists(retPath));
    }
}
