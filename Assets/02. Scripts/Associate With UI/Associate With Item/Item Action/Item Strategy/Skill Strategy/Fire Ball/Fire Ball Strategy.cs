using SkillService;
using UnityEngine;

public class FireBallStrategy : ItemStrategy, ISkillStrategy
{
    private ISkillService m_skill_service;
    private SkillItem m_fireball_skill;

    private float m_main_damage;
    private float m_sub_damage;
    private float m_mp_usage;

    public void Inject(ISkillService skill_service)
    {
        m_skill_service = skill_service;
    }

    public override bool Activate(Item item)
    {
        m_fireball_skill = item as SkillItem;

        Calculation();

        if (m_player_ctrl.Status.MP < m_mp_usage)
        {
            return false;
        }

        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var player_position = m_player_ctrl.transform.position;

        var fire_ball_direction = (Vector2)(mouse_position - player_position).normalized;

        m_player_ctrl.Status.UpdateMP(-m_mp_usage);
        InstantiateFireBallEffect(fire_ball_direction);

        return true;
    }

    private void Calculation()
    {
        var skill_level = m_skill_service.GetSkillLevel(m_fireball_skill.Code);

        m_main_damage = m_player_ctrl.Attack.ATK * ((m_fireball_skill.MainDMG + (skill_level - 1) * m_fireball_skill.GrowthMainDMG) / 100f);
        m_sub_damage = m_player_ctrl.Attack.ATK * ((m_fireball_skill.SubDMG + (skill_level - 1) * m_fireball_skill.GrowthSubDMG) / 100f);
        m_mp_usage = m_fireball_skill.MP + (m_fireball_skill.GrowthMP * (skill_level - 1));
    }

    private void InstantiateFireBallEffect(Vector2 fire_ball_direction)
    {
        var fire_ball_object = ObjectManager.Instance.GetObject(ObjectType.FIRE_BALL).GetComponent<FireBallEffect>();
        fire_ball_object.transform.position = m_player_ctrl.transform.position + (Vector3)(fire_ball_direction * 0.5f);

        float angle = Mathf.Atan2(fire_ball_direction.y, fire_ball_direction.x) * Mathf.Rad2Deg;
        fire_ball_object.transform.rotation = Quaternion.Euler(0, 0, angle);

        var fire_ball = fire_ball_object.GetComponent<FireBallEffect>();
        fire_ball.Inject(m_main_damage, 100f, fire_ball_direction);
    }
}
