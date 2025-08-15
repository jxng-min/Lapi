namespace QuestDataService
{
    public interface IQuestDataService
    {
        void Load();
        string GetName(int quest_id);
        string GetObjective(int quest_id);
        string GetDescription(int quest_id);
    }
}