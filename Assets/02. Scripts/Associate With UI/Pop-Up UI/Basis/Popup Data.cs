public class PopupData
{
    public string Name;
    public IPopupPresenter Presenter;

    public PopupData(string name, IPopupPresenter presenter)
    {
        Name = name;
        Presenter = presenter;
    }
}