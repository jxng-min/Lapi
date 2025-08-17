public class ShortcutPresenter : IPopupPresenter
{
    private readonly IShortcutView m_view;

    public ShortcutPresenter(IShortcutView view, InventoryPresenter inventory_presenter, SkillPresenter skill_presenter)
    {
        m_view = view;

        inventory_presenter.Initialize();
        skill_presenter.Initialize();

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }
}
