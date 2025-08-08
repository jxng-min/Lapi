using EXPService;
using SkillService;
using UserService;

public class StatusPresenter
{
    private readonly IStatusView m_view;
    private readonly StatusModel m_model;
    private readonly IEXPService m_exp_service;
    private readonly IUserService m_user_service;
    private readonly ISkillService m_skill_service;

    public StatusPresenter(IStatusView view, StatusModel model, IEXPService exp_service, IUserService user_service, ISkillService skill_service)
    {
        m_view = view;
        m_model = model;
        m_exp_service = exp_service;
        m_user_service = user_service;
        m_skill_service = skill_service;

        m_user_service.OnUpdatedLevel += UpdateLV;
        m_model.OnUpdatedHP += UpdateHP;
        m_model.OnUpdateMP += UpdateMP;

        m_user_service.InitializeLevel();
    }

    public void UpdateLV(int level, int current_exp)
    {
        var max_exp = m_exp_service.GetEXP(level);
        while (current_exp >= max_exp)
        {
            current_exp -= max_exp;

            m_user_service.UpdateLevel(-max_exp);
            m_user_service.Status.Level++;
            m_skill_service.UpdatePoint(3);
            m_model.Initialize();
        }

        m_view.UpdateLV(m_user_service.Status.Level, (float)current_exp / (float)max_exp);
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
