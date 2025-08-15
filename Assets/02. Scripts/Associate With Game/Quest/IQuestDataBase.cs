public interface IQuestDataBase
{
    Quest GetQuest(int quest_id);
    KillQuest[] GetKillQuests(int quest_id);
    ItemQuest[] GetItemQuests(int quest_id);
    DialogueQuest[] GetDialogueQuests(int quest_id);
    BaseQuest[] GetAllSubquests(int quest_id);
}