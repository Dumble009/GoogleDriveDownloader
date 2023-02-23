using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// SheetLoaderクラスの挙動を確認するためのテスト
/// </summary>
public class SheetLoaderTest
{
    // ----------------------------------------------------------
    // テスト用のシートは6列4行
    // 1列目はIDが記載され、1行目には各列の名前が記載されている
    // よって実質的には5つのパラメータを持つ3つの値を保持している
    // ----------------------------------------------------------


    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    SheetLoader sheetLoader;

    /// <summary>
    /// テスト用のデータが書かれたスプレッドシートのID
    /// </summary>
    const string TEST_SHEET_ID = "1V9Lfk4Dvm9EDkcG_xcUgSnQWtIpuL2sKk9UHxOxMzHk";

    /// <summary>
    /// テスト用のスプレッドシート上のパラメータの数
    /// </summary>
    const int TEST_SHEET_PARAMETER_COUNT = 5;

    /// <summary>
    /// テスト用のスプレッドシート上のデータの数
    /// </summary>
    const int TEST_SHEET_DATA_COUNT = 3;

    /// <summary>
    /// テスト用のスプレッドシート上のパラメータの名称
    /// </summary>
    static readonly string[] TEST_SHEET_PARAMETER_NAMES =
        new string[TEST_SHEET_PARAMETER_COUNT] {
            "NAME",
            "ATK",
            "DEF",
            "SPD",
            "HP"
        }; // const配列は作ることが出来ないので、static readonlyで代用する
    static readonly string[,] TEST_SHEET_PARAMETER_VALUES =
        new string[TEST_SHEET_DATA_COUNT, TEST_SHEET_PARAMETER_COUNT]{
            {"Tom",     "1",    "2",    "3",    "4"},
            {"Bob",     "5",    "6",    "7",    "8"},
            {"Jone",    "9",    "10",   "11",   "12"}
        };

    [SetUp]
    public void Setup()
    {
        sheetLoader = new SheetLoader();
    }

    /// <summary>
    /// 指定されたシートを読み込み、その結果が意図したとおりになっているか調べる
    /// </summary>
    /// <param name="sheetID">読み込みたいシートのID</param>
    /// <param name="parameterCount">シートから読み込みたいパラメータの数</param>
    /// <param name="expectedSheet">
    /// シートから読み込まれることが想定される内容。
    /// expectedData[i][j] = i個目のデータのj番目のパラメータの(パラメータ名、値)のタプル
    /// </param>
    private void LoadAndCheckSheet(string sheetID,
                                int parameterCount,
                                List<List<(string, string)>> expectedSheet)
    {
        var sheetData = sheetLoader.LoadSheetData(
                    sheetID,
                    parameterCount
                );

        // ちゃんと意味のある値は返ってきているか？
        Assert.NotNull(sheetData);

        int dataCount = expectedSheet.Count;
        // 各データについて
        for (int id = 1; id <= dataCount; id++)
        {
            // 適切なIDをキーとして持っているか？
            Dictionary<string, string> row = null;
            Assert.DoesNotThrow(
                () =>
                {
                    row = sheetData.GetRow(id.ToString());
                }
            );
            Assert.NotNull(row);

            // パラメータの個数はあっているか？
            Assert.AreEqual(parameterCount, row.Keys.Count);

            // パラメータの値はあっているか？
            for (int i = 0; i < parameterCount; i++)
            {
                var (expectedKey, expectedValue) = expectedSheet[id - 1][i];

                Assert.True(row.ContainsKey(expectedKey));
                Assert.AreEqual(expectedValue, row[expectedKey]);
            }
        }
    }

    /// <summary>
    /// 想定解を作成する
    /// </summary>
    /// <param name="dataCount">想定解に含まれるデータの個数</param>
    /// <param name="parameterCount">想定解を構成するパラメータの数</param>
    /// <returns>dataCount個のデータを持ち、parameterCount個のパラメータからなる想定解</returns>
    private List<List<(string, string)>> CreateExpectedSheet(
        int dataCount,
        int parameterCount
    )
    {
        var expectedData = new List<List<(string, string)>>();
        for (int dataID = 0; dataID < dataCount; dataID++)
        {
            var expectedRow = new List<(string, string)>();
            for (int parameterID = 0; parameterID < parameterCount; parameterID++)
            {
                expectedRow.Add(
                    (TEST_SHEET_PARAMETER_NAMES[parameterID],
                    TEST_SHEET_PARAMETER_VALUES[dataID, parameterID])
                );
            }

            expectedData.Add(expectedRow);
        }

        return expectedData;
    }

    /// <summary>
    /// シート全体のデータを正しく読み込めるか確認するテスト
    /// </summary>
    [Test]
    public void SheetLoaderTestLoadAll()
    {
        var expectedData = CreateExpectedSheet(TEST_SHEET_DATA_COUNT, TEST_SHEET_PARAMETER_COUNT);

        LoadAndCheckSheet(TEST_SHEET_ID, TEST_SHEET_PARAMETER_COUNT, expectedData);
    }

    /// <summary>
    /// シートの一部のデータのみを正しく読み込めるか確認するテスト
    /// </summary>
    [Test]
    public void SheetLoaderTestLoadPartial()
    {
        int dataCount = TEST_SHEET_PARAMETER_COUNT / 2;
        int parameterCount = TEST_SHEET_PARAMETER_COUNT / 2;

        var expectedData = CreateExpectedSheet(dataCount, parameterCount);

        LoadAndCheckSheet(TEST_SHEET_ID, parameterCount, expectedData);
    }
}
