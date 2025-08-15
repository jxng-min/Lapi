public interface IFullQuestView : IPopupView
{
    void Inject(FullQuestPresenter presenter);
    void OpenUI();
    void CloseUI();

    IFullQuestSlotView AddSlot();
    void UpdateInfo(string info);
}