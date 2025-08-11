public interface IWorkshopView : IPopupView
{
    void Inject(WorkshopPresenter presenter);

    void OpenUI();
    void CloseUI();

    IWorkshopSlotView GetWorkshopSlotView();
    void ReturnSlots();
}