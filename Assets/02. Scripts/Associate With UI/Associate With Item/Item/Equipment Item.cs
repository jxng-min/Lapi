using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "SO/Create Equipment Item")]
public class EquipmentItem : Item
{
    [Header("장비 아이템의 효과")]
    [SerializeField] private EquipmentEffect m_effect;
    public EquipmentEffect EquipmentEffect => m_effect;
}
