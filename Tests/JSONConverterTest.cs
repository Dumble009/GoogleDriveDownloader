using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GoogleDriveDownloader;

/// <summary>
/// JSONConverterクラスの挙動を確認するためのテスト
/// </summary>
public class JSONConverterTest
{
    /// <summary>
    /// テスト対象のオブジェクト
    /// </summary>
    JSONConverter jsonConverter;

    [SetUp]
    public void Setup()
    {
        jsonConverter = new JSONConverter();
    }

    /// <summary>
    /// JSON文字列を、変換前の辞書オブジェクトに戻す
    /// </summary>
    /// <param name="jsonString">辞書オブジェクトへ戻したいJSON文字列</param>
    /// <returns>jsonStringから変換された辞書オブジェクト</returns>
    private Dictionary<string, Dictionary<string, string>> Deserialize(string jsonString)
    {
        return JsonConvert.DeserializeObject
                    <Dictionary<string, Dictionary<string, string>>>(jsonString);
    }

    /// <summary>
    /// 各テストケースで行うメイン部分の共通処理
    /// </summary>
    /// <param name="originalData">テストに使用する、JSONへの変換を行うシートオブジェクト</param>
    private void TestBody(SheetData originalData)
    {
        // JSON文字列であればインデントや改行の有無などは問わないので、
        // テストケースとして文字列を比較することは出来ない。
        // よってJSON文字列を再度オブジェクトへデシリアライズし、
        // その結果が元のオブジェクトと一致するかを調べる。

        // originalDataをJSONに変換する。バイト列で返ってくるのでstringに変換する
        var bytes = jsonConverter.Convert(originalData);

        // 意味のある値が返ってきているか?
        Assert.NotNull(bytes);

        // バイト列を文字列へ戻す。
        var jsonString = Encoding.UTF8.GetString(bytes.ToArray());
        Assert.NotNull(jsonString);

        // 返ってきた文字列を正しく戻すことが出来るか？
        var deserializedData = Deserialize(jsonString);
        Assert.NotNull(deserializedData);

        // キーの数が等しいか、片方に含まれる全てのキーがもう片方にも含まれるか
        // 両方の辞書で同じキーに対応する値が等しいかを調べれば辞書の一致判定が出来る
        var expectedData = originalData.Data;
        Assert.AreEqual(expectedData.Keys.Count, deserializedData.Keys.Count);
        foreach (var expectedKey in expectedData.Keys)
        {
            Assert.True(deserializedData.ContainsKey(expectedKey));
            Assert.AreEqual(expectedData[expectedKey], deserializedData[expectedKey]);
        }
    }

    /// <summary>
    /// テスト用のGoogleDrive上のスプレッドシートと同じ形式のデータを正しく変換できるか確認するテスト
    /// </summary>
    [Test]
    public void JSONConverterTestNormalData()
    {
        SheetData testSource = new SheetData();
        testSource.SetRow(
            "1",
            new Dictionary<string, string>(){
                { "NAME", "Tom"},
                { "ATK",  "1"},
                { "DEF", "2"},
                { "SPD", "3"},
                { "HP", "4"}
            }
        );
        testSource.SetRow(
            "2",
            new Dictionary<string, string>(){
                { "NAME", "Bob"},
                { "ATK",  "5"},
                { "DEF", "6"},
                { "SPD", "7"},
                { "HP", "8"}
            }
        );
        testSource.SetRow(
            "3",
            new Dictionary<string, string>(){
                { "NAME", "Jone"},
                { "ATK",  "9"},
                { "DEF", "10"},
                { "SPD", "11"},
                { "HP", "12"}
            }
        );

        TestBody(testSource);
    }

    /// <summary>
    /// 空の辞書を渡されても正しく振舞えるか確認するテスト
    /// </summary>
    [Test]
    public void JSONConverterTestEmptyData()
    {
        // 空の辞書に対しては、エラーなどではなく、空のJSON文字列を返してほしい
        SheetData testSource = new SheetData();
        TestBody(testSource);
    }
}
