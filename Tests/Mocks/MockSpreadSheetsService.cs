using System;
using System.Collections.Generic;
using GoogleDriveDownloader;

/// <summary>
/// ISpreadSheetsServiceインタフェースのモッククラス
/// 事前に渡された2次元リストをシートに見立て、要求に従って範囲データを計算して返す
/// </summary>
public class MockSpreadSheetsService : ISpreadSheetsService
{
    List<List<string>> table;
    /// <summary>
    /// Get関数を実行された際に返却値を作るために参照するデータ
    /// 行優先で、1行目にパラメータ名に該当するデータを持つこと
    /// </summary>
    public List<List<string>> Table
    {
        set => table = value;
    }

    string passedSheetID;
    public string PassedSheetID
    {
        get => passedSheetID;
    }

    public IList<IList<object>> Get(string sheetID, string range)
    {
        passedSheetID = sheetID;

        // まずrangeの文字列を分解し、開始列・行、終了列・行を示す文字列に分解する
        string startColStr = "", startRowStr = "", endColStr = "", endRowStr = "";

        // rangeを分析しやすいようにQueueに移し替える
        Queue<char> rangeCharacters = new Queue<char>();
        foreach (var c in range)
        {
            rangeCharacters.Enqueue(c);
        }

        // 先頭はstartColStrを示すアルファベット大文字の文字列になっているはず
        char peek = rangeCharacters.Dequeue();
        while ('A' <= peek && peek <= 'Z')
        {
            startColStr += peek;
            peek = rangeCharacters.Dequeue();
        }

        // その後はstartRowStrを示す数字の文字列になっているはず
        // 開始情報と終了情報の間はコロンで区切られている
        while (peek != ':')
        {
            startRowStr += peek;
            peek = rangeCharacters.Dequeue();
        }
        peek = rangeCharacters.Dequeue(); // この時点でpeek=':'なので、次の文字に更新しておく

        // その後はendColStrを示すアルファベット大文字の文字列になっているはず
        while ('A' <= peek && peek <= 'Z')
        {
            endColStr += peek;
            peek = rangeCharacters.Dequeue();
        }

        // 最後に残った文字列がendRowStrを示す数字の文字列になっているはず
        endRowStr += peek;
        while (rangeCharacters.Count > 0)
        {
            endRowStr += rangeCharacters.Dequeue();
        }

        // 文字列で得た行・列番号を0-indexedの数値に変換する
        int startColIdx = ConvertColStrToInt(startColStr);
        int startRowIdx = Convert.ToInt32(startRowStr) - 1;
        int endColIdx = ConvertColStrToInt(endColStr);
        int endRowIdx = Convert.ToInt32(endRowStr) - 1;

        // 最終的に返却するデータテーブルを作成する
        var retVal = new List<IList<object>>();
        for (int i = startRowIdx; i <= endRowIdx; i++)
        {
            if (i >= table.Count)
            {
                break;
            }

            var currentRow = new List<object>();
            for (int j = startColIdx; j <= endColIdx; j++)
            {
                if (j >= table[i].Count)
                {
                    break;
                }

                currentRow.Add(table[i][j]);
            }
            retVal.Add(currentRow);
        }

        // 1行も返り値に含まれない場合は、GoogleAPIの仕様に則って
        // nullを返す
        if (retVal.Count == 0)
        {
            return null;
        }

        return retVal;
    }

    /// <summary>
    /// スプレッドシートの列を示すアルファベットの文字列を、
    /// 何列目を示しているのかという数値に変換する。
    /// </summary>
    /// <param name="colStr">
    /// 変換対象の文字列。
    /// 大文字アルファベットで構成された文字列である事
    /// </param>
    /// <returns>
    /// colStrが示している列番号。
    /// 0-indexed
    /// </returns>
    private int ConvertColStrToInt(string colStr)
    {
        const int ALPHABET_COUNT = 26;
        int colIdx = 0;
        for (int i = 0; i < colStr.Length; i++)
        {
            colIdx *= ALPHABET_COUNT;
            colIdx += (colStr[i] - 'A') + 1;
        }

        return colIdx - 1;
    }
}
