using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class EquipmentView : MonoBehaviour, IEquipmentView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("체력")]
    [SerializeField] private TMP_Text m_hp_label;

    [Header("마나")]
    [SerializeField] private TMP_Text m_mp_label;

    [Header("공격력")]
    [SerializeField] private TMP_Text m_atk_label;

    [Header("이동 속도")]
    [SerializeField] private TMP_Text m_spd_label;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private EquipmentPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Inject(EquipmentPresenter presenter)
    {
        m_presenter = presenter;

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);
        m_close_button.onClick.AddListener(() => SoundManager.Instance.PlaySFX("CloseUI"));
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }

    public void UpdateEffect(float max_hp, float max_mp, float atk, float spd)
    {
        m_hp_label.text = NumberFormatter.FormatNumber(max_hp);
        m_mp_label.text = NumberFormatter.FormatNumber(max_mp);
        m_atk_label.text = NumberFormatter.FormatNumber(atk);
        m_spd_label.text = NumberFormatter.FormatNumber(spd);
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsLastSibling();
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }
}
