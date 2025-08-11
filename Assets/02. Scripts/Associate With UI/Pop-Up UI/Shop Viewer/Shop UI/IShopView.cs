public interface IShopView : IPopupView
{
    void Inject(ShopPresenter presenter);
    void OpenUI();
    void CloseUI();

    IShopSlotView GetShopSlotView();
    IItemSlotView GetItemSlotView(int index);
}