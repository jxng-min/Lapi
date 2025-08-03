using System;
using InventoryService;

namespace ShortcutService
{
    public interface IShortcutService
    {
        event Action<int, ItemData> OnUpdatedSlot;
        
        ItemData GetItem(int offset);
        void SetItem(int offset, ItemCode code);
        void Clear(int offset);
    }
}