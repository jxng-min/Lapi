using System.Numerics;
using InventoryService;

public class DragSlotPresenter
{
    private readonly IDragSlotView m_view;
    private ItemDataBase m_item_db;
    private readonly IInventoryService m_inventory_service;

    private SlotType m_slot_type;
    private int m_offset;
    private DragMode m_mode;

    public DragMode Mode => m_mode;

    public DragSlotPresenter(IDragSlotView view, ItemDataBase item_db, IInventoryService inventory_service)
    {
        m_view = view;
        m_item_db = item_db;
        m_inventory_service = inventory_service;
    }

    public void OpenUI(SlotType slot_type, int offset, DragMode mode)
    {
        m_offset = offset;
        m_slot_type = slot_type;
        m_mode = mode;

        var item_data = GetItemData(m_slot_type, m_offset);
        var item = m_item_db.GetItem(item_data.Code);

        m_view.OpenUI(item.Sprite);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void Clear()
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.Clear(m_offset);
                break;

            case SlotType.Equipment:
                break;

            case SlotType.Shortcut:
                break;
        }
    }

    public void Updates(int amount)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.UpdateItem(m_offset, amount);
                break;

            case SlotType.Equipment:
                break;

            case SlotType.Shortcut:
                break;
        }        
    }

    public void Set(ItemCode code, int amount)
    {
        switch (m_slot_type)
        {
            case SlotType.Inventory:
                m_inventory_service.SetItem(m_offset, code, amount);
                break;

            case SlotType.Equipment:
                break;

            case SlotType.Shortcut:
                break;
        }              
    }

    public void SetPosition(Vector2 mouse_position)
    {
        m_view.SetPosition(mouse_position);
    }

    public Item GetItem()
    {
        var code = GetItemData(m_slot_type, m_offset).Code;
        var item = m_item_db.GetItem(code);

        return item;
    }

    public ItemData GetItemData()
    {
        return GetItemData(m_slot_type, m_offset);
    }

    public ItemData GetItemData(SlotType slot_type, int offset)
    {
        switch (slot_type)
        {
            case SlotType.Inventory:
                return m_inventory_service.GetItem(offset);

            default:
                return null;
        }
    }
}