public class PausePresenter : IPopupPresenter
{
    private readonly IPauseView m_view;
    public PausePresenter(IPauseView view)
    {
        m_view = view;
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
        throw new System.NotImplementedException();
    }
}
