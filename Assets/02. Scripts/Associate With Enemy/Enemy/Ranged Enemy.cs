using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Enemy", menuName = "SO/Enemy/Create Ranged Enemy")]
public class RangedEnemy : MeleeEnemy
{
    [Space(30f)]
    [Header("원거리 몬스터 관련")]
    [Header("발사체")]
    [SerializeField] private GameObject m_projectile;
    public GameObject Projectile => m_projectile;

    [Header("발사 속도")]
    [SerializeField] private float m_rof;
    public float ROF => m_rof;
}