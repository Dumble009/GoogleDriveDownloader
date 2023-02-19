using System.IO;
using System.Collections.Generic;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

using UnityEngine;

/// <summary>
/// GoogleDriveからスプレッドシートを読み込んでディクショナリに変換するクラス
/// </summary>
public class SheetLoader
{
    /// <summary>
    /// Googleスプレッドシートの操作を提供するオブジェクト
    /// </summary>
    SheetsService sheetsService;
    public SheetLoader()
    {
        // 認証情報の読み込みと、認証処理
        using var fileStream = new FileStream(
            Application.dataPath + "/Scripts/GoogleDriveDownloader/Credentials/credentials.json",
            FileMode.Open,
            FileAccess.Read
        );
        var googleCredential = GoogleCredential
                                .FromStream(fileStream)
                                .CreateScoped(SheetsService.Scope.Spreadsheets);

        sheetsService = new SheetsService(
            new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential
            }
        );
    }

    /// <summary>
    /// GoogleDrive上のスプレッドシートを読み込んで、パースして返す
    /// </summary>
    /// <param name="sheetID">読み込むスプレッドシートの、GoogleDrive上でのID</param>
    /// <param name="parameterCount">1つの行を構成するパラメータの個数</param>
    /// <returns>スプレッドシートをパースしたSheetDataオブジェクト</returns>
    public SheetData LoadSheetData(
        string sheetID,
        int parameterCount
    )
    {
        SheetData retVal = new SheetData(); // 返り値

        // まずは1行目を読み込んで、パラメータ名を取得する。
        var colEdge = ColIdxToColName(parameterCount);
        var metaDataRange = $"Sheet1!B1:{colEdge}1"; // 1列目はIDで決まりなので2列目から取得する
        var request = sheetsService
                    .Spreadsheets
                    .Values
                    .Get(sheetID, metaDataRange);
        var response = request.Execute();
        var parameterNames = response.Values[0]; // 1行分しか取得しないので、即座に0番のリストを返しておく

        // 続いて2行目以降を読み込む。
        int rowIdx = 2;
        while (true)
        {
            var range = $"Sheet1!A{rowIdx}:{colEdge}{rowIdx}";
            request = sheetsService
                    .Spreadsheets
                    .Values
                    .Get(sheetID, range);
            response = request.Execute();
            var rowData = response.Values[0];

            var id = (string)rowData[0];
            // 何行あるか分からないので、まずは1列目の値を読み込んで、それが空であれば中断する
            if (id == "")
            {
                break;
            }

            // 1列目が空でなければ、他の列にもデータが存在するので、読み込む
            Dictionary<string, string> rowDic = new Dictionary<string, string>();
            for (int i = 1; i <= parameterCount; i++)
            {
                rowDic.Add((string)parameterNames[i], (string)rowData[i]);
            }

            retVal.SetRow(id, rowDic);
        }

        return retVal;
    }

    /// <summary>
    /// 列のインデックス番号を、シート上の列名(A, B, AA等)に変換する
    /// </summary>
    /// <param name="idx">0-indexedな列のインデックス番号。idx=0はAに変換される。</param>
    /// <returns>idxから変換した列名の文字列</returns>
    private string ColIdxToColName(int idx)
    {
        const int ALPHABET_COUNT = 26;
        string colName = "";

        // 1桁目は0=Aなので、個別で扱う
        int mod = idx % ALPHABET_COUNT;
        colName += 'A' + mod;

        // 2桁目以降は1=Aになる
        idx -= mod;
        while (idx > 0)
        {
            idx /= ALPHABET_COUNT;
            mod = idx % ALPHABET_COUNT;

            // A=1...Z=26なのでmod = 0->Z
            if (mod == 0)
            {
                colName += 'Z';
            }
            else
            {
                colName += 'A' + (mod - 1);
            }
        }

        // 上の桁が文字列の末尾に来ているので、反転する
        var charArray = colName.ToCharArray();
        System.Array.Reverse(charArray);

        return new string(charArray);
    }
}