using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour, IPauseView
{
    [Header("UI 관련 트랜스폼")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;
    [Header("일시 정지 패널")]
    [SerializeField] private GameObject m_pause_panel;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private PausePresenter m_presenter;


    public void Inject(PausePresenter presenter)
    {
        m_presenter = presenter;

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);
    }

    public void OpenUI()
    {
        m_pause_panel.SetActive(true);
        SoundManager.Instance.PlaySFX("OpenUI");
    }

    public void CloseUI()
    {
        m_pause_panel.SetActive(false);
        SoundManager.Instance.PlaySFX("CloseUI");
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}
