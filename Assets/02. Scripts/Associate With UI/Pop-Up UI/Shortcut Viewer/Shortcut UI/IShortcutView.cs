public interface IShortcutView : IPopupView
{
    void Inject(ShortcutPresenter presenter);
    void OpenUI();
    void CloseUI();
}