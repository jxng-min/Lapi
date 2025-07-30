using System;

namespace EquipmentService
{
    public interface IEquipmentService
    {
        void Inject(ItemDataBase item_db);

        void InitializeSlot(int offset);

        void AddItem(ItemCode code);
        void SetItem(int offset, ItemCode code);
        void Clear(int offset);
        int GetOffset(ItemType type);
    }
}