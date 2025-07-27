using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyCtrl m_controller;

    private float m_radius = 8f;

    [Header("플레이어의 레이어")]
    [SerializeField] private LayerMask m_player_layer;

    public float ATK { get; private set; }

    private void Awake()
    {
        m_controller = GetComponent<EnemyCtrl>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerStatus>();
            player.UpdateHP(-ATK);

            InstantiateIndicator(collision.transform, -ATK);
        }
    }

    public void Initialize(float atk)
    {
        ATK = atk;
    }

    public Collider2D SearchTarget()
    {
        var hit = Physics2D.OverlapCircle(transform.position, m_radius, m_player_layer);
        if (hit)
        {
            m_controller.ChangeState(EnemyState.TRACE);
        }

        return hit;
    }

    private void InstantiateIndicator(Transform target, float damage)
    {
        var di_obj = ObjectManager.Instance.GetObject(ObjectType.DAMAGE_INDICATOR);
        di_obj.transform.SetParent(target);
        di_obj.transform.localPosition = Vector3.zero;

        var di = di_obj.GetComponent<DamageIndicator>();
        di.Initialize(damage);
    }
}
