
namespace GoogleDriveDownloader
{
    /// <summary>
    /// IUIMetaSheetDataFunctionとIMetaSheetLoaderを仲介する
    /// </summary>
    public class MetaSheetLoaderAdapter
    {
        public MetaSheetLoaderAdapter(
            IUILoadMetaSheetDataFunction _ui,
            IMetaSheetLoader _metaSheetLoader
        )
        {
        }
    }
}