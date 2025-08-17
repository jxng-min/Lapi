using System;
using System.Collections.Generic;
using QuestDataService;
using QuestService;

public class FullQuestPresenter : IDisposable, IPopupPresenter
{
    private readonly IFullQuestView m_view;
    private readonly IQuestService m_quest_service;
    private readonly IQuestDataService m_quest_data_service;
    private readonly CompactQuestPresenter m_compact_quest_presenter;

    private List<FullQuestSlotPresenter> m_full_quest_slot_presenters;

    public FullQuestPresenter(IFullQuestView view,
                              IQuestService quest_service,
                              IQuestDataService quest_data_service,
                              CompactQuestPresenter compact_quest_presenter)
    {
        m_view = view;
        m_quest_service = quest_service;
        m_quest_data_service = quest_data_service;
        m_compact_quest_presenter = compact_quest_presenter;
        
        m_full_quest_slot_presenters = new();

        m_quest_service.OnAddedQuest += AddQuest;
        m_quest_service.Initialize();

        m_view.Inject(this);
    }

    public void OpenUI()
    {
        m_view.UpdateInfo(string.Empty);
        m_view.OpenUI();
    }

    public void CloseUI()
    {
        m_view.CloseUI();
    }

    public void AddQuest(Quest quest)
    {
        var full_quest_slot_view = m_view.AddSlot();

        var full_quest_slot_presenter = new FullQuestSlotPresenter(m_view,
                                                                   full_quest_slot_view,
                                                                   m_quest_service,
                                                                   m_quest_data_service,
                                                                   m_compact_quest_presenter,
                                                                   quest);
        full_quest_slot_presenter.Initialize();
        m_full_quest_slot_presenters.Add(full_quest_slot_presenter);
    }

    public void OnChangedToggle(bool isOn)
    {
        foreach (var presenter in m_full_quest_slot_presenters)
        {
            if (isOn)
            {
                if (presenter.State == QuestState.CLEARED)
                {
                    presenter.ToggleSlot(false);
                }
            }
            else
            {
                presenter.ToggleSlot(true);
            }
        }
    }

    public void SortDepth()
    {
        m_view.SetDepth();
    }

    public void Dispose()
    {
        m_quest_service.OnAddedQuest -= AddQuest;
    }
}
