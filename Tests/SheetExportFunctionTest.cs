using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GoogleDriveDownloader;

public class SheetExportFunctionTest
{
    //====================================================
    // このテストでは実際にファイル出力した結果を調べる。
    // そのファイル出力はTests/Tempフォルダ内に全て出力される。
    //====================================================

    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    SheetExportFunction target;

    /// <summary>
    /// シートをドライブから読み込むオブジェクトのモック
    /// </summary>
    MockSheetLoader sheetLoader;

    /// <summary>
    /// シートの読み込み結果を変換するオブジェクトのモック
    /// </summary>
    MockSheetDataConverter sheetDataConverter;

    /// <summary>
    /// エクスポート操作時のイベントを発行するUIのモック
    /// </summary>
    MockSheetExportUI sheetExportUI;

    /// <summary>
    /// テスト結果のファイルが出力されるフォルダのフォルダ名
    /// </summary>
    const string TEMP_DIR_NAME = "Temp";

    [SetUp]
    public void Setup()
    {
        sheetLoader = new MockSheetLoader();
        sheetDataConverter = new MockSheetDataConverter();

        target = new SheetExportFunction(sheetLoader, sheetDataConverter);

        target.SetUIElements(
            new List<IUIElement>
            {
                sheetExportUI
            }
        );

        CleanTempFiles();
    }

    /// <summary>
    /// Tempフォルダのパスを計算する。
    /// </summary>
    /// <returns></returns>
    private string CalcTempDirPath()
    {
        // このソースファイルはTestsフォルダ直下に置かれている筈なので、
        // このファイルを格納しているディレクトリにTempフォルダのフォルダ名を連結すれば
        // Tempフォルダのパスになる。
        var stackFrame = new StackFrame(true);
        var testsDirPath = Path.GetDirectoryName(stackFrame.GetFileName());

        return Path.Combine(testsDirPath, TEMP_DIR_NAME);
    }

    /// <summary>
    /// テスト結果の出力フォルダを使用可能な状態にする。
    /// すでにフォルダが作られていてファイルが存在する場合はファイルを全て削除し、
    /// フォルダが存在しない場合は作成する。
    /// </summary>
    private void CleanTempFiles()
    {
        var tempDirPath = CalcTempDirPath();

        if (Directory.Exists(tempDirPath))
        {
            var filePaths = Directory.GetFiles(tempDirPath);

            foreach (var filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }
        else
        {
            Directory.CreateDirectory(tempDirPath);
        }
    }

    /// <summary>
    /// テキストファイルを読み込み、その内容が想定と一致しているか調べるテスト
    /// また、内容だけでなくファイル自体の存在確認も行う
    /// </summary>
    /// <param name="path">
    /// チェック対象のテキストファイルのパス
    /// </param>
    /// <param name="expectedContent">
    /// 対象のテキストファイルに書かれているべき内容。
    /// </param>
    private void AreEqualFileContent(
        string path,
        string expectedContent
    )
    {
        Assert.True(File.Exists(path));

        string actualContent = File.ReadAllText(path);

        Assert.AreEqual(actualContent, expectedContent);
    }

    /// <summary>
    /// 一般的なデータを読み込んでエクスポートするテスト
    /// </summary>
    [Test]
    public void LoadNormalDataAndExportTest()
    {
        // 読み込み対象のシートのメタデータの作成
        const string RESULT_FILE_NAME = "LoadNormalDataAndExportTestResult.txt";
        string resultFilePath = Path.Combine(
            CalcTempDirPath(),
            RESULT_FILE_NAME
        );

        MetaSheetData metaSheetData = new MetaSheetData(
            1,
            "1",
            "sheetName",
            resultFilePath,
            "displayName"
        );

        // 読み込み対象のシートの、シートデータの作成
        SheetData sheetData = new SheetData();
        sheetData.SetRow(
            "key",
            new Dictionary<string, string>() {
                {"key1", "val1"}
            }
        );
        sheetLoader.Sheet = sheetData;

        // ファイルに書き込まれるデータの作成
        const string RESULT_TEXT = "LoadNormalDataAndExportTest";
        var bytes = new List<byte>(Encoding.UTF8.GetBytes(RESULT_TEXT));
        sheetDataConverter.ConvertResult = bytes;

        // エクスポート操作が行われ、想定通りの振る舞いを行い、
        // 想定通りの内容がファイルに出力されるかどうか
        sheetExportUI.Export(metaSheetData);

        Assert.AreEqual(metaSheetData.SheetID, sheetLoader.PassedSheetID);
        TestUtil.AssertAreEqualSheetData(sheetData, sheetDataConverter.PassedSheetData);
        AreEqualFileContent(resultFilePath, RESULT_TEXT);
    }
}
