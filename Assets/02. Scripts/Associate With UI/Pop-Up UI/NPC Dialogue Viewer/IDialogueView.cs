public interface IDialogueView : IPopupView
{
    void Inject(DialoguePresenter presenter);
    
    void OpenUI(System.Numerics.Vector2 position);
    void UpdateUI(string context);
    void CloseUI();
}