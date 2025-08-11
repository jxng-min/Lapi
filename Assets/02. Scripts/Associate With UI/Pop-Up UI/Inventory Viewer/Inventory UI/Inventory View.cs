using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class InventoryView : MonoBehaviour, IInventoryView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;
    
    [Header("골드")]
    [SerializeField] private TMP_Text m_gold_label;

    [Header("닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private InventoryPresenter m_presenter;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void Inject(InventoryPresenter inventory_presenter)
    {
        m_presenter = inventory_presenter;

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }

    public void UpdateMoney(int amount)
    {
        m_gold_label.text = NumberFormatter.FormatNumber(amount);
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
