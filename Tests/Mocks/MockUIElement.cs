using GoogleDriveDownloader;

/// <summary>
/// IUIElementのモッククラス。
/// Draw関数を呼ばれた回数を数えておき、外部に公開する。
/// </summary>
public class MockUIElement : IUIElement
{
    int drawCallCount = 0;
    /// <summary>
    /// Draw関数を呼び出された回数
    /// </summary>
    public int DrawCallCount
    {
        get => drawCallCount;
    }

    public void Draw()
    {
        drawCallCount++;
    }
}
