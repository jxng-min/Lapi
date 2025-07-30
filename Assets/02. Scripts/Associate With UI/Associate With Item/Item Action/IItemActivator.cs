using EquipmentService;
using InventoryService;

public interface IItemActivator
{
    void Inject(PlayerCtrl player_ctrl, IInventoryService inventory_service, IEquipmentService equipment_service);
    bool UseItem(Item item, int offset, SlotType slot_type = SlotType.Inventory);
}