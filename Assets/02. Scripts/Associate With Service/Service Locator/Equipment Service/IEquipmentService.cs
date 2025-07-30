using System;
using InventoryService;

namespace EquipmentService
{
    public interface IEquipmentService
    {
        void Inject(IItemDataBase item_db);

        void InitializeSlot(int offset);

        event Action<int, ItemData> OnUpdatedSlot;
        event Action<EquipmentEffect> OnUpdatedEffect;
        void AddItem(ItemCode code);
        void SetItem(int offset, ItemCode code);
        ItemData GetItem(int offset);
        void Clear(int offset);
        int GetOffset(ItemType type);
    }
}