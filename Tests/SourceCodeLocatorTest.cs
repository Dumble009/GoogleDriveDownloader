using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GoogleDriveDownloader;

public class SourceCodeLocatorTest
{

    /// <summary>
    /// このテストコードが書かれているファイルのファイル名
    /// </summary>
    const string FILE_NAME = "SourceCodeLocatorTest.cs";

    /// <summary>
    /// このテストコードのパスを正しく計算する事が出来るか調べるテスト
    /// </summary>
    [Test]
    public void GetSourceCodePathTest()
    {
        // このテストコードのファイルパスを取得し、ファイル名が正しいか、
        // そのパスにちゃんとファイルが存在しているかを確認
        var pathOfThisFile = SourceCodeLocator.GetSourceCodePath();

        var fileName = Path.GetFileName(pathOfThisFile);
        Assert.AreEqual(fileName, FILE_NAME);
        Assert.True(File.Exists(pathOfThisFile));
    }
}
