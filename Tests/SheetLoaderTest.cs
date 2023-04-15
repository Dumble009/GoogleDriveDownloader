using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// SheetLoaderクラスの挙動を確認するためのテスト
/// </summary>
public class SheetLoaderTest
{

    /// <summary>
    /// このファイルからコンフィグファイルまでの相対パス
    /// </summary>
    const string RELATIVE_CONFIG_PATH = "../Config/Config.json";

    /// <summary>
    /// コンフィグファイル内の、IDを表すデータのキー
    /// </summary>
    const string ID_KEY = "ID";

    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    SheetLoader sheetLoader;

    [SetUp]
    public void Setup()
    {
        sheetLoader = new SheetLoader(
            new SpreadSheetsService()
        );
    }

    /// <summary>
    /// 試しにメタシートを読み込ませてみて、エラーが生じたりしないか調べるテスト
    /// </summary>
    [Test]
    public void LoadMetaSheetTest()
    {
        // 確実に存在が保証できるのはメタシートだけなので、
        // メタシートに対して読み込みテストを行う。
        // メタシートの中身までは保証できないので、中身の確認は行わない。

        var thisDirectoryPath = new SourceCodeLocator().GetDirectoryOfSourceCodePath();
        var configFilePath = Path.Combine(thisDirectoryPath, RELATIVE_CONFIG_PATH);
        var fileContent = File.ReadAllText(configFilePath);
        var configContent = JsonConvert
        .DeserializeObject<Dictionary<string, string>>(fileContent);
        var sheetID = configContent[ID_KEY];

        sheetLoader.LoadSheetData(sheetID);
    }
}
