public class PausePresenter
{
    private readonly IPauseView m_view;
    private bool m_is_open;

    public PausePresenter(IPauseView view)
    {
        m_view = view;
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
