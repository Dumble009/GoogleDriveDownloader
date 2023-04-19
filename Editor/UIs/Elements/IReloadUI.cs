
namespace GoogleDriveDownloader
{
    /// <summary>
    /// メタシートのリロード時に呼ばれるイベントのデリゲート 
    /// </summary>
    public delegate void OnMetaSheetReloadHandler();

    /// <summary>
    /// リロード操作を提供するUIが実装するインタフェース
    /// </summary>
    public interface IReloadUI
    {
        /// <summary>
        /// メタシートのリロード時に呼び出されるイベントを登録する
        /// </summary>
        /// <param name="_handler">
        /// メタシートのリロード時に呼び出される処理
        /// </param>
        void RegisterOnMetaSheetReload(OnMetaSheetReloadHandler _handler);
    }
}