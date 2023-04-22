using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// UI操作に従ってメタシートを読み込んでUIに反映するクラス
    /// </summary>
    public class LoadMetaSheetDataFunction : IUIFunction
    {
        public LoadMetaSheetDataFunction(
            IMetaSheetLoader _metaSheetLoader
        )
        {

        }

        public void SetUIElements(List<IUIElement> uis)
        {
            throw new System.NotImplementedException();
        }
    }
}