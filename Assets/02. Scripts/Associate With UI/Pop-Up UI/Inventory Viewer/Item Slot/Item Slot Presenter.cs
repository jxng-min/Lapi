using System;
using System.Numerics;
using InventoryService;

public class ItemSlotPresenter : IDisposable
{
    private readonly IItemSlotView m_view;
    private readonly IItemDataBase m_item_db;
    private readonly IItemSlotContext m_slot_context;
    private readonly IItemInteractionHandler m_interaction_handler;
    private IItemCooler m_item_cooler;
    private DragSlotPresenter m_drag_slot_presenter;

    private int m_offset;
    private SlotType m_slot_type;
    private int m_item_count;

    public bool IsShopOrCraft => m_slot_type == SlotType.Shop || m_slot_type == SlotType.Craft;
    public bool IsEmpty => m_slot_context.Get(m_slot_type, m_offset).Code == ItemCode.NONE;

    public ItemSlotPresenter(IItemSlotView view,
                             IItemDataBase item_db,
                             IItemSlotContext slot_context,
                             IItemInteractionHandler interaction_handler,
                             IItemCooler item_cooler,
                             DragSlotPresenter drag_slot_presenter,
                             int offset,
                             SlotType slot_type = SlotType.Inventory,
                             int item_count = 1)
    {
        m_view = view;
        m_item_db = item_db;
        m_slot_context = slot_context;
        m_interaction_handler = interaction_handler;
        m_offset = offset;
        m_item_cooler = item_cooler;
        m_drag_slot_presenter = drag_slot_presenter;
        m_slot_type = slot_type;
        m_item_count = item_count;

        m_slot_context.Register(m_slot_type, UpdateSlot, m_offset, m_item_count);

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        if (!IsShopOrCraft && m_offset != offset)
        {
            return;
        }

        if (item_data.Code == ItemCode.NONE)
        {
            m_view.ClearUI();
            return;
        }

        var item = m_item_db.GetItem(item_data.Code);
        m_view.UpdateUI(item.Sprite, item.Stackable, item_data.Count);
    }

    public float GetCool()
    {
        var code = m_slot_context.Get(m_slot_type, m_offset).Code;
        var item = m_item_db.GetItem(code);

        return m_item_cooler.GetCool(code) / item.Cool;
    }

    public void OnPointerEnter()
    {
        m_interaction_handler.OnPointerEnter(m_slot_type, m_offset);
    }

    public void OnPointerExit()
    {
        m_interaction_handler.OnPointerExit();
    }

    public void OnBeginDrag(Vector2 mouse_position, DragMode drag_mode)
    {
        m_interaction_handler.OnBeginDrag(mouse_position, drag_mode);
    }

    public void OnDrag(Vector2 mouse_position)
    {
        m_interaction_handler.OnDrag(mouse_position);
    }

    public void OnEndDrag()
    {
        m_interaction_handler.OnEndDrag();
    }

    public void OnDrop()
    {
        var item = m_drag_slot_presenter.GetItem();
        m_interaction_handler.OnDrop(m_slot_type, m_offset, m_view.IsMask(item.Type));
    }

    public void OnPointerClick()
    {
        m_interaction_handler.OnPointerClick(m_slot_type, m_offset);
    }

    public void Dispose()
    {
        m_slot_context.Discard(m_slot_type, UpdateSlot);
    }
}