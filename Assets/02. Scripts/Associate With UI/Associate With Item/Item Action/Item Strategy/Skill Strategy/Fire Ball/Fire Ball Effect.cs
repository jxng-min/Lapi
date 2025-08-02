using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class FireBallEffect : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private CircleCollider2D m_collider;

    private float m_atk;
    private float m_spd;
    private Vector2 m_direction;
    private bool m_is_return;


    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        m_is_return = false;
        m_rigidbody.simulated = true;

        StartCoroutine(Co_Return());
    }

    private void OnDisable()
    {
        m_is_return = false;
        m_rigidbody.simulated = false;
        m_rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ENEMY"))
        {
            gameObject.transform.SetParent(collider.transform);
            m_animator.SetTrigger("Boom");
            m_rigidbody.linearVelocity = Vector2.zero;

            InstantiateIndicator(collider.transform, -m_atk);

            var enemy_ctrl = collider.GetComponent<EnemyCtrl>();
            enemy_ctrl.Status.UpdateHP(-m_atk, m_direction);
        }
    }

    public void Inject(float atk, float spd, Vector2 direction)
    {
        m_atk = atk;
        m_spd = spd;
        m_direction = direction;

        m_rigidbody.linearVelocity = m_direction * m_spd;
    }

    public void EnableCollider()
    {
        m_collider.enabled = true;
    }

    public void DisableCollider()
    {
        m_collider.enabled = false;
    }

    public void Return()
    {
        if (m_is_return)
        {
            return;
        }

        m_is_return = true;

        var container = ObjectManager.Instance.GetPool(ObjectType.FIRE_BALL).Container;
        gameObject.transform.SetParent(container);

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.FIRE_BALL);
    }

    private IEnumerator Co_Return()
    {
        float elapsed_time = 0f;
        float target_time = 2f;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            yield return null;
        }

        Return();
    }

    private void InstantiateIndicator(Transform target, float amount)
    {
        var di_obj = ObjectManager.Instance.GetObject(ObjectType.DAMAGE_INDICATOR);
        di_obj.transform.SetParent(target);
        di_obj.transform.localPosition = Vector3.zero;

        var di = di_obj.GetComponent<DamageIndicator>();
        di.Initialize(amount);

        var damage = ObjectManager.Instance.GetObject(ObjectType.DAMAGE);
        damage.transform.SetParent(target);
        damage.transform.localPosition = Vector3.zero;
    }
}