using System.Numerics;
using InventoryService;

public class DragSlotPresenter
{
    private readonly IDragSlotView m_view;
    private IItemSlotContext m_slot_context;
    private IItemDataBase m_item_db;

    private SlotType m_slot_type;
    private int m_offset;
    private DragMode m_mode;

    public DragMode Mode => m_mode;
    public SlotType Type => m_slot_type;

    public DragSlotPresenter(IDragSlotView view,
                             IItemSlotContext slot_context,
                             IItemDataBase item_db)
    {
        m_view = view;
        m_slot_context = slot_context;
        m_item_db = item_db;
    }

    public void OpenUI(SlotType slot_type, int offset, DragMode mode)
    {
        m_offset = offset;
        m_slot_type = slot_type;
        m_mode = mode;

        var item_data = GetItemData();
        var item = m_item_db.GetItem(item_data.Code);

        m_view.OpenUI(item.Sprite);
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void Clear()
    {
        m_slot_context.Clear(m_slot_type, m_offset);
    }

    public void Updates(int amount)
    {
        m_slot_context.Update(m_slot_type, m_offset, amount);  
    }

    public void Set(ItemCode code, int amount)
    {
        m_slot_context.Set(m_slot_type, m_offset, code, amount);         
    }

    public void SetPosition(Vector2 mouse_position)
    {
        m_view.SetPosition(mouse_position);
    }

    public Item GetItem()
    {
        var code = GetItemData().Code;
        var item = m_item_db.GetItem(code);

        return item;
    }

    public ItemData GetItemData()
    {
        return m_slot_context.Get(m_slot_type, m_offset);
    }
}