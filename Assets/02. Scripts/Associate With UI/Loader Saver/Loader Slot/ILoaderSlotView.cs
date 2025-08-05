public interface ILoaderSlotView
{
    void Inject(LoaderSlotPresenter presenter);

    void UpdateUI(bool can_load, bool is_loader, int level = 0, float hour = 0f, float minute = 0f, float second = 0f);
    void LoadScene(string scene_name);
}