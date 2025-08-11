using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoaderSlotView : MonoBehaviour, ILoaderSlotView
{
    [Header("로더 슬롯 버튼")]
    [SerializeField] private Button m_slot_button;

    [Header("레벨")]
    [SerializeField] private TMP_Text m_level_label;

    [Header("플레이 타임")]
    [SerializeField] private TMP_Text m_playtime_label;

    [Header("공백")]
    [SerializeField] private TMP_Text m_empty_label;

    private LoaderSlotPresenter m_presenter;

    public void Inject(LoaderSlotPresenter presenter)
    {
        m_presenter = presenter;

        m_slot_button.onClick.AddListener(m_presenter.OnClickedButton);
    }

    public void LoadScene(string scene_name)
    {
        LoadingManager.Instance.LoadScene(scene_name);
    }

    public void UpdateUI(bool can_load, bool is_loader, int level = 0, float hour = 0, float minute = 0, float second = 0)
    {
        if (!can_load)
        {
            m_slot_button.interactable = !is_loader;

            SetAlpha(m_level_label, 0f);
            SetAlpha(m_playtime_label, 0f);
            SetAlpha(m_empty_label, 1f);
        }
        else
        {
            m_slot_button.interactable = true;

            m_level_label.text = $"레벨: {level}";
            SetAlpha(m_level_label, 1f);

            m_playtime_label.text = $"플레이 타임: {hour.ToString("00")}:{minute.ToString("00")}:{second.ToString("00")}";
            SetAlpha(m_playtime_label, 1f);

            SetAlpha(m_empty_label, 0f);
        }
    }

    private void SetAlpha(TMP_Text text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
