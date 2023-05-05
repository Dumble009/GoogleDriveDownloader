using System.IO;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GoogleDriveDownloader;

public class SystemFileLocatorForUnityTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    SystemFileLocatorForUnity target;

    [SetUp]
    public void Setup()
    {
        target = new SystemFileLocatorForUnity();
    }

    /// <summary>
    /// プロジェクトルートからの相対パスが、想定通りの場所を指しているか調べるテストの共通処理
    /// 文字列の比較ではなく、パスとして等価かどうかを調べる
    /// </summary>
    /// <param name="expected">
    /// プロジェクトルートからの相対パスとして想定される値
    /// </param>
    /// <param name="actual">
    /// 実際に取得したプロジェクトルートからの相対パス
    /// </param>
    private void PathCompareTestBody(
        string expected,
        string actual
    )
    {
        var projectRootAbsPath = Path.Combine(Application.dataPath, "..");

        // 相対パスのままではパスとしての等価性を判定する事が出来ないので、
        // プロジェクトルートの絶対パスと結合して、絶対パスに直す必要がある。
        var expectedAbsPath = Path.Combine(projectRootAbsPath, expected);
        var actualAbsPath = Path.Combine(projectRootAbsPath, actual);

        Assert.That(expectedAbsPath, Is.SamePath(actualAbsPath));
    }

    /// <summary>
    /// コンフィグファイルが格納されているフォルダのパスが正しいか確認するテスト
    /// </summary>
    [Test]
    public void GetConfigFolderPathTest()
    {
        PathCompareTestBody("Cofig", target.GetConfigFolderPath());
    }

    /// <summary>
    /// 認証情報ファイルが格納されているフォルダのパスを取得するテスト
    /// </summary>
    [Test]
    public void GetCredentialsFolderPathTest()
    {
        PathCompareTestBody("Credentials", target.GetCredentialsFolderPath());
    }
}
