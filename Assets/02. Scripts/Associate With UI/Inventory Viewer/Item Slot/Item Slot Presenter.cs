using InventoryService;

public class ItemSlotPresenter
{
    private readonly IItemSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private ItemDataBase m_item_db;

    private int m_offset;
    private bool m_is_inventory;

    public ItemSlotPresenter(IItemSlotView view, IInventoryService inventory_service, ItemDataBase item_db, bool is_inventory = true)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_item_db = item_db;
        m_is_inventory = is_inventory;

        if (m_is_inventory)
        {
            m_inventory_service.OnUpdatedSlot += UpdateSlot;
        }

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        m_offset = offset;

        if (item_data.Code == ItemCode.NONE)
        {
            m_view.ClearUI();
        }
        else
        {
            var item = m_item_db.GetItem(item_data.Code);

            m_view.UpdateUI(item.Sprite, item.Stackable, item_data.Count);
        }
    }

    public void OnPointerEnter()
    {
        var code = m_inventory_service.GetItem(m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        // TODO: 툴팁 오픈
    }

    public void OnPointerExit()
    {
        // TODO: 툴팁 클로즈
    }
}