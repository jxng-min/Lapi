public interface ICompactQuestView
{
    void Inject(CompactQuestPresenter presenter);
    ICompactQuestSlotView AddSlot();
}