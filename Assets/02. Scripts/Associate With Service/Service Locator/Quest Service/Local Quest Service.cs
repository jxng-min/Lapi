using System;
using System.Collections.Generic;
using System.IO;
using InventoryService;
using UnityEngine;
using UserService;

namespace QuestService
{
    #region Serialization
    [System.Serializable]
    public class KillQuestData
    {
        public int ID;
        public EnemyCode EnemyCode;
        public bool Clear;
        public int Count;

        public KillQuestData(int id, EnemyCode enemy_code, bool clear, int count)
        {
            ID = id;
            EnemyCode = enemy_code;
            Clear = clear;
            Count = count;
        }
    }

    [System.Serializable]
    public class ItemQuestData
    {
        public int ID;
        public ItemCode ItemCode;
        public bool Clear;
        public int Count;

        public ItemQuestData(int id, ItemCode item_code, bool clear, int count)
        {
            ID = id;
            ItemCode = item_code;
            Clear = clear;
            Count = count;
        }
    }

    [System.Serializable]
    public class DialogueQuestData
    {
        public int ID;
        public NPCCode NPCCode;
        public bool Clear;

        public DialogueQuestData(int id, NPCCode npc_code, bool clear)
        {
            ID = id;
            NPCCode = npc_code;
            Clear = clear;
        }
    }

    [System.Serializable]
    public class QuestData
    {
        public int ID;
        public QuestState State;
        public KillQuestData[] KillQuests;
        public ItemQuestData[] ItemQuests;
        public DialogueQuestData[] DialogueQuests;

        public QuestData(int id, QuestState state, KillQuestData[] kill_quests, ItemQuestData[] item_quest, DialogueQuestData[] dialogue_quests)
        {
            ID = id;
            State = state;
            KillQuests = kill_quests;
            ItemQuests = item_quest;
            DialogueQuests = dialogue_quests;
        }
    }

    [System.Serializable]
    public class DataWrapper
    {
        public QuestData[] Data;

        public DataWrapper(QuestData[] data)
        {
            Data = data;
        }
    }
    #endregion Serialization

    public class LocalQuestService : IQuestService, ISaveable
    {
        private Dictionary<int, QuestData> m_quest_dict;

        private IInventoryService m_inventory_service;
        private IUserService m_user_service;
        private IQuestDataBase m_quest_db;

        public event Action<Quest> OnAddedQuest;
        public event Action<int, QuestState> OnUpdatedState;


