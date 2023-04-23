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
    /// 今回のテストではSheetDataの中身は重要では無いので、全てのテストケースで使い回す
    /// </summary>
    SheetData sheetData;

    /// <summary>
    /// テスト結果のファイルが出力されるフォルダのフォルダ名
    /// </summary>
    const string TEMP_DIR_NAME = "Temp";

    [SetUp]
    public void Setup()
    {
        sheetLoader = new MockSheetLoader();
        SheetData sheetData = new SheetData();
        sheetData.SetRow(
            "key",
            new Dictionary<string, string>() {
                {"key1", "val1"}
            }
        );
        sheetLoader.Sheet = sheetData;

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
    /// <param name="expectedText">
    /// 対象のテキストファイルに書かれているべき内容。
    /// </param>
    private void AreEqualFileContentText(
        string path,
        string expectedText
    )
    {
        Assert.True(File.Exists(path));

        string actualText = File.ReadAllText(path);

        Assert.AreEqual(expectedText, actualText);
    }

    /// <summary>
    /// バイトファイルを読み込み、その内容が想定と一致しているか調べるテスト
    /// また、内容だけでなくファイル自体の存在確認も行う
    /// </summary>
    /// <param name="path">
    /// チェック対象のバイトファイルのパス
    /// </param>
    /// <param name="expectedBytes">
    /// 対象のバイトファイルに書かれているべきバイト列
    /// </param>
    private void AreEqualFileContentBytes(
        string path,
        List<byte> expectedBytes
    )
    {
        Assert.True(File.Exists(path));

        var actualBytes = File.ReadAllBytes(path);

        Assert.AreEqual(expectedBytes, actualBytes);
    }

    /// <summary>
    /// エクスポートしたいファイル名を指定すると、
    /// Temp以下にそのファイルをエクスポートするように指定したメタシートデータを作成する
    /// </summary>
    /// <param name="exportFileName">
    /// エクスポートしたいファイルのファイル名
    /// </param>
    /// <returns>
    /// Temp以下にexportFileNameのファイル名を持ったファイルをエクスポートするように
    /// SavePathが指定されているメタシートデータ
    /// </returns>
    private MetaSheetData CreateMetaSheetData(string exportFileName)
    {
        var exportPath = Path.Combine(CalcTempDirPath(), exportFileName);
        return new MetaSheetData(
            1,
            "1",
            "sheetName",
            exportPath,
            "displayName"
        );
    }

    /// <summary>
    /// targetから外部のオブジェクトに渡されるオブジェクトの値が正しいか調べる処理
    /// </summary>
    /// <param name="metaSheetData">
    /// targetから渡されるべきMetaSheetData
    /// </param>
    /// <param name="sheetData">
    /// targetから渡されるべきSheetData
    /// </param>
    private void AssertPassedDatas(
        MetaSheetData metaSheetData,
        SheetData sheetData
    )
    {
        Assert.AreEqual(metaSheetData.SheetID, sheetLoader.PassedSheetID);
        TestUtil.AssertAreEqualSheetData(sheetData, sheetDataConverter.PassedSheetData);
    }

    /// <summary>
    /// 一般的なデータを読み込んで、テキストとして一度だけエクスポートするテスト
    /// </summary>
    [Test]
    public void LoadNormalDataAndExportAsTextOnceTest()
    {
        // 読み込み対象のシートのメタデータの作成
        const string RESULT_FILE_NAME = "LoadNormalDataAndExportAsTextOnceTestResult.txt";
        var metaSheetData = CreateMetaSheetData(RESULT_FILE_NAME);

        // ファイルに書き込まれるデータの作成
        const string RESULT_TEXT = "LoadNormalDataAndExportAsTextOnceTestResult";
        var bytes = new List<byte>(Encoding.UTF8.GetBytes(RESULT_TEXT));
        sheetDataConverter.ConvertResult = bytes;

        // エクスポート操作が行われ、想定通りの振る舞いを行い、
        // 想定通りの内容がファイルに出力されるかどうかを調べる
        sheetExportUI.Export(metaSheetData);

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentText(metaSheetData.SavePath, RESULT_TEXT);
    }

    /// <summary>
    /// 一般的なデータを読み込んで、バイト列として同じファイルに複数回エクスポートするテスト
    /// また、エクスポートとエクスポートの間にUIの追加を行い、それに正しく対応できるか調べる
    /// </summary>
    [Test]
    public void LoadNormalDataAndExportAsBytesMultipleWithMultiUITest()
    {
        // 読み込み対象のシートのメタデータの作成
        const string RESULT_FILE_NAME = "LoadNormalDataAndExportAsBytesMultipleWithMultiUITestResult";
        var metaSheetData = CreateMetaSheetData(RESULT_FILE_NAME);

        // ファイルに書き込まれるデータの作成
        var resultBytes = new List<byte>() { 1, 2, 3 };
        sheetDataConverter.ConvertResult = resultBytes;

        // 1回目のエクスポート操作
        sheetExportUI.Export(metaSheetData);

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);

        // 2回目のエクスポート操作の前にUIを新たに追加し、
        // そのUIからエクスポート指示を送ることが出来るか調べる
        var sheetExportUI2 = new MockSheetExportUI();
        target.SetUIElements(new List<IUIElement>() {
            sheetExportUI,
            sheetExportUI2
        });

        // 書き込み対象のデータを更新する。
        // そうでないと、上書き更新が行われたのか何も行われなかったのか判断がつかないため
        // また、すでに存在するファイルに書き込みが出来るか調べるために、書き込み先のパスは更新しない
        resultBytes = new List<byte>() { 4, 5, 6 };
        sheetDataConverter.ConvertResult = resultBytes;

        // 2回目のエクスポート操作
        sheetExportUI2.Export(metaSheetData);

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);

        // 最初に使用したUIからも依然エクスポート操作を行えるか確認する
        // 先ほどと同様に、書き込み対象のデータを更新する。
        resultBytes = new List<byte>() { 7, 8, 9 };
        sheetDataConverter.ConvertResult = resultBytes;

        // 3回目のエクスポート操作
        sheetExportUI.Export(metaSheetData);

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);
    }
}
