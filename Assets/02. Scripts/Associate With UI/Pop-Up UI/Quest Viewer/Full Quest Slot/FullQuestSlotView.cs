using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullQuestSlotView : MonoBehaviour, IFullQuestSlotView
{
    [Header("퀘스트 제목")]
    [SerializeField] private TMP_Text m_quest_title_label;

    [Header("퀘스트 상태")]
    [SerializeField] private TMP_Text m_quest_state_label;

    [Header("컴팩트 버튼")]
    [SerializeField] private Button m_compact_button;

    [Header("자세히 버튼")]
    [SerializeField] private Button m_info_button;

    [Header("네비게이션 버튼")]
    [SerializeField] private Button m_nav_button;

    private FullQuestSlotPresenter m_presenter;

    public void Inject(FullQuestSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_compact_button.onClick.AddListener(m_presenter.OnClickedCompact);
        m_info_button.onClick.AddListener(m_presenter.OnClickedInfo);
        m_nav_button.onClick.AddListener(m_presenter.OnCLickedNavigation);
    }

    public void InitializeSlot(string title, string state)
    {
        m_quest_title_label.text = title;
        m_quest_state_label.text = state;
    }

    public void UpdateSlot(string state, bool button_active)
    {
        m_quest_state_label.text = state;

        m_compact_button.interactable = button_active;
        m_nav_button.interactable = button_active;
    }

    public void ToggleSlot(bool active)
    {
        gameObject.SetActive(active);
    }
}
