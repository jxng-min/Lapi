using System;
using System.Text;
using QuestDataService;
using QuestService;

public class CompactQuestSlotPresenter: IDisposable
{
    private readonly ICompactQuestSlotView m_view;
    private readonly IQuestService m_quest_service;
    private readonly IQuestDataService m_quest_data_service;
    private readonly IQuestDataBase m_quest_db;

    private readonly Quest m_quest;
    private readonly QuestData m_quest_data;

    private bool m_is_active = true;

    public int ID => m_quest.ID;

    public CompactQuestSlotPresenter(ICompactQuestSlotView view,
                                     IQuestService quest_service,
                                     IQuestDataService quest_data_service,
                                     IQuestDataBase quest_db,
                                     Quest quest,
                                     QuestData quest_data)
    {
        m_view = view;

        m_quest_service = quest_service;
        m_quest_data_service = quest_data_service;
        m_quest_db = quest_db;

        m_quest = quest;
        m_quest_data = quest_data;

        m_quest_service.OnUpdatedState += Updates;

        m_view.Inject(this);
    }

    public void Initialize()
    {
        m_view.UpdateUI(GetDescription());
    }

    public void Updates(int quest_id, QuestState state)
    {
        if (quest_id == m_quest.ID)
        {
            if (state == QuestState.CLEARED)
            {
                m_view.Destroy();
                return;
            }

            m_view.UpdateUI(GetDescription());
        }
    }

    public void Toggle()
    {
        m_is_active = !m_is_active;
        m_view.ToggleUI(m_is_active);
    }

    private string GetDescription()
    {
        var compact_builder = new StringBuilder();
        compact_builder.AppendLine($"<size=26><color=#00CCFF>{m_quest_data_service.GetName(m_quest.ID)}</color></size>");
        compact_builder.AppendLine();

        var objective = m_quest_data_service.GetObjective(m_quest.ID);
        for (int i = 0; i < objective.Length; i++)
        {
            if (objective[i] == '{')
            {
                var format_start = i;

                for (; i < objective.Length; i++)
                {
                    if (objective[i] == '}')
                    {
                        var format_index = int.Parse(objective.Substring(format_start + 1, i - format_start - 1));

                        compact_builder.Append(GetFormatIndex(m_quest, format_index));
                        break;
                    }
                }
            }
            else
            {
                compact_builder.Append(objective[i]);
            }
        }

        return compact_builder.ToString();
    }

    private string GetFormatIndex(Quest quest, int format_id)
    {
        var subquests = m_quest_db.GetAllSubquests(quest.ID);

        for (int i = 0; i < subquests.Length; i++)
        {
            if (subquests[i].ID == format_id)
            {
                return subquests[i].GetFormatText(GetCurrentProcess(subquests[i].ID));
            }
        }

        return null;
    }

    private int GetCurrentProcess(int subquest_id)
    {
        for (int i = 0; i < m_quest_data.KillQuests.Length; i++)
        {
            if (m_quest_data.KillQuests[i].ID == subquest_id)
            {
                return m_quest_data.KillQuests[i].Count;
            }
        }

        for (int i = 0; i < m_quest_data.ItemQuests.Length; i++)
        {
            if (m_quest_data.ItemQuests[i].ID == subquest_id)
            {
                return m_quest_data.ItemQuests[i].Count;
            }
        }

        return 1;
    }

    public void Dispose()
    {
        m_quest_service.OnUpdatedState -= Updates;
    }
}
