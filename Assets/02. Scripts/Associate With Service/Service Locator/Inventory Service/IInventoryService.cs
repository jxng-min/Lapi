using System;

namespace InventoryService
{
    public interface IInventoryService
    {
        void Inject(ItemDataBase item_db);

        void InitializeSlot(int offset);
        void InitializeGold();

        event Action<int> OnUpdatedGold;
        void UpdateGold(int amount);

        event Action<int, ItemData> OnUpdatedSlot;
        void AddItem(ItemCode code, int count);
        void SetItem(int offset, ItemCode code, int count);
        bool UpdateItem(int offset, int count);
        void Clear(int offset);
        int GetItemCount(ItemCode code);
        bool HasItem(ItemCode code);
        ItemData GetItem(int offset);
    }
}