        public LocalQuestService()
        {
            m_quest_dict = new();

            m_inventory_service = ServiceLocator.Get<IInventoryService>();
            m_user_service = ServiceLocator.Get<IUserService>();

            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var local_directory_path = Path.Combine(Application.persistentDataPath, "Quest");

            if (!Directory.Exists(local_directory_path))
            {
                Directory.CreateDirectory(local_directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Quest 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        public void Inject(IQuestDataBase quest_db)
        {
            m_quest_db = quest_db;
        }

        public void Initialize()
        {
            foreach (var quest_data in m_quest_dict.Values)
            {
                var quest = m_quest_db.GetQuest(quest_data.ID);
                OnAddedQuest?.Invoke(quest);
            }
        }

        public QuestData GetQuest(int quest_id)
        {
            return m_quest_dict.TryGetValue(quest_id, out var data) ? data : null;
        }

        public QuestState GetQuestState(int quest_id)
        {
            return m_quest_dict.TryGetValue(quest_id, out var data) ? data.State : QuestState.NONE;
        }

        public void ClaimReward(Quest quest)
        {
            m_inventory_service.UpdateGold(quest.RewardGold);

            for (int i = 0; i < quest.RewardItem.Length; i++)
            {
                m_inventory_service.AddItem(quest.RewardItem[i].Code, quest.RewardItem[i].Count);
            }

            m_user_service.UpdateLevel(quest.RewardEXP);
        }

        public void ClaimSubmit(Quest quest)
        {
            var item_quests = quest.ItemQuest;

            for (int i = 0; i < item_quests.Length; i++)
            {
                m_inventory_service.RemoveItem(item_quests[i].Code, item_quests[i].Total); 
            }
        }

        public void AddQuest(Quest quest)
        {
            var temp_kill_subquests = new List<KillQuestData>();
            for (int i = 0; i < quest.KillQuest.Length; i++)
            {
                temp_kill_subquests.Add(new(quest.KillQuest[i].ID, quest.KillQuest[i].Code, false, 0));
            }
            var kill_subquests = temp_kill_subquests.ToArray();

            var temp_item_subquests = new List<ItemQuestData>();
            for (int i = 0; i < quest.ItemQuest.Length; i++)
            {
                temp_item_subquests.Add(new(quest.ItemQuest[i].ID, quest.ItemQuest[i].Code, false, m_inventory_service.GetItemCount(quest.ItemQuest[i].Code)));
            }
            var item_subquests = temp_item_subquests.ToArray();

            var temp_dialogue_subquests = new List<DialogueQuestData>();
            for (int i = 0; i < quest.DialogueQuest.Length; i++)
            {
                temp_dialogue_subquests.Add(new(quest.DialogueQuest[i].ID, quest.DialogueQuest[i].Code, false));
            }
            var dialogue_subquests = temp_dialogue_subquests.ToArray();

            var new_quest_data = new QuestData(quest.ID, QuestState.IN_PROGRESS, kill_subquests, item_subquests, dialogue_subquests);
            m_quest_dict.TryAdd(new_quest_data.ID, new_quest_data);

            OnAddedQuest?.Invoke(quest);
        }

        public void RemoveQuest(int quest_id)
        {
            if (m_quest_dict.ContainsKey(quest_id))
            {
                m_quest_dict.Remove(quest_id);
            }
        }

        public void UpdateQuest(int quest_id)
        {
            var current_state = QuestState.CAN_CLEAR;

            if (m_quest_dict.TryGetValue(quest_id, out var quest_data))
            {
                if (quest_data.State == QuestState.CLEARED)
                {
                    OnUpdatedState?.Invoke(quest_id, quest_data.State);
                    return;
                }

                foreach (var kill_quest in quest_data.KillQuests)
                {
                    if (!kill_quest.Clear)
                    {
                        current_state = QuestState.IN_PROGRESS;
                        break;
                    }
                }

                foreach (var item_quest in quest_data.ItemQuests)
                {
                    if (!item_quest.Clear)
                    {
                        current_state = QuestState.IN_PROGRESS;
                        break;
                    }
                }

                foreach (var dialogue_quest in quest_data.DialogueQuests)
                {
                    if (!dialogue_quest.Clear)
                    {
                        current_state = QuestState.IN_PROGRESS;
                        break;
                    }
                }
            }

            quest_data.State = current_state;

            OnUpdatedState?.Invoke(quest_id, quest_data.State);
        }

        public void UpdateQuestState(int quest_id, QuestState state)
        {
            if (m_quest_dict.TryGetValue(quest_id, out var quest_data))
            {
                quest_data.State = state;

                OnUpdatedState?.Invoke(quest_id, quest_data.State);
            }
        }

        public void UpdateKillCount(EnemyCode enemy_code, int count)
        {
            foreach (var quest_data in m_quest_dict.Values)
            {
                if (quest_data.State == QuestState.CAN_CLEAR || quest_data.State == QuestState.CLEARED)
                {
                    continue;
                }

                foreach (var kill_quest_data in quest_data.KillQuests)
                {
                    if (kill_quest_data.EnemyCode == enemy_code && !kill_quest_data.Clear)
                    {
                        kill_quest_data.Count += count;
                    }
                }

                var kill_quests = m_quest_db.GetKillQuests(quest_data.ID);
                foreach (var kill_quest in kill_quests)
                {
                    if (kill_quest.Code == enemy_code)
                    {
                        var kill_quest_data = GetKillQuestData(quest_data.ID, kill_quest.ID);
                        if (kill_quest.Total <= kill_quest_data.Count)
                        {
                            kill_quest_data.Clear = true;
                        }

                        UpdateQuest(quest_data.ID);
                    }
                }
            }
        }

        public void UpdateItemCount(ItemCode item_code)
        {
            foreach (var quest_data in m_quest_dict.Values)
            {
                if (quest_data.State == QuestState.CLEARED)
                {
                    continue;
                }

                foreach (var item_quest_data in quest_data.ItemQuests)
                {
                    if (item_quest_data.ItemCode == item_code)
                    {
                        item_quest_data.Count = m_inventory_service.GetItemCount(item_code);
                    }
                }

                var item_quests = m_quest_db.GetItemQuests(quest_data.ID);
                foreach (var item_quest in item_quests)
                {
                    if (item_quest.Code == item_code &&
                        m_inventory_service.GetItemCount(item_code) >= item_quest.Total)
                    {
                        var item_quest_data = GetItemQuestData(quest_data.ID, item_quest.ID);
                        item_quest_data.Clear = true;
                    }

                    UpdateQuest(quest_data.ID);
                }
            }
        }

        public void UpdateDialogueCount(NPCCode npc_code)
        {
            foreach (var quest in m_quest_dict.Values)
            {
                if (quest.State == QuestState.CLEARED)
                {
                    continue;
                }

                foreach (var dialogue_quest in quest.DialogueQuests)
                {
                    if (dialogue_quest.NPCCode == npc_code)
                    {
                        dialogue_quest.Clear = true;
                    }
                }
            }
        }

        public void InitializeQuest(int quest_id)
        {
            if (m_quest_dict.TryGetValue(quest_id, out var quest_data))
            {
                foreach (var item_subquest in quest_data.ItemQuests)
                {
                    UpdateItemCount(item_subquest.ItemCode);
                }

                foreach (var dialogue_quest in quest_data.DialogueQuests)
                {
                    UpdateDialogueCount(dialogue_quest.NPCCode);
                }

                UpdateQuest(quest_id);
            }
        }

        private KillQuestData GetKillQuestData(int quest_id, int subquest_id)
        {
            if (m_quest_dict.TryGetValue(quest_id, out var quest_data))
            {
                foreach (var kill_subquest in quest_data.KillQuests)
                {
                    if (kill_subquest.ID == subquest_id)
                    {
                        return kill_subquest;
                    }
                }

                return null;
            }

            return null;
        }

        private ItemQuestData GetItemQuestData(int quest_id, int subquest_id)
        {
            if (m_quest_dict.TryGetValue(quest_id, out var quest_data))
            {
                foreach (var item_subquest in quest_data.ItemQuests)
                {
                    if (item_subquest.ID == subquest_id)
                    {
                        return item_subquest;
                    }
                }

                return null;
            }

            return null;
        }

        public bool Load(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Quest", $"QuestData{offset}.json");

            if (File.Exists(local_data_path))
            {
                m_quest_dict.Clear();

                var json_data = File.ReadAllText(local_data_path);
                var wrapped_data = JsonUtility.FromJson<DataWrapper>(json_data);

                foreach (var quest_data in wrapped_data.Data)
                {
                    m_quest_dict.TryAdd(quest_data.ID, quest_data);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save(int offset)
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Quest", $"QuestData{offset}.json");

            var temp_list = new List<QuestData>();
            foreach (var quest_data in m_quest_dict.Values)
            {
                temp_list.Add(quest_data);
            }

            var wrapped_data = new DataWrapper(temp_list.ToArray());
            var json_data = JsonUtility.ToJson(wrapped_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}