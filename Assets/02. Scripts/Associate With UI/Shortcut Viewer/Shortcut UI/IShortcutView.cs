public interface IShortcutView
{
    void Inject(ShortcutPresenter presenter);
    void OpenUI();
    void CloseUI();
}