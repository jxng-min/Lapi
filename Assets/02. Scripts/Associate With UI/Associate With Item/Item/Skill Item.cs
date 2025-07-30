using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Item", menuName = "SO/Create Skill Item")]
public class SkillItem : Item
{
    [Space(30f)]
    [Header("스킬 정보")]
    [Header("스킬 해방 레벨")]
    [SerializeField] private int m_constraint;
    public int Constraint => m_constraint;

    [Header("주 스킬 데미지 계수")]
    [SerializeField] private int m_main_rate;
    public int MainDMG => m_main_rate;

    [Header("부 스킬 데미지 계수")]
    [SerializeField] private int m_sub_rate;
    public int SubDMG => m_sub_rate;

    [Header("사용 마나")]
    [SerializeField] private float m_mp;
    public float MP => m_mp;

    [Space(30f)]
    [Header("스킬 강화 정보")]
    [Header("성장 쿨 타임 감소")]
    [SerializeField] private float m_growth_cool;
    public float GrowthCool => m_growth_cool;

    [Header("성장 주 스킬 데미지 계수")]
    [SerializeField] private int m_growth_main_rate;
    public int GrowthMainDMG => m_growth_main_rate;

    [Header("성장 부 스킬 데미지 계수")]
    [SerializeField] private int m_growth_sub_rate;
    public int GrowthSubDMG => m_growth_sub_rate;

    [Header("성장 사용 마나")]
    [SerializeField] private float m_growth_mp;
    public float GrowthMP => m_growth_mp;
}
