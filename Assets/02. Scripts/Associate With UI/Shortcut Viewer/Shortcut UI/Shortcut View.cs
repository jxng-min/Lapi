using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ShortcutView : MonoBehaviour, IShortcutView
{
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;
    
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
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }
}
