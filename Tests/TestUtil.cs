using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// テストコードが使用する共通処理をまとめた静的クラス
/// </summary>
public class TestUtil
{
    /// <summary>
    /// MetaSheetDataに対して一致しているかどうかのアサート処理を行う
    /// 同一のオブジェクトを指しているかどうかではなく、各メンバの値が一致しているかを調べる
    /// </summary>
    /// <param name="expected">
    /// 期待されるMetaSheetDataの値
    /// </param>
    /// <param name="actual">
    /// 実際にテスト対象から得られたMetaSheetDataの値
    /// </param>
    static public void AssertAreEqualMetaSheetData(
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
    /// SheetDataに対して一致しているかどうかのアサート処理を行う
    /// 同一のオブジェクトを指しているかどうかではなく、各メンバの値が一致しているかを調べる
    /// </summary>
    /// <param name="expected">
    /// 期待されるShetDataの値
    /// </param>
    /// <param name="actual">
    /// 実際にテスト対象から得られたSheetDataの値
    /// </param>
    static public void AssertAreEqualSheetData(
        SheetData expected,
        SheetData actual
    )
    {
        Assert.AreEqual(expected.Data, actual.Data);
    }
}
