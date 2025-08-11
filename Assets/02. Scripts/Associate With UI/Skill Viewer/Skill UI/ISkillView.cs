public interface ISkillView : IPopupView
{
    void Inject(SkillPresenter presenter);

    void OpenUI();
    void CloseUI();

    void UpdatePoint(int point);
}