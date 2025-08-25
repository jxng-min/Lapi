using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class WorkshopView : MonoBehaviour, IWorkshopView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("제작 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("제작 토글")]
    [SerializeField] private Toggle m_toggle;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private WorkshopPresenter m_presenter;
    private List<WorkshopSlotView> m_workshop_slot_list;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_workshop_slot_list = new();
    }

    public void Inject(WorkshopPresenter presenter)
    {
        m_presenter = presenter;

        m_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnChangedToggle(isOn));
        m_toggle.onValueChanged.AddListener((isOn) => SoundManager.Instance.PlaySFX("Default"));

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

    public IWorkshopSlotView GetWorkshopSlotView()
    {
        var workshop_slot_obj = ObjectManager.Instance.GetObject(ObjectType.WORKSHOP_SLOT);
        workshop_slot_obj.transform.SetParent(m_slot_root);

        var workshop_slot_view = workshop_slot_obj.GetComponent<WorkshopSlotView>();
        m_workshop_slot_list.Add(workshop_slot_view);

        return workshop_slot_view;
    }

    public void ReturnSlots()
    {
        m_presenter.ReturnItemSlots();

        var container = ObjectManager.Instance.GetPool(ObjectType.WORKSHOP_SLOT).Container;

        foreach (var workshop_slot in m_workshop_slot_list)
        {
            workshop_slot.transform.SetParent(container);
            ObjectManager.Instance.ReturnObject(workshop_slot.gameObject, ObjectType.WORKSHOP_SLOT);
        }

        m_workshop_slot_list.Clear();
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