using System;
using InventoryService;

public class InventoryPresenter : IDisposable, IPopupPresenter
{
    private readonly IInventoryView m_view;
    private readonly IInventoryService m_model;

    private ItemSlotPresenter[] m_slot_presenters;

    public InventoryPresenter(IInventoryView view, IInventoryService model, ItemSlotPresenter[] slot_presenters)
    {
        m_view = view;
        m_model = model;
        m_slot_presenters = slot_presenters;

        m_model.OnUpdatedGold += m_view.UpdateMoney;
        m_view.Inject(this);
    }

    public void OpenUI()
    {
        Initialize();
        m_view.OpenUI();
    }

    public void CloseUI()
    {
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

    public void Dispose()
    {
        m_model.OnUpdatedGold -= m_view.UpdateMoney;
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}