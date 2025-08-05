using System;
using InventoryService;

namespace EquipmentService
{
    public interface IEquipmentService
    {
        event Action<WeaponType, float> OnUpdatedWeapon;
        event Action<int, ItemData> OnUpdatedSlot;
        event Action<EquipmentEffect> OnUpdatedEffect;

        void Inject(IItemDataBase item_db);
        void InitializeSlot(int offset);

        void AddItem(ItemCode code);
        void SetItem(int offset, ItemCode code);
        ItemData GetItem(int offset);
        void Clear(int offset);
        int GetOffset(ItemType type);
    }
}