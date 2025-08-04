using UnityEngine;

[CreateAssetMenu(fileName = "Default Status", menuName = "SO/Create Default Status")]
public class DefaultStatus : ScriptableObject
{
    [Header("기본 체력")]
    [field: SerializeField] public float HP { get; private set; }

    [Header("기본 마나")]
    [field: SerializeField] public float MP { get; private set; }

    [Header("기본 공격력")]
    [field: SerializeField] public float ATK { get; private set; }

    [Header("기본 이동 속도")]
    [field: SerializeField] public float SPD { get; private set; }    
}