public interface IPauseView
{
    void Inject(PausePresenter presenter);

    void OpenUI();
    void CloseUI();
}