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
    MockSheetLoader mockSheetLoader;

    /// <summary>
    /// シートの読み込み結果を変換するオブジェクトのモック
    /// </summary>
    MockSheetDataConverter mockSheetDataConverter;

    /// <summary>
    /// エクスポート操作時のイベントを発行するUIのモック
    /// </summary>
    MockSheetExportUI mockSheetExportUI;

    /// <summary>
    /// ダミーのエクスポート先を提供するコンフィグオブジェクトのモック
    /// </summary>
    MockConfig mockConfig;

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
        mockSheetLoader = new MockSheetLoader();
        sheetData = new SheetData();
        sheetData.SetRow(
            "key",
            new Dictionary<string, string>() {
                {"key1", "val1"}
            }
        );
        mockSheetLoader.Sheet = sheetData;

        mockSheetDataConverter = new MockSheetDataConverter();

        mockSheetExportUI = new MockSheetExportUI();

        mockConfig = new MockConfig();
        mockConfig.ExportRootPath = CalcTempDirPath();

        target = new SheetExportFunction(mockSheetLoader, mockSheetDataConverter, mockConfig);

        target.SetUIElements(
            new List<IUIElement>
            {
                mockSheetExportUI
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

            var directoryPaths = Directory.GetDirectories(tempDirPath);
            foreach (var directoryPath in directoryPaths)
            {
                Directory.Delete(directoryPath, true);
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
    /// エクスポート先のルート以下の、チェック対象のテキストファイルのパス
    /// </param>
    /// <param name="expectedText">
    /// 対象のテキストファイルに書かれているべき内容。
    /// </param>
    private void AreEqualFileContentText(
        string path,
        string expectedText
    )
    {
        string fullPath = Path.Combine(mockConfig.GetExportRootPath(), path);
        Assert.True(File.Exists(fullPath));

        string actualText = File.ReadAllText(fullPath);

        Assert.AreEqual(expectedText, actualText);
    }

    /// <summary>
    /// バイトファイルを読み込み、その内容が想定と一致しているか調べるテスト
    /// また、内容だけでなくファイル自体の存在確認も行う
    /// </summary>
    /// <param name="path">
    /// エクスポート先のルート以下の、チェック対象のバイトファイルのパス
    /// </param>
    /// <param name="expectedBytes">
    /// 対象のバイトファイルに書かれているべきバイト列
    /// </param>
    private void AreEqualFileContentBytes(
        string path,
        List<byte> expectedBytes
    )
    {
        string fullPath = Path.Combine(mockConfig.GetExportRootPath(), path);
        Assert.True(File.Exists(fullPath));

        var actualBytes = File.ReadAllBytes(fullPath);

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
        return new MetaSheetData(
            1,
            "1",
            "sheetName",
            exportFileName,
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
        Assert.AreEqual(metaSheetData.SheetID, mockSheetLoader.PassedSheetID);
        TestUtil.AssertAreEqualSheetData(sheetData, mockSheetDataConverter.PassedSheetData);
    }

    /// <summary>
    /// テキスト形式でデータをエクスポートするよう指示し、
    /// 正しい内容でエクスポートされているか確認するテスト
    /// </summary>
    /// <param name="exportPath">
    /// エクスポート先のルート以下の相対パス。
    /// </param>
    /// <param name="fileContent">
    /// エクスポートするファイルに書き込むテキスト
    /// </param>
    private void TextExportBody(
        string exportPath,
        string fileContent
    )
    {
        var metaSheetData = CreateMetaSheetData(exportPath); // 読み込み対象のシートのメタデータ

        // ファイルに書き込まれるデータの作成
        var bytes = new List<byte>(Encoding.UTF8.GetBytes(fileContent));
        mockSheetDataConverter.ConvertResult = bytes;

        // エクスポート操作が行われ、想定通りの振る舞いを行い、
        // 想定通りの内容がファイルに出力されるかどうかを調べる
        mockSheetExportUI.Export(
            new List<MetaSheetData>() { metaSheetData }
        );

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentText(metaSheetData.SavePath, fileContent);
    }

    /// <summary>
    /// 一般的なデータを読み込んで、テキストとして一度だけエクスポートするテスト
    /// </summary>
    [Test]
    public void LoadNormalDataAndExportAsTextOnceTest()
    {
        TextExportBody(
            "LoadNormalDataAndExportAsTextOnceTestResult.txt",
            "LoadNormalDataAndExportAsTextOnceTestResult"
        );
    }

    /// <summary>
    /// エクスポート先のサブディレクトリ以下にファイルをエクスポートする事が出来るか確認するテスト
    /// </summary>
    [Test]
    public void LoadNormalDataAndExportToSubDirectory()
    {
        TextExportBody(
            "subdir/LoadNormalDataAndExportToSubDirectoryResult.txt",
            "LoadNormalDataAndExportToSubDirectoryResult"
        );

        // 2階層以上下に作成する事は出来るか
        TextExportBody(
            "subdir/subsubdir/LoadNormalDataAndExportToSubDirectoryResult.txt",
            "LoadNormalDataAndExportToSubDirectoryResult"
        );
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
        mockSheetDataConverter.ConvertResult = resultBytes;

        // 1回目のエクスポート操作
        mockSheetExportUI.Export(
            new List<MetaSheetData>() { metaSheetData }
        );

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);

        // 2回目のエクスポート操作の前にUIを新たに追加し、
        // そのUIからエクスポート指示を送ることが出来るか調べる
        var sheetExportUI2 = new MockSheetExportUI();
        target.SetUIElements(new List<IUIElement>() {
            mockSheetExportUI,
            sheetExportUI2
        });

        // 書き込み対象のデータを更新する。
        // また、すでに存在するファイルに書き込みが出来るか調べるために、書き込み先のパスは更新しない
        resultBytes = new List<byte>() { 4, 5, 6 };
        mockSheetDataConverter.ConvertResult = resultBytes;

        // 2回目のエクスポート操作
        sheetExportUI2.Export(
            new List<MetaSheetData>() { metaSheetData }
        );

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);

        // 最初に使用したUIからも依然エクスポート操作を行えるか確認する
        // 先ほどと同様に、書き込み対象のデータを更新する。
        resultBytes = new List<byte>() { 7, 8, 9 };
        mockSheetDataConverter.ConvertResult = resultBytes;

        // 3回目のエクスポート操作
        mockSheetExportUI.Export(
            new List<MetaSheetData>() { metaSheetData }
        );

        AssertPassedDatas(metaSheetData, sheetData);
        AreEqualFileContentBytes(metaSheetData.SavePath, resultBytes);
    }

}
