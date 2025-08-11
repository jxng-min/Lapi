public interface IInventoryView : IPopupView
{
    void Inject(InventoryPresenter inventory_presenter);

    void OpenUI();
    void CloseUI();

    void UpdateMoney(int amount);
}