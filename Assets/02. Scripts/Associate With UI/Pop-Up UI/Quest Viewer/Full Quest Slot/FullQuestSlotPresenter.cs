using System;
using QuestDataService;
using QuestService;

public class FullQuestSlotPresenter : IDisposable
{
    private readonly IFullQuestView m_quest_view;
    private readonly IFullQuestSlotView m_view;

    private readonly IQuestService m_quest_service;
    private readonly IQuestDataService m_quest_data_service;
    private readonly Quest m_quest;
    private readonly QuestData m_quest_data;

    public QuestState State => m_quest_service.GetQuestState(m_quest.ID);

    public FullQuestSlotPresenter(IFullQuestView quest_view,
                                  IFullQuestSlotView view,
                                  IQuestService quest_service,
                                  IQuestDataService quest_data_service,
                                  Quest quest,
                                  QuestData quest_data)
    {
        m_quest_view = quest_view;
        m_view = view;

        m_quest_service = quest_service;
        m_quest_service.OnUpdatedState += Updates;

        m_quest_data_service = quest_data_service;

        m_quest = quest;
        m_quest_data = quest_data;

        m_quest_service.InitializeQuest(m_quest.ID);
        m_quest_service.UpdateQuest(m_quest.ID);
        view.Inject(this);
    }

    public void Initialize()
    {
        m_view.InitializeSlot(m_quest_data_service.GetName(m_quest.ID),
                              EncodeState(m_quest_data.State));
    }

    public void Updates(int quest_id, QuestState state)
    {
        if (m_quest.ID == quest_id)
        {
            m_view.UpdateSlot(EncodeState(state), state != QuestState.CLEARED);
        }
    }

    private string EncodeState(QuestState state)
    {
        switch (state)
        {
            case QuestState.IN_PROGRESS:
                return "<color=red>진행중</color>";

            case QuestState.CAN_CLEAR:
                return "<color=green>완료 가능</color>";

            case QuestState.CLEARED:
                return "<color=#999999>완료</color>";

            default:
                return "";
        }
    }

    public void OnClickedCompact()
    {
        m_quest_service.OnUpdatedState -= Updates;
    }

    public void OnClickedInfo()
    {
        m_quest_view.UpdateInfo(m_quest_data_service.GetDescription(m_quest.ID));
    }

    public void OnCLickedNavigation()
    {

    }

    public void ToggleSlot(bool active)
    {
        m_view.ToggleSlot(active);
    }

    public void Dispose()
    {
        m_quest_service.OnUpdatedState -= Updates;
    }
}
