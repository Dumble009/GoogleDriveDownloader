using NUnit.Framework;
using GoogleDriveDownloader;

/// <summary>
/// テストコードが使用する共通処理をまとめた静的クラス
/// </summary>
public class TestUtil
{
    /// <summary>
    /// MetaSheetDataに対してアサート処理を行う
    /// </summary>
    static public void AssertMetaSheetData(
        MetaSheetData expected,
        MetaSheetData actual)
    {
        Assert.AreEqual(expected.ID, actual.ID);
        Assert.AreEqual(expected.SheetID, actual.SheetID);
        Assert.AreEqual(expected.SheetName, actual.SheetName);
        Assert.AreEqual(expected.SavePath, actual.SavePath);
        Assert.AreEqual(expected.DisplayName, actual.DisplayName);
    }
}
