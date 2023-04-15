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

        for (int i = 1; i < rowCount; i++)
        {
            var sheetRow = sheetData.GetRow(
                table[i][0]
            );
            for (int j = 1; j < colCount; j++)
            {
                var colName = colNames[j];
                Assert.AreEqual(table[i][j], sheetRow[colName]);
            }
        }
    }

    /// <summary>
    /// 列名の行を含めて、3x3サイズのテーブルを正しく読み込めるかどうか
    /// </summary>
    [Test]
    public void LoadTest3x3()
    {
        var table = new List<List<string>>();

        // 1行目にはパラメータ名が書かれる。1列目は必ず"ID"
        table.Add(new List<string>(){
            COL_NAME_ID, "PARAM1", "PARAM2"
        });

        // テーブル本体の作成
        table.Add(new List<string>()
        {
            "1", "1_1", "2_1"
        });
        table.Add(new List<string>()
        {
            "2", "2_1", "2_2"
        });
        spreadSheetsService.Table = table;

        const string SHEET_ID = "sheet3x3";
        AssertTable(table, target.LoadSheetData(SHEET_ID));
        Assert.AreEqual(SHEET_ID, spreadSheetsService.LastPassedSheetID);
    }

    /// <summary>
    /// 列名の行を含めて、1000x1000サイズのテーブルを読み込めるか調べるテスト
    /// 1000はこのツールにおける列数の限界値
    /// </summary>
    [Test]
    public void LoadTest1000x1000()
    {
        const int ROW_COUNT = 1000;
        const int COL_COUNT = 1000;
        var table = new List<List<string>>();

        // 1行目にはパラメータ名が書かれる
        table.Add(new List<string>(COL_COUNT));
        table[0].Add(COL_NAME_ID);
        for (int i = 1; i < COL_COUNT; i++)
        {
            table[0].Add("PARAM_" + i.ToString());
        }

        // テーブル本体の作成
        for (int i = 1; i < ROW_COUNT; i++)
        {
            table.Add(new List<string>(COL_COUNT));
            table[i].Add("ID_" + i.ToString());
            for (int j = 1; j < COL_COUNT; j++)
            {
                table[i].Add(i.ToString() + "_" + j.ToString());
            }
        }
        spreadSheetsService.Table = table;

        const string SHEET_ID = "sheet1000x1000";
        AssertTable(table, target.LoadSheetData(SHEET_ID));
        Assert.AreEqual(SHEET_ID, spreadSheetsService.LastPassedSheetID);
    }
}
