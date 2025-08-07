using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotView : MonoBehaviour, ISkillSlotView
{
    [Header("UI 관련 컴포넌트")]
    [Header("아이템 이름")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("아이템 레벨")]
    [SerializeField] private TMP_Text m_level_label;

    [Header("업그레이드 버튼")]
    [SerializeField] private Button m_upgrade_button;

    [Header("비활성화 패널")]
    [SerializeField] private GameObject m_lock_panel;

    [Header("비활성화 패널 텍스트")]
    [SerializeField] private TMP_Text m_lock_label;

    private SkillSlotPresenter m_presenter;

    public void Inject(SkillSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_upgrade_button.onClick.AddListener(m_presenter.UpgradeSkill);
    }

    public void UpdateUI(string name, int level, bool can_upgrade, bool can_use, int constraint)
    {
        m_name_label.text = name;
        m_level_label.text = $"LV.{NumberFormatter.FormatNumber(level)}";

        m_upgrade_button.interactable = can_upgrade;

        if (can_use)
        {
            m_lock_label.text = string.Empty;
            m_lock_panel.SetActive(false);
        }
        else
        {
            m_lock_panel.SetActive(true);
            m_lock_label.text = $"LV.{constraint}";
        }
    }
}
