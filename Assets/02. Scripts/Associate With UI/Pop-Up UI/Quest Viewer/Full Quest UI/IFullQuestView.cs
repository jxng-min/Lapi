public interface IFullQuestView : IPopupView
{
    void Inject(FullQuestPresenter presenter);
    void OpenUI();
    void CloseUI();

    void AddSlot();
    IFullQuestSlotView GetSlot(int offset);
    void UpdateInfo(string info);
}