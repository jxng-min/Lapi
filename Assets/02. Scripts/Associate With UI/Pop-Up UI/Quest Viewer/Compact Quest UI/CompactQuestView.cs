using System.Collections.Generic;
using UnityEngine;

public class CompactQuestView : MonoBehaviour, ICompactQuestView
{
    [Header("컴팩트 슬롯 프리펩")]
    [SerializeField] private GameObject m_slot_prefab;

    private CompactQuestPresenter m_presenter;
    private List<CompactQuestSlotView> m_compact_quest_slots;

    private void Awake()
    {
        m_compact_quest_slots = new();
    }

    private void OnDestroy()
    {
        m_presenter.Dispose();
    }

    public void Inject(CompactQuestPresenter presenter)
    {
        m_presenter = presenter;
    }

    public ICompactQuestSlotView AddSlot()
    {
        var compact_obj = Instantiate(m_slot_prefab, transform);

        var compact_slot = compact_obj.GetComponent<CompactQuestSlotView>();
        m_compact_quest_slots.Add(compact_slot);

        return compact_slot;
    }
}
