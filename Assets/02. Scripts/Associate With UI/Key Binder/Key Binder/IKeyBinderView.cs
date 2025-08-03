public interface IKeyBinderView
{
    void Inject(KeyBinderPresenter presenter);
    void OpenUI();
    void CloseUI();
}