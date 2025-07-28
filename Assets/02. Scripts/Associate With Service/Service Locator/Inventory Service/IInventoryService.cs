using System;

namespace InventoryService
{
    public interface IInventoryService
    {
        void Inject(ItemDataBase item_db);

        event Action<int> OnUpdatedGold;
        void UpdateGold(int amount);

        event Action<int, ItemData> OnUpdatedSlot;
        void Initialize(int offset);
        void AddItem(ItemCode code, int count);
        void SwapItem(int offset1, int offset2);
        void RemoveItem(int offset, int count);
        void Clear(int offset);
        int GetItemCount(ItemCode code);
        bool HasItem(ItemCode code);
        ItemData GetItem(int offset);
    }
}