public interface IWorkshopView
{
    void Inject(WorkshopPresenter presenter);

    void OpenUI();
    void CloseUI();

    IWorkshopSlotView GetWorkshopSlotView();
    void ReturnSlots();
}