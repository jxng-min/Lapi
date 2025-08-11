using InventoryService;
using SkillService;

public class ItemSlotFactory
{
    private readonly IInventoryService m_inventory_service;
    private readonly ISkillService m_skill_service;

    private readonly IItemSlotContext m_slot_context;

    private readonly IItemDataBase m_item_db;
    private readonly ICursorDataBase m_cursor_db;

    private readonly ToolTipPresenter m_tooltip_presenter;
    private readonly DragSlotPresenter m_drag_slot_presenter;

    private readonly IItemActivator m_item_activator;
    private readonly IItemCooler m_item_cooler;

    public ItemSlotFactory(IInventoryService inventory_service,
                           ISkillService skill_service,
                           IItemSlotContext slot_context,
                           IItemDataBase item_db,
                           ICursorDataBase cursor_db,
                           ToolTipPresenter tooltip_presenter,
                           DragSlotPresenter drag_slot_presenter,
                           IItemActivator item_activator,
                           IItemCooler item_cooler)
    {
        m_inventory_service = inventory_service;
        m_skill_service = skill_service;

        m_slot_context = slot_context;

        m_item_db = item_db;
        m_cursor_db = cursor_db;

        m_tooltip_presenter = tooltip_presenter;
        m_drag_slot_presenter = drag_slot_presenter;

        m_item_activator = item_activator;
        m_item_cooler = item_cooler;
    }

    public ItemSlotPresenter Instantiate(IItemSlotView view, int offset, SlotType slot_type, int count = 1)
    {
        return new ItemSlotPresenter(view,
                                     m_item_db,
                                     m_slot_context,
                                     InstantiateInteractionHandler(),
                                     m_item_cooler,
                                     m_drag_slot_presenter,
                                     offset,
                                     slot_type,
                                     count);
    }

    private IItemInteractionHandler InstantiateInteractionHandler()
    {
        return new ItemInteractionHandler(InstantiatePointerHandler(),
                                          InstantiateDragHandler(),
                                          InstantiateDropHandler());
    }

    private SlotPointerHandler InstantiatePointerHandler()
    {
        return new SlotPointerHandler(m_skill_service,
                                      m_slot_context,
                                      m_tooltip_presenter,
                                      m_item_activator,
                                      m_item_cooler,
                                      m_item_db,
                                      m_cursor_db);
    }

    private SlotDragHandler InstantiateDragHandler()
    {
        return new SlotDragHandler(m_slot_context,
                                   m_tooltip_presenter,
                                   m_drag_slot_presenter,
                                   m_cursor_db);
    }

    private SlotDropHandler InstantiateDropHandler()
    {
        return new SlotDropHandler(m_inventory_service,
                                   m_slot_context,
                                   m_tooltip_presenter,
                                   m_drag_slot_presenter,
                                   m_cursor_db);
    }
}
