using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    #region Variables
    private Rigidbody2D m_rigidbody;

    private float m_atk;
    private float m_speed;
    private Vector2 m_direction;
    private bool m_is_return;
    #endregion Variables

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_is_return = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ENEMY"))
        {
            var enemy_ctrl = collider.GetComponent<EnemyCtrl>();
            enemy_ctrl.Status.UpdateHP(-m_atk, -m_direction);

            InstantiateIndicator(collider.transform, -m_atk);

            Return();
        }
    }

    #region Helper Methods
    public void Initialize(float atk, float speed, Vector2 direction)
    {
        m_atk = atk;
        m_speed = speed;
        m_direction = direction;

        StartCoroutine(Co_Return());

        Translation();
        Rotation(m_direction);
    }

    private void Translation()
    {
        m_rigidbody.linearVelocity = m_direction * m_speed;
    }

    private void Rotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 45f, Vector3.forward);
    }

    public void Stop()
    {
        m_rigidbody.linearVelocity = Vector2.zero;
    }

    public void Resume()
    {
        Translation();
    }

    private void Return()
    {
        if (m_is_return)
        {
            return;
        }
        m_is_return = true;
        
        Stop();
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ARROW);
    }

    private IEnumerator Co_Return()
    {
        float elapsed_time = 0f;
        float target_time = 1.5f;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;
            yield return null;
        }

        Return();
    }

    protected void InstantiateIndicator(Transform target, float amount)
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
    #endregion Helper Methods
}