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
    /// SetUIElementsでUI要素を渡す事が出来るか調べるテスト
    /// </summary>
    [Test]
    public void SetUIElementsTest()
    {
        Assert.DoesNotThrow(() =>
        {
            PassUIElements();
        });
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
            TestUtil.AssertMetaSheetData(
                expected[i],
                actual[i]
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

        var passedMetaSheetDatas = new List<MetaSheetData>()
        {
            data1,
            data2,
            data3
        };

        metaSheetLoader.MetaSheetDatas = passedMetaSheetDatas;

        reloadUI.Reload();

        AssertMetaDataLists(
            passedMetaSheetDatas,
            sheetListUI1.PassedMetaSheetData
        );

        AssertMetaDataLists(
            passedMetaSheetDatas,
            sheetListUI2.PassedMetaSheetData
        );
    }
}
