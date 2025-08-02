using SkillService;
using UnityEngine;

public class ThunderStrategy : ItemStrategy, ISkillStrategy
{
    private ISkillService m_skill_service;
    private SkillItem m_thunder_skill;

    private float m_main_damage;
    private float m_sub_damage;
    private float m_mp_usage;

    public void Inject(ISkillService skill_service)
    {
        m_skill_service = skill_service;
    }

    public override bool Activate(Item item)
    {
        m_thunder_skill = item as SkillItem;

        Calculation();

        if (m_player_ctrl.Status.MP < m_mp_usage)
        {
            return false;
        }

        var mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        m_player_ctrl.Status.UpdateMP(-m_mp_usage);
        InstantiateThunderEffect(mouse_position);

        return true;
    }

    private void Calculation()
    {
        var skill_level = m_skill_service.GetSkillLevel(m_thunder_skill.Code);

        m_main_damage = m_player_ctrl.Attack.ATK * ((m_thunder_skill.MainDMG + (skill_level - 1) * m_thunder_skill.GrowthMainDMG) / 100f);
        m_sub_damage = m_player_ctrl.Attack.ATK * ((m_thunder_skill.SubDMG + (skill_level - 1) * m_thunder_skill.GrowthSubDMG) / 100f);
        m_mp_usage = m_thunder_skill.MP - (m_thunder_skill.GrowthMP * (skill_level - 1));
    }

    private void InstantiateThunderEffect(Vector2 mouse_position)
    {
        var thunder_object = ObjectManager.Instance.GetObject(ObjectType.THUNDER).GetComponent<ThunderEffect>();
        thunder_object.transform.position = mouse_position;

        var thunder = thunder_object.GetComponent<ThunderEffect>();
        thunder.Inject(m_main_damage, m_sub_damage);
    }
}
