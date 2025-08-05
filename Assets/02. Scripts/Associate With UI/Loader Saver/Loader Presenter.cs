public class LoaderPresenter
{
    private readonly ILoaderView m_view;
    private LoaderSlotPresenter[] m_loader_slot_presenters;

    public LoaderPresenter(ILoaderView view, LoaderSlotPresenter[] loader_slot_presenters)
    {
        m_view = view;
        m_loader_slot_presenters = loader_slot_presenters;

        m_view.Inject(this);
    }

    public void Initialize()
    {
        m_loader_slot_presenters[4].Save();

        for (int i = 0; i < 4; i++)
        {
            m_loader_slot_presenters[i].Load();
        }

        m_loader_slot_presenters[4].Load();
    }

    public void OpenUI()
    {
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }
}
