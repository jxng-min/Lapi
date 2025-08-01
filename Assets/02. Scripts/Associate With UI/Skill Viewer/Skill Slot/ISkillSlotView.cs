public interface ISkillSlotView
{
    void Inject(SkillSlotPresenter presenter);
    void UpdateUI(string name, int level, bool can_upgrade, bool can_use, int constraint);
}