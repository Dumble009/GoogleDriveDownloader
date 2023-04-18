using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

public class MetaSheetLoaderAdapterTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    MetaSheetLoaderAdapter target;

    /// <summary>
    /// UI側の動作を模倣するモッククラス
    /// </summary>
    MockUILoadMetaSheetDataFunction mockUI;

    /// <summary>
    /// メタシートの読み込み機能を模倣するモッククラス
    /// </summary>
    MockMetaSheetLoader mockMetaSheetLoader;

    [SetUp]
    public void Setup()
    {
        mockUI = new MockUILoadMetaSheetDataFunction();
        mockMetaSheetLoader = new MockMetaSheetLoader();
        target = new MetaSheetLoaderAdapter(
            mockUI,
            mockMetaSheetLoader
        );
    }

    /// <summary>
    /// 2つのMetaSheetDataオブジェクトが一致しているか確認しアサーションする関数。
    /// 2つが一致していればパスする
    /// </summary>
    /// <param name="expected">
    /// 想定される正しい値
    /// </param>
    /// <param name="actual">
    /// 実際にMetaSheetLoaderAdapterから返ってきた値
    /// </param>
    void AssertMetaSheetData(
        MetaSheetData expected,
        MetaSheetData actual
    )
    {
        Assert.AreEqual(expected.ID, actual.ID);
        Assert.AreEqual(expected.SheetID, actual.SheetID);
        Assert.AreEqual(expected.SheetName, actual.SheetName);
        Assert.AreEqual(expected.SavePath, actual.SavePath);
        Assert.AreEqual(expected.DisplayName, actual.DisplayName);
    }

    /// <summary>
    /// mockMetaSheetLoaderが作成したメタシートのデータがそのままmockUIに渡されるか調べるテスト
    /// </summary>
    [Test]
    public void PassMetaSheetDatasTest()
    {
        // テスト対象のデータの作成
        var datas = new List<MetaSheetData>();

        const int ID_1 = 1;
        const string SHEET_ID_1 = "SheetId1";
        const string SHEET_NAME_1 = "SheetName1";
        const string SAVE_PATH_1 = "SavePath1";
        const string DISPLAY_NAME_1 = "DisplayName1";
        datas.Add(new MetaSheetData(
            ID_1,
            SHEET_ID_1,
            SHEET_NAME_1,
            SAVE_PATH_1,
            DISPLAY_NAME_1
        ));

        const int ID_2 = 2;
        const string SHEET_ID_2 = "SheetId2";
        const string SHEET_NAME_2 = "SheetName2";
        const string SAVE_PATH_2 = "SavePath2";
        const string DISPLAY_NAME_2 = "DisplayName2";
        datas.Add(new MetaSheetData(
            ID_2,
            SHEET_ID_2,
            SHEET_NAME_2,
            SAVE_PATH_2,
            DISPLAY_NAME_2
        ));

        mockMetaSheetLoader.MetaSheetDatas = datas;

        // UI側からロード要求が来たら、
        // UIにmockMetaSheetLoaderに渡したのと同じメタシートのデータが届くはず
        mockUI.Load();
        var actualDatas = mockUI.PassedMetaSheetDatas;
        Assert.NotNull(actualDatas);
        Assert.AreEqual(datas.Count, actualDatas.Count);
        for (int i = 0; i < datas.Count; i++)
        {
            AssertMetaSheetData(datas[i], actualDatas[i]);
        }
    }

    /// <summary>
    /// 要素数0のリストが渡された際に正しく振舞えるか確認するテスト
    /// </summary>
    [Test]
    public void PassEmptyDatasTest()
    {
        // 要素数0のリストが渡された場合も、それをそのままUI側に渡してほしい

        mockMetaSheetLoader.MetaSheetDatas = new List<MetaSheetData>();

        mockUI.Load();
        var actualDatas = mockUI.PassedMetaSheetDatas;
        Assert.NotNull(actualDatas);
        Assert.AreEqual(0, actualDatas.Count);
        // 要素数が0であれば、中身は当然調べなくても良い
    }
}
