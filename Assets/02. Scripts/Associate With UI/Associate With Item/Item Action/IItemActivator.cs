using EquipmentService;
using InventoryService;
using SkillService;

public interface IItemActivator
{
    void Inject(PlayerCtrl player_ctrl, IInventoryService inventory_service, IEquipmentService equipment_service, ISkillService skill_service);
    bool UseItem(Item item, int offset, SlotType slot_type = SlotType.Inventory);
}