using InventoryService;

public class ItemSlotPresenter
{
    private readonly IItemSlotView m_view;
    private readonly IInventoryService m_inventory_service;
    private ItemDataBase m_item_db;

    private ToolTipPresenter m_tooltip_presenter;

    private int m_offset;
    private bool m_is_inventory;

    public ItemSlotPresenter(IItemSlotView view, IInventoryService inventory_service, ItemDataBase item_db, ToolTipPresenter tooltip_presenter, int offset, bool is_inventory = true)
    {
        m_view = view;
        m_inventory_service = inventory_service;
        m_item_db = item_db;

        m_offset = offset;

        m_tooltip_presenter = tooltip_presenter;

        m_is_inventory = is_inventory;

        if (m_is_inventory)
        {
            m_inventory_service.OnUpdatedSlot += UpdateSlot;
        }

        m_view.Inject(this);
    }

    public void UpdateSlot(int offset, ItemData item_data)
    {
        if (m_offset == offset)
        {
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
    }

    public void OnPointerEnter()
    {
        var code = m_inventory_service.GetItem(m_offset).Code;
        if (code == ItemCode.NONE)
        {
            return;
        }

        m_tooltip_presenter.OpenUI(code);
    }

    public void OnPointerExit()
    {
        m_tooltip_presenter.CloseUI();
    }
}