using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class FullQuestView : MonoBehaviour, IFullQuestView
{
    [Header("UI 관련 컴포넌트")]
    [Header("팝업 UI 매니저")]
    [SerializeField] private PopupUIManager m_ui_manager;

    [Header("토글")]
    [SerializeField] private Toggle m_toggle;

    [Header("풀 퀘스트 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_root;

    [Header("풀 퀘스트 슬롯 프리펩")]
    [SerializeField] private GameObject m_slot_prefab;

    [Header("풀 퀘스트 텍스트 라벨")]
    [SerializeField] private TMP_Text m_view_label;

    [Header("UI 닫기 버튼")]
    [SerializeField] private Button m_close_button;

    private Animator m_animator;
    private FullQuestPresenter m_presenter;
    private List<FullQuestSlotView> m_full_quest_slot_views;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_full_quest_slot_views = new();
    }

    public void Inject(FullQuestPresenter presenter)
    {
        m_presenter = presenter;

        m_close_button.onClick.AddListener(m_presenter.CloseUI);
        m_close_button.onClick.AddListener(PopupCloseUI);

        m_toggle.onValueChanged.AddListener((isOn) => m_presenter.OnChangedToggle(isOn));
    }

    public void OpenUI()
    {
        m_animator.SetBool("Open", true);
    }

    public void CloseUI()
    {
        m_animator.SetBool("Open", false);
    }

    public void AddSlot()
    {
        var slot_obj = Instantiate(m_slot_prefab, m_slot_root);

        var slot_view = slot_obj.GetComponent<FullQuestSlotView>();
        m_full_quest_slot_views.Add(slot_view);
    }

    public void UpdateInfo(string info)
    {
        m_view_label.text = info;
    }

    public IFullQuestSlotView GetSlot(int offset)
    {
        return m_full_quest_slot_views[offset];
    }

    public void PopupCloseUI()
    {
        m_ui_manager.RemovePresenter(m_presenter);
    }

    public void SetDepth()
    {
        (transform as RectTransform).SetAsFirstSibling();
    }
}
