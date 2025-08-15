using TMPro;
using UnityEngine;

public class CompactQuestSlotView : MonoBehaviour, ICompactQuestSlotView
{
    [Header("퀘스트 내용")]
    [SerializeField] private TMP_Text m_description_label;

    private CompactQuestSlotPresenter m_presenter;

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void Inject(CompactQuestSlotPresenter presenter)
    {
        m_presenter = presenter;
    }

    public void UpdateUI(string compact_description)
    {
        m_description_label.text = compact_description;
    }

    public void ToggleUI(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
