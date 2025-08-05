using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class LoaderView : MonoBehaviour, ILoaderView
{
    [Header("UI 관련 컴포넌트")]
    [Header("UI 열기 버튼")]
    [SerializeField] private Button m_open_button;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private LoaderPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Inject(LoaderPresenter presenter)
    {
        m_presenter = presenter;

        m_open_button.onClick.AddListener(m_presenter.OpenUI);
        m_close_button.onClick.AddListener(m_presenter.CloseUI);

        m_presenter.Initialize();
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
