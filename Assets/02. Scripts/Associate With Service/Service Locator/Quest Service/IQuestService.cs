using System;

namespace QuestService
{
    public interface IQuestService
    {
        event Action<Quest> OnAddedQuest;
        event Action<int, QuestState> OnUpdatedState;

        QuestData GetQuest(int quest_id);
        QuestState GetQuestState(int quest_id);

        void Initialize();
        void Inject(IQuestDataBase quest_db);

        void ClaimReward(Quest quest);
        void ClaimSubmit(Quest quest);
        void InitializeQuest(int quest_id);

        void AddQuest(Quest quest);
        void RemoveQuest(int quest_id);

        void UpdateQuest(int quest_id);
        void UpdateQuestState(int quest_id, QuestState state);

        void UpdateKillCount(EnemyCode enemy_code, int count);
        void UpdateItemCount(ItemCode item_code);
        void UpdateDialogueCount(NPCCode npc_code);
    }
}