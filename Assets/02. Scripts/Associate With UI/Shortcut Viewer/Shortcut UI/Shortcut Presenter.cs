using ShortcutService;

public class ShortcutPresenter
{
    private readonly IShortcutView m_view;
    private readonly IShortcutService m_shortcut_service;

    private bool m_is_open;

    public ShortcutPresenter(IShortcutView view, IShortcutService shortcut_service, InventoryPresenter inventory_presenter, SkillPresenter skill_presenter)
    {
        m_view = view;
        m_shortcut_service = shortcut_service;

        inventory_presenter.Initialize();
        skill_presenter.Initialize();

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

        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_is_open = false;

        m_view.CloseUI();
    }
}
