using GoogleDriveDownloader;

/// <summary>
/// IReloadUIのモッククラス
/// 任意のタイミングでリロード操作の発生をエミュレートする事が出来る。
/// </summary>
public class MockReloadUI : IReloadUI, IUIElement
{
    /// <summary>
    /// リロード操作時に呼び出すイベント
    /// </summary>
    OnMetaSheetReloadHandler handler;

    public void Draw()
    {
    }

    public void RegisterOnMetaSheetReload(OnMetaSheetReloadHandler _handler)
    {
        handler += _handler;
    }

    /// <summary>
    /// リロードボタンを押された際のイベントを発行する
    /// </summary>
    public void Reload()
    {
        handler();
    }
}
