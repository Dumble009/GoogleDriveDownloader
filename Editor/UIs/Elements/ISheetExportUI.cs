
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
        /// <summary>
        /// シートをエクスポートする操作が行われた際に
        /// 呼び出されるイベントを登録する
        /// </summary>
        /// <param name="_handler">
        /// シートをエクスポートする操作が行われた際に呼び出されるイベント
        /// </param>
        void RegisterOnExportSheet(OnExportSheetHandler _handler);
    }
}