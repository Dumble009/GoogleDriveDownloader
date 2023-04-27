using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

public class LoadMetaSheetDataFunctionTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    LoadMetaSheetDataFunction target;

    /// <summary>
    /// MetaSheetLoaderのモックオブジェクト
    /// </summary>
    MockMetaSheetLoader metaSheetLoader;

    /// <summary>
    /// リロードボタンを持ったUIのモック
    /// </summary>
    MockReloadUI reloadUI;

    /// <summary>
    /// メタシートの読み込み結果を受け取るUIのモック
    /// </summary>
    MockSheetListUI sheetListUI1, sheetListUI2;

    [SetUp]
    public void Startup()
    {
        metaSheetLoader = new MockMetaSheetLoader();
        target = new LoadMetaSheetDataFunction(metaSheetLoader);

        reloadUI = new MockReloadUI();
        sheetListUI1 = new MockSheetListUI();
        sheetListUI2 = new MockSheetListUI();
    }

    /// <summary>
    /// UI要素をtargetに渡す共通処理
    /// </summary>
    private void PassUIElements()
    {
        target.SetUIElements(
            new List<IUIElement>()
            {
                reloadUI,
                sheetListUI1,
                sheetListUI2
            }
        );
    }

    /// <summary>
    /// テスト用のメタシートデータを作成する
    /// </summary>
    /// <returns>
    /// MetaSheetDataを3つ含むList。呼び出すたびに異なるインスタンスが作られる
    /// </returns>
    private List<MetaSheetData> CreateMetaSheetData()
    {
        var data1 = new MetaSheetData(
            1,
            "SheetID1",
            "SheetName1",
            "SavePath1",
            "DisplayName1"
        );

        var data2 = new MetaSheetData(
            2,
            "SheetID2",
            "SheetName2",
            "SavePath2",
            "DisplayName2"
        );

        var data3 = new MetaSheetData(
            3,
            "SheetID3",
            "SheetName3",
            "SavePath3",
            "DisplayName3"
        );

        return new List<MetaSheetData>() {
            data1, data2, data3
        };
    }

    /// <summary>
    /// メタシートのダミーデータを作成し、MetaSheetLoaderのモックにデータを渡す
    /// </summary>
    /// <returns>
    /// 作成されたメタシートのデータ
    /// </returns>
    private List<MetaSheetData> CreateAndPassMetaSheetDataToMock()
    {
        var metaSheetDatas = CreateMetaSheetData();

        metaSheetLoader.MetaSheetDatas = metaSheetDatas;

        return metaSheetDatas;
    }

    /// <summary>
    /// 2つのMetaSheetDataのリストが保持している全ての値が一致しているか調べる
    /// </summary>
    /// <param name="expected">
    /// 想定される値
    /// </param>
    /// <param name="actual">
    /// テスト対象から得られた値
    /// </param>
    private void AssertMetaDataLists(
        List<MetaSheetData> expected,
        List<MetaSheetData> actual
    )
    {
        Assert.NotNull(expected);
        Assert.NotNull(actual);
        Assert.AreEqual(expected.Count, actual.Count);

        int count = expected.Count;

        for (int i = 0; i < count; i++)
        {
            TestUtil.AssertAreEqualMetaSheetData(
                expected[i],
                actual[i]
            );
        }
    }

    /// <summary>
    /// リロードボタンを押し、全てのSheetListUIに想定通りの
    /// MetaSheetDataのリストが渡されているか確認する処理
    /// </summary>
    /// <param name="reloadUI">
    /// リロードイベントを発行するUIオブジェクト
    /// </param>
    /// <param name="expectedMetaSheetDatas">
    /// 各SheetListUIに渡されるべきMetaSheetDataのリスト
    /// </param>
    /// <param name="sheetListUIs">
    /// 渡された値を確認する対象のSheetListUI
    /// </param>
    private void PushRealodButtonThenCheckSheetList(
        MockReloadUI reloadUI,
        List<MetaSheetData> expectedMetaSheetDatas,
        params MockSheetListUI[] sheetListUIs
    )
    {
        reloadUI.Reload();

        foreach (var sheetListUI in sheetListUIs)
        {
            AssertMetaDataLists(
                expectedMetaSheetDatas,
                sheetListUI.PassedMetaSheetData
            );
        }
    }



    /// <summary>
    /// リロード操作をしたらMetaSheetLoaderが読み込んだメタデータがそのまま
    /// SheetListUIに渡されるか調べるテスト
    /// </summary>
    [Test]
    public void PassThroughMetaSheetDataTest()
    {
        PassUIElements();

        var passedMetaSheetDatas = CreateAndPassMetaSheetDataToMock();

        PushRealodButtonThenCheckSheetList(
            reloadUI,
            passedMetaSheetDatas,
            sheetListUI1,
            sheetListUI2
        );
    }

    /// <summary>
    /// 途中で新たなUIを追加しても正常に動作するか確認するテスト
    /// </summary>
    [Test]
    public void AddNewElementTest()
    {
        PassUIElements();

        var reloadUI2 = new MockReloadUI();
        var sheetListUI3 = new MockSheetListUI();

        var newUIs = new List<IUIElement>() {
            reloadUI,
            reloadUI2,
            sheetListUI1,
            sheetListUI2,
            sheetListUI3
        };

        target.SetUIElements(newUIs);

        // 古いSheetListにも新しく追加したSheetListにもデータが渡されるか
        // 古いRealodUIにも新しく追加したReloadUIにも反応できるか

        var passedMetaSheetDatas = CreateAndPassMetaSheetDataToMock();

        PushRealodButtonThenCheckSheetList(
            reloadUI,
            passedMetaSheetDatas,
            sheetListUI1,
            sheetListUI2,
            sheetListUI3
        );

        PushRealodButtonThenCheckSheetList(
            reloadUI2,
            passedMetaSheetDatas,
            sheetListUI1,
            sheetListUI2,
            sheetListUI3
        );
    }
}
