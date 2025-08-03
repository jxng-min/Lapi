using KeyService;
using UnityEngine;

public class KeyBinderPresenter
{
    private readonly IKeyBinderView m_view;
    private readonly IKeyService m_key_service;

    private bool m_is_open;

    public KeyBinderPresenter(IKeyBinderView view, IKeyService key_service)
    {
        m_view = view;
        m_key_service = key_service;

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
