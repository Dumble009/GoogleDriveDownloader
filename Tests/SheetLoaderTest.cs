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
        const string COL_NAME_PARAM1 = "PARAM1";
        const string COL_NAME_PARAM2 = "PARAM2";
        table.Add(new List<string>(){
            COL_NAME_ID, COL_NAME_PARAM1, COL_NAME_PARAM2
        });

        // テーブル本体の作成
        const string ID1 = "1";
        const string ID2 = "2";
        const string PARAM1_1 = "1_1";
        const string PARAM2_1 = "2_1";
        const string PARAM1_2 = "1_2";
        const string PARAM2_2 = "2_2";

        table.Add(new List<string>()
        {
            ID1, PARAM1_1, PARAM2_1
        });
        table.Add(new List<string>()
        {
            ID2, PARAM1_2, PARAM2_2
        });
        spreadSheetsService.Table = table;

        const string SHEET_ID = "sheet1";
        AssertTable(table, target.LoadSheetData(SHEET_ID));
        Assert.AreEqual(SHEET_ID, spreadSheetsService.LastPassedSheetID);
    }
}
