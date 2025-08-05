using InventoryService;

public class InventoryPresenter
{
    private readonly IInventoryView m_view;
    private readonly IInventoryService m_model;

    private ItemSlotPresenter[] m_slot_presenters;

    private bool m_is_open;

    public InventoryPresenter(IInventoryView view, IInventoryService model, ItemSlotPresenter[] slot_presenters)
    {
        m_view = view;
        m_model = model;
        m_slot_presenters = slot_presenters;

        m_model.OnUpdatedGold += m_view.UpdateMoney;
        m_view.Inject(this);
    }

    public void ToggleUI()
    {
        if (m_is_open)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    public void OpenUI()
    {
        m_is_open = true;

        Initialize();
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_is_open = false;
        m_view.CloseUI();
    }

    public void Initialize()
    {
        m_model.InitializeGold();
        for (int i = 0; i < 30; i++)
        {
            m_model.InitializeSlot(i);
        }
    }

    public ItemSlotPresenter GetPrioritySlotPresenter(ItemCode code)
    {
        var offset = m_model.GetPriorityOffset(code);

        return offset != -1 ? m_slot_presenters[offset] : null;
    }
}