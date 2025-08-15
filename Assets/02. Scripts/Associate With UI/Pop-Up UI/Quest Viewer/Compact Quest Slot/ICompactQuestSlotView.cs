public interface ICompactQuestSlotView
{
    void Inject(CompactQuestSlotPresenter presenter);
    void UpdateUI(string compact_description);
    void ToggleUI(bool active);
    void Destroy();
}
