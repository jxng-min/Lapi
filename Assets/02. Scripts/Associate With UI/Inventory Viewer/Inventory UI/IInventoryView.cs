public interface IInventoryView
{
    void Inject(InventoryPresenter inventory_presenter);

    void OpenUI();
    void CloseUI();

    void UpdateMoney(int amount);
}