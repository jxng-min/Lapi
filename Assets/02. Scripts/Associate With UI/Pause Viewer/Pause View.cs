using KeyService;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour, IPauseView
{
    [Header("UI 관련 트랜스폼")]
    [Header("일시 정지 패널")]
    [SerializeField] private GameObject m_pause_panel;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private PausePresenter m_presenter;
    private IKeyService m_key_service;

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Pause")))
        {
            m_presenter.ToggleUI();
        }
    }

    public void Inject(PausePresenter presenter)
    {
        m_presenter = presenter;
        m_key_service = ServiceLocator.Get<IKeyService>();

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
    }

    public void OpenUI()
    {
        m_pause_panel.SetActive(true);
    }

    public void CloseUI()
    {
        m_pause_panel.SetActive(false);
    }
}
