using EquipmentService;
using InventoryService;
using ShortcutService;
using SkillService;

public class ItemSlotFactory
{
    private readonly IInventoryService m_inventory_service;
    private readonly IEquipmentService m_equipment_service;
    private readonly ISkillService m_skill_service;
    private readonly IShortcutService m_shortcut_service;

    private readonly IItemDataBase m_item_db;

    private readonly ToolTipPresenter m_tooltip_presenter;
    private readonly DragSlotPresenter m_drag_slot_presenter;

    private readonly IItemActivator m_item_activator;
    private readonly IItemCooler m_item_cooler;

    public ItemSlotFactory(IInventoryService inventory_service,
                           IEquipmentService equipment_service,
                           ISkillService skill_service,
                           IShortcutService shortcut_service,
                           IItemDataBase item_db,
                           ToolTipPresenter tooltip_presenter,
                           DragSlotPresenter drag_slot_presenter,
                           IItemActivator item_activator,
                           IItemCooler item_cooler)
    {
        m_inventory_service = inventory_service;
        m_equipment_service = equipment_service;
        m_skill_service = skill_service;
        m_shortcut_service = shortcut_service;

        m_item_db = item_db;

        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;

        m_item_activator = item_activator;
        m_item_cooler = item_cooler;
    }

    public ItemSlotPresenter Instantiate(IItemSlotView view, int offset, SlotType slot_type, int count = 1)
    {
        return new ItemSlotPresenter(view,
                                     m_inventory_service,
                                     m_equipment_service,
                                     m_skill_service,
                                     m_shortcut_service,
                                     m_item_db,
                                     m_tooltip_presenter,
                                     m_drag_slot_presenter,
                                     m_item_activator,
                                     m_item_cooler,
                                     offset,
                                     slot_type,
                                     count);
    }
}
