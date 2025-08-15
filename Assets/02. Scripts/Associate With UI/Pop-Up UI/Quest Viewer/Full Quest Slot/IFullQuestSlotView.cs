public interface IFullQuestSlotView
{
    void Inject(FullQuestSlotPresenter presenter);
    void InitializeSlot(string title, string state);
    void UpdateSlot(string state, bool button_active);
    void ToggleSlot(bool active);
}