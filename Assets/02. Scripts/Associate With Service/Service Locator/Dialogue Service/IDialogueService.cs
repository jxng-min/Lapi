namespace DialogueService
{
    public interface IDialogueService
    {
        void Load();
        DialogueData GetDialogue(int dialogue_id);
    }
}