using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// メタシートを読み込むクラスMetaSheetLoaderをテストするクラス
/// </summary>
public class MetaSheetLoaderTest
{
    /// <summary>
    /// ダミーのコンフィグファイルから読み込まれる事が期待される
    /// メタシートのID
    /// </summary>
    const string EXPECTED_META_SHEET_ID = "dummy-meta-sheet-id";

    /// <summary>
    /// このファイルからダミーのコンフィグファイルが格納されている
    /// ディレクトリまでの相対パス
    /// </summary>
    const string DUMMY_CONFIG_RELATIVE_PATH = "DummyResources/Config";

    /// <summary>
    /// メタシートにおける、シートIDのパラメータ名
    /// </summary>
    const string META_SHEET_PARAMETER_NAME_SHEET_ID = "SheetID";

    /// <summary>
    /// メタシートにおける、スプレッドシート内の参照シートの名前のパラメータ名
    /// </summary>
    const string META_SHEET_PARAMETER_NAME_SHEET_NAME = "SheetName";

    /// <summary>
    /// メタシートにおける、シートからダウンロードした結果を保存するパスのパラメータ名
    /// </summary>
    const string META_SHEET_PARAMETER_NAME_SAVE_PATH = "SavePath";

    /// <summary>
    /// メタシートにおける、データの表示名のパラメータ名
    /// </summary>
    const string META_SHEET_PARAMETER_NAME_DISPLAY_NAME = "DisplayName";

    /// <summary>
    /// targetに読み込ませるメタシートに含まれるシートIDの値の接頭辞
    /// </summary>
    const string SHEET_ID_HEADER = "sheetID";

    /// <summary>
    /// targetに読み込ませるメタシートに含まれるシート名の値の接頭辞
    /// </summary>
    const string SHEET_NAME_HEADER = "sheetName";

    /// <summary>
    /// targetに読み込ませるメタシートに含まれる保存場所の値の接頭辞
    /// </summary>
    const string SAVE_PATH_HEADER = "savePath";

    /// <summary>
    /// targetに読み込ませるメタシートに含まれる表示名の値の接頭辞
    /// </summary>
    const string DISPLAY_NAME_HEADER = "displayName";

    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    MetaSheetLoader target;

    /// <summary>
    /// targetに渡すISheetLoaderのモックオブジェクト
    /// </summary>
    MockSheetLoader mockSheetLoader;

    /// <summary>
    /// targetに渡すISourceCodeLocatorのモックオブジェクト
    /// </summary>
    MockSourceCodeLocator mockSourceCodeLocator;

    [SetUp]
    public void Setup()
    {
        mockSheetLoader = new MockSheetLoader();
        mockSourceCodeLocator = new MockSourceCodeLocator();

        target = new MetaSheetLoader(mockSheetLoader, mockSourceCodeLocator);


        // MetaSheetLoaderがダミーのコンフィグファイルを読み込むようにパスを作成
        // MetaSheetLoaderから本物のコンフィグファイルまでの相対パスは"../Config/Config.json"なので
        // モックのSourceCodeLocatorで、ダミーのコンフィグフォルダのパスをMetaSheetLoaderのパスとして返せば
        // ダミーのConfig.jsonを読み込む
        var frame = new StackFrame(true);
        var dirPathOfThisFile = Path.GetDirectoryName(frame.GetFileName());
        var dummyConFigPath = Path.Combine(
            dirPathOfThisFile,
            DUMMY_CONFIG_RELATIVE_PATH
        );
        mockSourceCodeLocator.ReturnPath = dummyConFigPath;
    }

    /// <summary>
    /// dataCount個のメタデータを作成して、
    /// targetがそのデータを正しく変換することが出来るか調べる処理
    /// </summary>
    /// <param name="dataCount">
    /// テストで作成、検証するメタデータの個数
    /// </param>
    private void TestBody(int dataCount)
    {
        // dataCount個のメタデータを作成
        SheetData sheetData = new SheetData();
        for (int i = 1; i <= dataCount; i++)
        {
            sheetData.SetRow(
                i.ToString(),
                new Dictionary<string, string>(){
                    {META_SHEET_PARAMETER_NAME_SHEET_ID, SHEET_ID_HEADER + i},
                    {META_SHEET_PARAMETER_NAME_SHEET_NAME, SHEET_NAME_HEADER + i},
                    {META_SHEET_PARAMETER_NAME_SAVE_PATH, SAVE_PATH_HEADER + i},
                    {META_SHEET_PARAMETER_NAME_DISPLAY_NAME, DISPLAY_NAME_HEADER + i}
                }
            );
        }

        mockSheetLoader.Sheet = sheetData;

        // 全てのメタデータが想定通りになっているか確認
        var metaSheetDatas = target.LoadMetaSheet();

        Assert.AreEqual(dataCount, metaSheetDatas.Count);
        Assert.AreEqual(EXPECTED_META_SHEET_ID, mockSheetLoader.LastPassedSheetID);
        for (int i = 0; i < dataCount; i++)
        {
            int id = i + 1;
            Assert.AreEqual(id, metaSheetDatas[i].ID);
            Assert.AreEqual(SHEET_ID_HEADER + id, metaSheetDatas[i].SheetID);
            Assert.AreEqual(SHEET_NAME_HEADER + id, metaSheetDatas[i].SheetName);
            Assert.AreEqual(SAVE_PATH_HEADER + id, metaSheetDatas[i].SavePath);
            Assert.AreEqual(DISPLAY_NAME_HEADER + id, metaSheetDatas[i].DisplayName);
        }
    }

    /// <summary>
    /// 列名を除き、一つだけのデータを持つメタシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadSingleMetaData()
    {
        TestBody(1);
    }

    /// <summary>
    /// 複数のデータを持つメタシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadMultiMetaData()
    {
        TestBody(5); // ここの数は適当
    }
}
