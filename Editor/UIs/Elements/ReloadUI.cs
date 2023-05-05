using UnityEngine;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// メタシートのリロードを行うボタン
    /// </summary>
    public class ReloadUI : IReloadUI, IUIElement
    {
        /// <summary>
        /// リロードボタン上に表示する文言
        /// </summary>
        const string RELOAD_BUTTON_LABEL = "Reload";

        /// <summary>
        /// リロードイベントに対するイベントハンドラ
        /// </summary>
        OnMetaSheetReloadHandler onReloadHandler;

        public ReloadUI()
        {
            onReloadHandler = () => { }; // 空関数で初期化する事でnull参照を防ぐ
        }

        public void Draw()
        {
            if (GUILayout.Button(RELOAD_BUTTON_LABEL))
            {
                onReloadHandler();
            }
        }

        public void RegisterOnMetaSheetReload(OnMetaSheetReloadHandler _handler)
        {
            onReloadHandler += _handler;
        }
    }
}