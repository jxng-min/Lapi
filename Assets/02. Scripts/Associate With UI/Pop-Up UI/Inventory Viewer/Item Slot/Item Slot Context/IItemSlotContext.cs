using System;
using InventoryService;

public interface IItemSlotContext
{
    void Register(SlotType slot_type, Action<int, ItemData> update_action, int offset = 0, int count = 0);
    void Discard(SlotType slot_type, Action<int, ItemData> update_action);
    ItemData Get(SlotType slot_type, int offset, int count = 1);
    void Set(SlotType slot_type, int offset, ItemCode code, int count);
    void Update(SlotType slot_type, int offset, int count);
    void Clear(SlotType slot_type, int offset);
}