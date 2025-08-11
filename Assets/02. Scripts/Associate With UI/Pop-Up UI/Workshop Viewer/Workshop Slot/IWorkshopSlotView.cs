public interface IWorkshopSlotView
{
    void Inject(WorkshopSlotPresenter presenter);
    void UpdateUI(bool can_craft, bool is_constraint, int constraint);
    void CraftItem(float craft_time);

    IItemSlotView GetIngredientItemSlotView();
    IItemSlotView GetTargetItemSlotView();
    void ReturnItemSlots();
    void DisableObject(bool disable);
}