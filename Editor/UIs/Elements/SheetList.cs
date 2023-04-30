using System.Collections.Generic;

namespace GoogleDriveDownloader
{
    /// <summary>
    /// ドライブ上のシートを一覧表示するUI
    /// </summary>
    public class SheetList : IUIElement, ISheetListUI, ISheetExportUI
    {
        /// <summary>
        /// 一覧表示を構成する各アイテム
        /// </summary>
        List<SheetListItem> items;

        /// <summary>
        /// 現時点でエクスポート時のイベントを購読しているイベントハンドラ達
        /// アイテムを作り直した際に再登録する必要があるので、覚えておかないといけない
        /// </summary>
        OnExportSheetHandler registeredOnExportHandler;

        public SheetList()
        {
            items = new List<SheetListItem>();

            registeredOnExportHandler = (_) => { }; // 空関数で初期化する事でnull参照を防ぐ
        }

        public void Draw()
        {
            foreach (var item in items)
            {
                item.Draw();
            }
        }

        public void RegisterOnExportSheet(OnExportSheetHandler _handler)
        {
            registeredOnExportHandler += _handler;

            foreach (var item in items)
            {
                item.RegisterOnExportSheet(_handler);
            }
        }

        public void UpdateList(List<MetaSheetData> metaSheetDatas)
        {
            items.Clear();

            foreach (var data in metaSheetDatas)
            {
                var item = new SheetListItem(data);
                items.Add(item);

                item.RegisterOnExportSheet(registeredOnExportHandler);
            }
        }
    }
}