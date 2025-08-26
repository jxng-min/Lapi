using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SetterView : MonoBehaviour, ISetterView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("BGM 토글")]
    [SerializeField] private Toggle m_bgm_toggle;

    [Header("BGM 슬라이더")]
    [SerializeField] private Slider m_bgm_slider;

    [Header("SFX 토글")]
    [SerializeField] private Toggle m_sfx_toggle;

    [Header("SFX 슬라이더")]
    [SerializeField] private Slider m_sfx_slider;

    [Header("열기 버튼")]
    [SerializeField] private Button m_open_button;

    [Header("닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private SetterPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Inject(SetterPresenter presenter)
    {
        m_presenter = presenter;

        m_bgm_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnClickedBGM(isOn));
        m_bgm_toggle.onValueChanged.AddListener((isOn) => SoundManager.Instance.PlaySFX("Default"));
        m_sfx_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnClickedSFX(isOn));
        m_sfx_toggle.onValueChanged.AddListener((isOn) => SoundManager.Instance.PlaySFX("Default"));

        m_bgm_slider.onValueChanged.AddListener((value) => m_presenter.OnValuedChangedBGM(value));
        m_sfx_slider.onValueChanged.AddListener((value) => m_presenter.OnValuedChangedSFX(value));

        m_open_button.onClick.AddListener(m_presenter.OpenUI);
        m_close_button.onClick.AddListener(m_presenter.CloseUI);
    }

    public void OpenUI(bool bgm_active, bool sfx_active, float bgm_rate, float sfx_rate)
    {
        m_animator.SetBool("Open", true);

        m_ui_manager.AddPresenter(m_presenter);

        UpdateUI(bgm_active, sfx_active, bgm_rate, sfx_rate);

        PlaySFX("OpenUI");
    }

    public void UpdateUI(bool bgm_active, bool sfx_active, float bgm_rate, float sfx_rate)
    {
        m_bgm_toggle.isOn = bgm_active;
        m_bgm_slider.value = bgm_rate;
        m_bgm_slider.interactable = m_bgm_toggle.isOn;

        m_sfx_toggle.isOn = sfx_active;
        m_sfx_slider.value = sfx_rate;
        m_sfx_slider.interactable = m_sfx_toggle.isOn;
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);

        PlaySFX("CloseUI");
    }

    public void PopupCloseUI()
    {
        if (m_ui_manager)
        {
            m_ui_manager.RemovePresenter(m_presenter);
        }
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }

    private void PlaySFX(string sfx_name)
    {
        SoundManager.Instance.PlaySFX(sfx_name);
    }
}
