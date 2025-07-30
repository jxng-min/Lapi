using UnityEngine;

[System.Serializable]
public class EquipmentEffect
{
    [Header("추가 체력")]
    [SerializeField] private float m_hp;

    [Header("추가 마나")]
    [SerializeField] private float m_mp;

    [Header("추가 공격력")]
    [SerializeField] private float m_atk;

    [Header("추가 이동속도")]
    [SerializeField] private float m_spd;

    public float HP
    {
        get => m_hp;
        private set => m_hp = value;
    }

    public float MP
    {
        get => m_mp;
        private set => m_mp = value;
    }

    public float ATK
    {
        get => m_atk;
        private set => m_atk = value;
    }

    public float SPD
    {
        get => m_spd;
        private set => m_spd = value;
    }

    public static EquipmentEffect operator +(EquipmentEffect effect1, EquipmentEffect effect2)
    {
        var calculated_effect = new EquipmentEffect();

        calculated_effect.HP = effect1.HP + effect2.HP;
        calculated_effect.MP = effect1.MP + effect2.MP;
        calculated_effect.ATK = effect1.ATK + effect2.ATK;
        calculated_effect.SPD = effect1.SPD + effect2.SPD;

        return calculated_effect;
    }
}
