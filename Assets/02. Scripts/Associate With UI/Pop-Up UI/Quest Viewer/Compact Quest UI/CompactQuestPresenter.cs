using System;
using System.Collections.Generic;
using QuestDataService;
using QuestService;

public class CompactQuestPresenter : IDisposable
{
    private readonly ICompactQuestView m_view;
    private readonly IQuestService m_quest_service;
    private readonly IQuestDataService m_quest_data_service;
    private readonly IQuestDataBase m_quest_db;
    private List<CompactQuestSlotPresenter> m_compact_slot_presenters;

    public CompactQuestPresenter(ICompactQuestView view,
                                 IQuestService quest_service,
                                 IQuestDataService quest_data_service,
                                 IQuestDataBase quest_db)
    {
        m_view = view;
        m_quest_service = quest_service;
        m_quest_data_service = quest_data_service;
        m_quest_db = quest_db;

        m_compact_slot_presenters = new();

        m_quest_service.OnAddedQuest += AddQuest;

        m_view.Inject(this);
    }

    public void AddQuest(Quest quest, QuestData quest_data)
    {
        if (quest_data.State == QuestState.CLEARED)
        {
            return;
        }

        var compact_slot_view = m_view.AddSlot();

        var compact_slot_presenter = new CompactQuestSlotPresenter(compact_slot_view,
                                                                   m_quest_service,
                                                                   m_quest_data_service,
                                                                   m_quest_db,
                                                                   quest,
                                                                   quest_data);
        compact_slot_presenter.Initialize();
        m_compact_slot_presenters.Add(compact_slot_presenter);
    }

    public CompactQuestSlotPresenter GetCompactSlotPresenter(int quest_id)
    {
        foreach (var presenter in m_compact_slot_presenters)
        {
            if (presenter.ID == quest_id)
            {
                return presenter;
            }
        }

        return null;
    }

    public void Dispose()
    {
        m_quest_service.OnAddedQuest -= AddQuest;
    }
}
