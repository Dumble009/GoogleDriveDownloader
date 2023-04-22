using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// 全てのUI要素の描画処理を行うクラス
    /// </summary>
    public class UIDrawer
    {
        /// <summary>
        /// 描画対象のUIのリスト。
        /// 先頭に来るUI要素から順に描画していく
        /// </summary>
        List<IUIElement> uis;

        /// <summary>
        /// 描画対象のUI要素を注入するコンストラクタ
        /// </summary>
        /// <param name="_uis">
        /// 描画対象のUI要素、先頭にあるものから順番に描画命令が発行される
        /// </param>
        public UIDrawer(List<IUIElement> _uis)
        {
            uis = _uis;
        }

        /// <summary>
        /// 描画命令。コンストラクタで渡されたUI要素を全て描画する
        /// </summary>
        public void Draw()
        {
            foreach (var ui in uis)
            {
                ui.Draw();
            }
        }
    }
}