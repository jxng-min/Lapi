using Unity.VisualScripting;

public class StatusPresenter
{
    private readonly IStatusView m_view;
    private readonly StatusModel m_model;

    public StatusPresenter(IStatusView view, StatusModel model)
    {
        m_view = view;
        m_model = model;

        m_model.OnUpdatedHP += UpdateHP;
        m_model.OnUpdateMP += UpdateMP;
    }

    public void UpdateLV(int level, int current_exp, int max_exp)
    {
        m_view.UpdateLV(level, current_exp / max_exp);
    }

    public void UpdateHP(float current_hp, float max_hp)
    {
        m_view.UpdateHP(current_hp / max_hp);
    }

    public void UpdateMP(float current_mp, float max_mp)
    {
        m_view.UpdateMP(current_mp / max_mp);
    }
}
