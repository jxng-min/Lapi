public interface IPauseView : IPopupView
{
    void Inject(PausePresenter presenter);

    void OpenUI();
    void CloseUI();
}