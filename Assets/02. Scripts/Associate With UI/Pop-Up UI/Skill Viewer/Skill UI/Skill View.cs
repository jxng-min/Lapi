using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SkillView : MonoBehaviour, ISkillView
{
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;
    
    [Header("스킬 포인트")]
    [SerializeField] private TMP_Text m_skill_point_label;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private SkillPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void Inject(SkillPresenter presenter)
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

    public void UpdatePoint(int point)
    {
        m_skill_point_label.text = NumberFormatter.FormatNumber(point);
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
