public interface ISkillView
{
    void Inject(SkillPresenter presenter);

    void OpenUI();
    void CloseUI();

    void UpdatePoint(int point);
}