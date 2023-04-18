
namespace GoogleDriveDownloader
{
    /// <summary>
    /// IUIMetaSheetDataFunctionとIMetaSheetLoaderを仲介する
    /// </summary>
    public class MetaSheetLoaderAdapter
    {
        /// <summary>
        /// UI側の、メタシートの読み込み関連の機能を提供するオブジェクト
        /// </summary>
        IUILoadMetaSheetDataFunction ui;

        /// <summary>
        /// メタシートの読み込み処理を行ってくれるオブジェクト
        /// </summary>
        IMetaSheetLoader metaSheetLoader;
        public MetaSheetLoaderAdapter(
            IUILoadMetaSheetDataFunction _ui,
            IMetaSheetLoader _metaSheetLoader
        )
        {
            ui = _ui;
            metaSheetLoader = _metaSheetLoader;

            ui.RegisterOnTriggered(this.OnLoad);
        }

        /// <summary>
        /// メタシートの読み込み処理がUIから呼び出された際に実行される
        /// </summary>
        private void OnLoad()
        {
            var datas = metaSheetLoader.LoadMetaSheet();

            ui.PassNewMetaSheetDatas(datas);
        }
    }
}