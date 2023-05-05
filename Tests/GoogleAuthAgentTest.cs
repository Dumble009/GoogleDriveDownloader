using NUnit.Framework;
using GoogleDriveDownloader;

public class GoogleAuthAgentTest
{
    /// <summary>
    /// SheetsServiceオブジェクトを正常に作成できるか調べるテスト
    /// そのSheetsServiceオブジェクトを通してシートにアクセスできるか等は、
    /// 権限回りなどの問題が絡み単体テストの域を超えているので確認しない。
    /// また、異常系に関しても、主にGoogleAPIライブラリの問題になるので、
    /// 正しく例外を返せるか等は調べない。
    /// </summary>
    [Test]
    public void CreateSheetsServiceTest()
    {
        var sheetsService = new GoogleAuthAgent(
            new SystemFileLocatorForUnity()
        )
        .CreateSheetsService();

        Assert.IsNotNull(sheetsService);
    }
}
