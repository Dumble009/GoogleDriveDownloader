
namespace GoogleDriveDownloader
{

    /// <summary>
    /// シートのエクスポート操作が行われた時に呼び出されるイベントのデリゲート
    /// </summary>
    /// <param name="data">
    /// エクスポートしたいシートの情報を持ったMetaSheetData
    /// </param>
    public delegate void OnExportSheetHandler(MetaSheetData data);

    /// <summary>
    /// 各シートのデータをエクスポートするイベントを提供するUIのインタフェース
    /// </summary>
    public interface ISheetExportUI
    {
        void RegisterOnExportSheet(OnExportSheetHandler handler);
    }
}