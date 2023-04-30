using System.Collections.Generic;
using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// SheetLoaderクラスの挙動を確認するためのテスト
/// </summary>
public class SheetLoaderTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    SheetLoader target;

    /// <summary>
    /// ISpreadSheetsServiceのモックオブジェクト
    /// </summary>
    MockSpreadSheetsService spreadSheetsService;

    /// <summary>
    /// テーブルの1列目にある"ID"列の名称
    /// </summary>
    const string COL_NAME_ID = "ID";


    [SetUp]
    public void Setup()
    {
        spreadSheetsService = new MockSpreadSheetsService();
        target = new SheetLoader(spreadSheetsService);
    }

    /// <summary>
    /// tableを元に正しくsheetDataに変換できたか調べる
    /// </summary>
    /// <param name="table">
    /// 変換元のテーブル。1行目は必ず列名を持っている事
    /// </param>
    /// <param name="sheetData">
    /// tableから変換されたSheetDataオブジェクト
    /// </param>
    private void AssertTable(
        List<List<string>> table,
        SheetData sheetData
    )
    {
        int rowCount = table.Count;
        int colCount = table[0].Count;
        var colNames = table[0];

        Assert.AreEqual(rowCount - 1, sheetData.Data.Count); // 1行目は列名が書かれており、SheetDataには含まれないので、行数が1つ減る
        for (int i = 1; i < rowCount; i++)
        {
            var sheetRow = sheetData.GetRow(
                table[i][0]
            );
            Assert.AreEqual(colCount - 1, sheetRow.Count); // 1列目のIDはSheetDataではキーとして扱われるので、列数が1つ減る
            for (int j = 1; j < colCount; j++)
            {
                var colName = colNames[j];
                Assert.AreEqual(table[i][j], sheetRow[colName]);
            }
        }
    }

    /// <summary>
    /// シートデータの代わりとなるリストを作成する
    /// </summary>
    /// <param name="rowCount">
    /// シートの行数。列名が記載された1行目を含む
    /// </param>
    /// <param name="colCount">
    /// シートの列数。IDが記載された1列目を含む
    /// </param>
    /// <returns></returns>
    private List<List<string>> MakeTable(
        int rowCount,
        int colCount
    )
    {
        var table = new List<List<string>>();

        // 1行目にはパラメータ名が書かれる
        table.Add(new List<string>(colCount));
        table[0].Add(COL_NAME_ID);
        for (int i = 1; i < colCount; i++)
        {
            table[0].Add("PARAM_" + i.ToString());
        }

        // テーブル本体の作成
        for (int i = 1; i < rowCount; i++)
        {
            table.Add(new List<string>(colCount));
            table[i].Add("ID_" + i.ToString());
            for (int j = 1; j < colCount; j++)
            {
                table[i].Add(i.ToString() + "_" + j.ToString());
            }
        }

        return table;
    }

    /// <summary>
    /// シートの作成、読み込み指示、各種アサートを行う各テストの共通処理
    /// </summary>
    /// <param name="rowCount">
    /// シートの行数。先頭にある列名の行を含む
    /// </param>
    /// <param name="colCount">
    /// シートの列数。ID列を含む
    /// </param>
    /// <param name="sheetID">
    /// シートのID文字列
    /// </param>
    /// <param name="sheetName">
    /// スプレッドシート内のシート名。空文字列、nullも可
    /// </param>
    private void TestBody(
        int rowCount,
        int colCount,
        string sheetID,
        string sheetName
    )
    {
        var table = MakeTable(rowCount, colCount);
        spreadSheetsService.Table = table;
        spreadSheetsService.TargetSheetName = sheetName;

        AssertTable(table, target.LoadSheetData(sheetID, sheetName));
        Assert.AreEqual(sheetID, spreadSheetsService.PassedSheetID);
    }

    /// <summary>
    /// 列名の行を含めて、3x3サイズのシートを正しく読み込めるかどうか
    /// </summary>
    [Test]
    public void LoadTest3x3()
    {
        TestBody(3, 3, "sheet3x3", "");
    }

    /// <summary>
    /// 列名の行を含めて、1000x1000サイズのシートを読み込めるか調べるテスト
    /// 1000はこのツールにおける列数の限界値
    /// 行数には限界値が無いが、ここでは1000としておく
    /// </summary>
    [Test]
    public void LoadTestMaximum()
    {
        TestBody(1000, 1000, "sheet1000x1000", "");
    }

    /// <summary>
    /// 最小サイズのシートを読み込むテスト
    /// 最小サイズは2x2(1行目は必ず列名があり、1列目は必ずIDがあるため)
    /// </summary>
    [Test]
    public void LoadTestMinimum()
    {
        TestBody(2, 2, "sheet2x2", "");
    }

    /// <summary>
    /// 行数よりも列数の方が多いシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadWideSheetTest()
    {
        TestBody(10, 100, "wideSheet", "");
    }

    /// <summary>
    /// 列数よりも行数の方が多いシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadLongSheetTest()
    {
        TestBody(100, 10, "longSheet", null);
    }

    /// <summary>
    /// スプレッドシート内のシート名まで含めてシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadWithSheetNameTest()
    {
        TestBody(3, 3, "withSheetName", "SheetName");
    }

    /// <summary>
    /// 日本語のシート名を指定してシートを読み込むテスト
    /// </summary>
    [Test]
    public void LoadWithJPSheetNameTest()
    {
        TestBody(3, 3, "withJPSheetName", "シート名");
    }
}
