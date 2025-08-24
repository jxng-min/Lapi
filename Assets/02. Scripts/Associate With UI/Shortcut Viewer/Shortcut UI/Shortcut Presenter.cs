public class ShortcutPresenter
{
    private readonly IShortcutView m_view;

    public ShortcutPresenter(IShortcutView view, InventoryPresenter inventory_presenter, SkillPresenter skill_presenter)
    {
        m_view = view;

        inventory_presenter.Initialize();
        skill_presenter.Initialize();

        m_view.Inject(this);
    }
}
