using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ShortcutView : MonoBehaviour, IShortcutView
{
    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private ShortcutPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Inject(ShortcutPresenter presenter)
    {
        m_presenter = presenter;

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

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }
}
