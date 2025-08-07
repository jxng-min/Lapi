public interface IShopSlotView
{
    void Inject(ShopSlotPresenter presenter);
    void UpdateUI(string name, int price, bool purchase, bool is_constraint, int constraint_level = 1);
    void DisableObject(bool disable);
}