using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// UI要素を操作した際の処理を行うクラスが実装するインタフェース
    /// </summary>
    public interface IUIFunction
    {
        /// <summary>
        /// UIのリストを渡す。
        /// その中から各自が欲しい物を選んでイベントを購読したり、操作対象に選んだりするので、画面上に存在する全てのUIの要素を渡せばよい
        /// </summary>
        /// <param name="uis">
        /// 操作対象となるUIのリスト
        /// </param>
        void SetUIElements(List<IUIElement> uis);
    }
}