using SkillService;

public class SkillPresenter
{
    private readonly ISkillView m_view;
    private readonly ISkillService m_model;

    private ItemSlotPresenter[] m_slot_presenters;

    private bool m_is_open;

    public SkillPresenter(ISkillView view, ISkillService model, ItemSlotPresenter[] slot_presenters)
    {
        m_view = view;
        m_model = model;

        m_slot_presenters = slot_presenters;

        m_model.OnUpdatedPoint += m_view.UpdatePoint;
        m_view.Inject(this);

        Initialize();
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

    private void Initialize()
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
}
