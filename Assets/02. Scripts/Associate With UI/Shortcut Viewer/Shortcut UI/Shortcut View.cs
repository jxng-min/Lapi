using KeyService;
using ShortcutService;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ShortcutView : MonoBehaviour, IShortcutView
{
    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private ShortcutPresenter m_presenter;
    private IKeyService m_key_service;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Shortcut")))
        {
            m_presenter.ToggleUI();
        }
    }

    public void Inject(ShortcutPresenter presenter)
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
}
