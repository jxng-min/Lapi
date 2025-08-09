using KeyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SkillView : MonoBehaviour, ISkillView
{
    [Header("스킬 포인트")]
    [SerializeField] private TMP_Text m_skill_point_label;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private SkillPresenter m_presenter;
    private IKeyService m_key_service;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Skill")))
        {
            m_presenter.ToggleUI();
        }
    }

    public void Inject(SkillPresenter presenter)
    {
        m_presenter = presenter;
        m_key_service = ServiceLocator.Get<IKeyService>();

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
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
}
