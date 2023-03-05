using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// メタシートを読み込むクラスMetaSheetLoaderをテストするクラス
/// </summary>
public class MetaSheetLoaderTest
{
    // private関数を呼び出す際は関数名の文字列が必要になるので、定数で宣言しておく

    /// <summary>
    /// コンフィグファイルのパスを計算する関数の名前
    /// </summary>
    const string FUNC_NAME_GET_CONFIG_PATH = "GetConfigPath";

    /// <summary>
    /// メタシートのIDを読み込む関数の名前
    /// </summary>
    const string FUNC_NAME_LOAD_META_SHEET_ID = "LoadMetaSheetID";

    /// <summary>
    /// メタシートをSheetDataに変換する関数の名前
    /// </summary>
    const string FUNC_NAME_LOAD_META_SHEET_AS_SHEET_DATA = "LoadMetaSheetAsSheetData";

    /// <summary>
    /// 設定ファイルから読み込まれることが期待されるメタシートのID
    /// </summary>
    const string EXPECT_META_SHEET_ID = "1_wHr6TWOv2NeckBCXHOMv7qrnQCcXf3BWyeaf9OTQew";

    /// <summary>
    /// 1列目のIDを除いたメタシートのパラメータの種類の数
    /// </summary>
    const int META_SHEET_PARAMETER_COUNT = 4;

    /// <summary>
    /// メタシートに存在する事が期待されるパラメータの名前の一覧。
    /// これらすべてが存在しなくてはならない
    /// </summary>
    static readonly List<string> EXPECT_PARAMETER_NAMES = new List<string>(){
        "SheetID",
        "SheetName",
        "SavePath",
        "DisplayName"
    };

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
    public void CalcConfigFilePathTest()
    {
        // 設定ファイルのパスを計算する関数はprivate
        object retVal = TestUtil.CallPrivateMethod(
            metaSheetLoader,
            "GetConfigPath",
            null
        );

        string retPath = (string)retVal;

        // 返り値が指すパスが存在すればOK
        Assert.True(File.Exists(retPath));
    }

    /// <summary>
    /// MetaSheetLoaderが正しく設定ファイル中のメタシートのIDを取得できるか確認するテスト
    /// </summary>
    [Test]
    public void LoadMetaSheetIDTest()
    {
        // 設定ファイルを読み込む関数はprivate
        object retVal = TestUtil.CallPrivateMethod(
            metaSheetLoader,
            "LoadMetaSheetID",
            null
        );

        string retID = (string)retVal;

        Assert.AreEqual(EXPECT_META_SHEET_ID, retID);
    }

    /// <summary>
    /// MetaSheetLoaderが正しくメタシートをSheetDataに変換できるか確認する
    /// </summary>
    [Test]
    public void LoadMetaSheeetAsSheetDataTest()
    {
        // メタシートをSheetDataに変換する関数はprivate
        object retVal = TestUtil.CallPrivateMethod(
            metaSheetLoader,
            "LoadMetaSheetAsSheetData",
            null
        );

        var sheetData = (SheetData)retVal;

        // メタシートの中身は開発と共に変化していくので、テスト対象はパラメータの種類のみ
        // メタシートには1つ以上のデータが存在すると仮定している
        var topRow = sheetData.GetRow("1");

        // パラメータの種類の数が期待と一致していて、
        // 存在が期待されるパラメータが全て存在していればOK
        Assert.AreEqual(META_SHEET_PARAMETER_COUNT, topRow.Count);
        foreach (var parameterName in topRow.Keys)
        {
            Assert.True(EXPECT_PARAMETER_NAMES.Contains(parameterName));
        }
    }
}
