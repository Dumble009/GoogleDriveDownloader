
namespace GoogleDriveDownloader
{
    public interface ISheetLoader
    {

        /// <summary>
        /// GoogleDrive上のスプレッドシートを読み込んで、パースして返す
        /// </summary>
        /// <param name="sheetID">読み込むスプレッドシートの、GoogleDrive上でのID</param>
        /// <returns>スプレッドシートをパースしたSheetDataオブジェクト</returns>
        SheetData LoadSheetData(string sheetID);
    }
}