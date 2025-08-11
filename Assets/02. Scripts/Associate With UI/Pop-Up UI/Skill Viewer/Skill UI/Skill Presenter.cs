using System;
using SkillService;

public class SkillPresenter : IDisposable, IPopupPresenter
{
    private readonly ISkillView m_view;
    private readonly ISkillService m_model;

    private ItemSlotPresenter[] m_slot_presenters;
    public SkillPresenter(ISkillView view, ISkillService model, ItemSlotPresenter[] slot_presenters)
    {
        m_view = view;
        m_model = model;

        m_slot_presenters = slot_presenters;

        m_model.OnUpdatedPoint += m_view.UpdatePoint;
        m_view.Inject(this);

        Initialize();
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
        m_model.InitializePoint();
        for (int i = 0; i < 3; i++)
        {
            m_model.InitializeSlot(i);
        }
    }

    public ItemSlotPresenter GetPresenter(ItemCode code)
    {
        var offset = m_model.GetOffset(code);

        return offset != -1 ? m_slot_presenters[offset] : null;
    }

    public void Dispose()
    {
        m_model.OnUpdatedPoint -= m_view.UpdatePoint;
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
