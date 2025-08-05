using UnityEngine;

[CreateAssetMenu(fileName = "Growth Status", menuName = "SO/Create Growth Status")]
public class GrowthStatus : ScriptableObject
{
    [Header("성장 체력")]
    [field: SerializeField] public float HP { get; private set; }

    [Header("성장 마나")]
    [field: SerializeField] public float MP { get; private set; }

    [Header("성장 공격력")]
    [field: SerializeField] public float ATK { get; private set; }
}