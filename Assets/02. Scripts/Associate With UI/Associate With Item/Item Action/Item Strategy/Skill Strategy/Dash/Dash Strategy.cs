using System.Collections;
using SkillService;
using UnityEngine;

public class DashStrategy : ItemStrategy, ISkillStrategy
{
    private ISkillService m_skill_service;
    private SkillItem m_dash_skill;

    private float m_main_damage;
    private float m_sub_damage;
    private float m_mp_usage;

    public void Inject(ISkillService skill_service)
    {
        m_skill_service = skill_service;
    }

    public override bool Activate(Item item)
    {
        if (m_player_ctrl.Status.MP < m_mp_usage)
        {
            return false;
        }

        m_dash_skill = item as SkillItem;

        Calculation();

        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var player_position = m_player_ctrl.transform.position;

        var dash_direction = (Vector2)(mouse_position - player_position).normalized;

        m_player_ctrl.Status.UpdateMP(-m_mp_usage);
        InstantiateDashEffect(dash_direction);
        Dash(dash_direction);

        return true;
    }

    private void Calculation()
    {
        var skill_level = m_skill_service.GetSkillLevel(m_dash_skill.Code);

        m_main_damage = m_player_ctrl.Attack.ATK * ((m_dash_skill.MainDMG + (skill_level - 1) * m_dash_skill.GrowthMainDMG) / 100f);
        m_sub_damage = m_player_ctrl.Attack.ATK * ((m_dash_skill.SubDMG + (skill_level - 1) * m_dash_skill.GrowthSubDMG) / 100f);
        m_mp_usage = m_dash_skill.MP - (m_dash_skill.GrowthMP * (skill_level - 1));
    }

    private void InstantiateDashEffect(Vector2 dash_direction)
    {
        var dash_object = ObjectManager.Instance.GetObject(ObjectType.DASH).GetComponent<DashEffect>();
        dash_object.transform.position = m_player_ctrl.transform.position;

        float angle = Mathf.Atan2(dash_direction.y, dash_direction.x) * Mathf.Rad2Deg - 180;
        dash_object.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Dash(Vector2 dash_direction)
    {
        m_player_ctrl.StartCoroutine(Co_Dash(dash_direction));
    }

    private IEnumerator Co_Dash(Vector2 dash_direction)
    {
        m_player_ctrl.Movement.Controll = false;

        float dash_distance = 8f;
        float dash_speed = 40f;
        float duration = dash_distance / dash_speed;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float step = dash_speed * Time.deltaTime;
            m_player_ctrl.transform.position += (Vector3)(dash_direction * step);

            elapsed += Time.deltaTime;
            yield return null;
        }

        m_player_ctrl.Movement.Controll = true;
    }
}
