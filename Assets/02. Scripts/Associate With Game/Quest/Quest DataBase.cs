using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest DataBase", menuName = "SO/DB/Quest DataBase")]
public class QuestDataBase : ScriptableObject, IQuestDataBase
{
    [Header("퀘스트 목록")]
    [SerializeField] private Quest[] m_quest_list;

    private Dictionary<int, Quest> m_quest_dict;

#if UNITY_EDITOR
    private void OnEnable()
    {
        Initialize();
    }
#endif

    private void Initialize()
    {
        m_quest_dict = new();

        foreach (var quest in m_quest_list)
        {
            m_quest_dict.TryAdd(quest.ID, quest);
        }
    }

    public Quest GetQuest(int quest_id)
    {
        if (m_quest_dict == null)
        {
            Initialize();
        }

        return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest : null;
    }

    public KillQuest[] GetKillQuests(int quest_id)
    {
        if (m_quest_dict == null)
        {
            Initialize();
        }

        return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.KillQuest : null;
    }

    public ItemQuest[] GetItemQuests(int quest_id)
    {
        if (m_quest_dict == null)
        {
            Initialize();
        }

        return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.ItemQuest : null;
    }

    public DialogueQuest[] GetDialogueQuests(int quest_id)
    {
        if (m_quest_dict == null)
        {
            Initialize();
        }

        return m_quest_dict.TryGetValue(quest_id, out var quest) ? quest.DialogueQuest : null;
    }

    public BaseQuest[] GetAllSubquests(int quest_id)
    {
        var subquest_list = new List<BaseQuest>();

        if (m_quest_dict == null)
        {
            Initialize();
        }

        if (m_quest_dict.TryGetValue(quest_id, out var quest))
        {
            foreach (var subquest in quest.KillQuest)
            {
                subquest_list.Add(subquest);
            }

            foreach (var subquest in quest.ItemQuest)
            {
                subquest_list.Add(subquest);
            }

            foreach (var subquest in quest.DialogueQuest)
            {
                subquest_list.Add(subquest);
            }

            return subquest_list.ToArray();
        }
        else
        {
            return null;
        }
    }
}
