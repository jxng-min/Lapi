using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemTable
{
    public Item Item;
    public float Weight;
}

[CreateAssetMenu(fileName = "Melee Enemy", menuName = "SO/Enemy/Create Melee Enemy")]
public class Enemy : ScriptableObject
{
    [Header("몬스터 코드")]
    [SerializeField] private EnemyCode m_code;
    public EnemyCode Code => m_code;

    [Header("몬스터 타입")]
    [SerializeField] private EnemyType m_type;
    public EnemyType Type => m_type;

    [Header("몬스터 이름")]
    [SerializeField] private string m_name;
    public string Name => m_name;

    [Header("애니메이터")]
    [SerializeField] private RuntimeAnimatorController m_animator;
    public RuntimeAnimatorController Animator => m_animator;

    [Space(30f)]
    [Header("몬스터 능력 관련")]
    [Header("체력")]
    [SerializeField] private float m_hp;
    public float HP => m_hp;

    [Header("공격력")]
    [SerializeField] private float m_atk;
    public float ATK => m_atk;

    [Header("이동 속도")]
    [SerializeField] private float m_spd;
    public float SPD => m_spd;

    [Space(30f)]
    [Header("처치 보상 관련")]
    [Header("경험치")]
    [SerializeField] private int m_exp;
    public int EXP => m_exp;

    [Header("경험치 편차")]
    [SerializeField] private int m_exp_dev;
    public int EXP_DEV => m_exp_dev;

    [Header("골드")]
    [SerializeField] private int m_gold;
    public int GOLD => m_gold;

    [Header("골드 편차")]
    [SerializeField] private int m_gold_dev;
    public int GOLD_DEV => m_gold_dev;

    [Header("드랍 아이템 목록")]
    [SerializeField] private List<ItemTable> m_item_list;
    public List<ItemTable> DropList { get => m_item_list; }

    [Header("아이템 드랍 확률(0 ~ 100)")]
    [SerializeField] private float m_drop_rate;
    public float DropRate { get => m_drop_rate; }
}