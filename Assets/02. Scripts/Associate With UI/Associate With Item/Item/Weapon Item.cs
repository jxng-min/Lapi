using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "SO/Create Weapon Item")]
public class WeaponItem : EquipmentItem
{
    [Header("무기 타입")]
    [SerializeField] private WeaponType m_weapon_type;
    public WeaponType WeaponType => m_weapon_type;
}
