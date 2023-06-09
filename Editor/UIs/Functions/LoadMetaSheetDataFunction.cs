using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// UI操作に従ってメタシートを読み込んでUIに反映するクラス
    /// </summary>
    public class LoadMetaSheetDataFunction : IUIFunction
    {
        /// <summary>
        /// メタシートをドライブから読み込む処理を行うオブジェクト
        /// </summary>
        IMetaSheetLoader metaSheetLoader;

        /// <summary>
        /// メタシートのデータを渡す対象となるUIオブジェクトのリスト
        /// </summary>
        List<IMetaSheetDataReceiveUI> metaSheetDataReceivers;

        /// <summary>
        /// 自身がリロードイベントを購読しているUIオブジェクトのリスト
        /// </summary>
        List<IReloadUI> reloadUIs;

        public LoadMetaSheetDataFunction(
            IMetaSheetLoader _metaSheetLoader
        )
        {
            metaSheetLoader = _metaSheetLoader;

            metaSheetDataReceivers = new List<IMetaSheetDataReceiveUI>();
            reloadUIs = new List<IReloadUI>();
        }

        public void SetUIElements(List<IUIElement> uis)
        {
            foreach (var ui in uis)
            {
                // IReloadUIを見つけた時、それが今まで無かったものであれば
                // イベントを購読する
                if (ui is IReloadUI reloadUI)
                {
                    if (!reloadUIs.Contains(reloadUI))
                    {
                        reloadUIs.Add(reloadUI);
                        reloadUI.RegisterOnMetaSheetReload(
                            OnReload
                        );
                    }
                }

                // ISheetListUIを見つけた時、それが今まで無かったものであれば
                // データを渡す先として追加する
                if (ui is IMetaSheetDataReceiveUI receiver)
                {
                    if (!metaSheetDataReceivers.Contains(receiver))
                    {
                        metaSheetDataReceivers.Add(receiver);
                    }
                }
            }
        }

        /// <summary>
        /// UIがリロード操作を行ったときに呼び出される処理
        /// </summary>
        private void OnReload()
        {
            var metaSheetDatas = metaSheetLoader.LoadMetaSheet();
            foreach (var sheetListUI in metaSheetDataReceivers)
            {
                sheetListUI.UpdateList(metaSheetDatas);
            }
        }

        /// <summary>
        /// UI操作が無くとも、強制的にメタシートのロードを行う
        /// </summary>
        public void ForceLoad()
        {
            OnReload();
        }
    }
}