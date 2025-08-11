public interface IKeyBinderView : IPopupView
{
    void Inject(KeyBinderPresenter presenter);
    void OpenUI();
    void CloseUI();
}