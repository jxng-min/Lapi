using InventoryService;
using KeyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class InventoryView : MonoBehaviour, IInventoryView
{
    [Header("UI 관련 컴포넌트")]
    [Header("골드")]
    [SerializeField] private TMP_Text m_gold_label;

    [Header("닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private InventoryPresenter m_presenter;
    private IKeyService m_key_service;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_key_service.GetKeyCode("Inventory")))
        {
            m_presenter.ToggleUI();
        }
    }

    public void Inject(InventoryPresenter inventory_presenter)
    {
        m_presenter = inventory_presenter;
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

    public void UpdateMoney(int amount)
    {
        m_gold_label.text = NumberFormatter.FormatNumber(amount);
    }
}
