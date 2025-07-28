namespace ItemDataService
{
    public interface IItemDataService
    {
        bool Load();

        string GetName(ItemCode code);
        string GetDescription(ItemCode code);
    }
}