public interface ILoaderView : IPopupView
{
    void Inject(LoaderPresenter presenter);
    void OpenUI();
    void CloseUI();
}
