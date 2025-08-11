using System.Collections.Generic;
using KeyService;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    private IKeyService m_key_service;

    private LinkedList<IPopupPresenter> m_active_popup_list;
    private Dictionary<string, IPopupPresenter> m_presenter_dict;

    private void Awake()
    {
        m_key_service = ServiceLocator.Get<IKeyService>();
        m_active_popup_list = new();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Pause")))
        {
            if (m_active_popup_list.Count > 0)
            {
                CloseUI(m_active_popup_list.First.Value);
            }
            else
            {
                if (m_presenter_dict.TryGetValue("Pause", out var presenter))
                {
                    OpenUI(presenter);
                }
            }
        }

        InputToggleKey("Inventory");
        InputToggleKey("Equipment");
        InputToggleKey("Skill");
        InputToggleKey("Binder");
        InputToggleKey("Shortcut");
    }

    public void Inject(List<PopupData> popup_data_list)
    {
        m_presenter_dict = new();

        foreach (var popup_data in popup_data_list)
        {
            m_presenter_dict.TryAdd(popup_data.Name, popup_data.Presenter);
        }
    }

    private void InputToggleKey(string key_name)
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode(key_name)))
        {
            if (m_presenter_dict.TryGetValue(key_name, out var presenter))
            {
                ToggleUI(presenter);
            }
        }
    }

    private void ToggleUI(IPopupPresenter presenter)
    {
        if (m_active_popup_list.Contains(presenter))
        {
            CloseUI(presenter);
        }
        else
        {
            OpenUI(presenter);
        }

        SortDepth();
    }

    private void OpenUI(IPopupPresenter presenter)
    {
        m_active_popup_list.AddFirst(presenter);
        presenter.OpenUI();        
    }

    private void CloseUI(IPopupPresenter presenter)
    {
        m_active_popup_list.Remove(presenter);
        presenter.CloseUI();
    }

    private void SortDepth()
    {
        foreach (var presenter in m_active_popup_list)
        {
            presenter.SortDepth();
        }
    }
}
