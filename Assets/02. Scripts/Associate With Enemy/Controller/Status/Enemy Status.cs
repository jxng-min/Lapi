using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    private EnemyCtrl m_controller;

    private Coroutine m_knockback_coroutine;
    private Coroutine m_damage_coroutine;

    public float HP { get; private set; }
    public bool IsDead { get; set; }
    public bool IsKnockback { get; private set; }

    private void Awake()
    {
        m_controller = GetComponent<EnemyCtrl>();
    }

    public void Initialize(float hp)
    {
        HP = hp;
    }

    public void UpdateHP(float amount, Vector2 knockback_direction)
    {
        if (IsDead)
        {
            return;
        }

        HP += amount;

        if (amount < 0f)
        {
            if (HP <= 0f)
            {
                Death();
            }
            else
            {
                Damage(knockback_direction);
            }
        }
    }

    public void Damage(Vector2 knockback_direction)
    {
        if (m_damage_coroutine == null)
        {
            m_damage_coroutine = StartCoroutine(SetDamage());
        }

        if (m_knockback_coroutine == null)
        {
            m_knockback_coroutine = StartCoroutine(SetKnockback(knockback_direction));
        }
    }

    public void Death()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        m_controller.Animator.speed = 0f;
        m_controller.Rigidbody.simulated = false;
        m_controller.Collider.enabled = false;

        StartCoroutine(SetDeath());
    }

    private void SetAlpha(float alpha)
    {
        var color = m_controller.Renderer.color;
        color.a = alpha;
        m_controller.Renderer.color = color;
    }

    private void Return()
    {
        m_controller.Spawner.UpdateCount(m_controller.SpawnerID, -1);

        ObjectManager.Instance.ReturnObject(gameObject,
                                            m_controller.SO.Type == EnemyType.MELEE ?
                                            ObjectType.MELEE_ENEMY : ObjectType.RANGED_ENEMY);
    }

    private IEnumerator SetDamage()
    {
        float elapsed_time = 0f;
        float target_time = 0.25f;

        m_controller.Renderer.color = Color.black;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            yield return null;
        }

        m_controller.Renderer.color = Color.white;

        m_damage_coroutine = null;
    }

    private IEnumerator SetKnockback(Vector2 knockback_direction)
    {
        float elapsed_time = 0f;
        float target_time = 0.25f;

        float knockback_power = 0.25f;

        var start_pos = m_controller.Rigidbody.position;
        var offset = knockback_direction.normalized * knockback_power;

        IsKnockback = true;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            var delta = elapsed_time / target_time;
            var new_pos = Vector2.Lerp(start_pos, start_pos + offset, delta);

            m_controller.Rigidbody.MovePosition(new_pos);

            yield return null;
        }

        IsKnockback = false;

        m_knockback_coroutine = null;
    }

    private IEnumerator SetDeath()
    {
        float elapsed_time = 0f;
        float target_time = 1.5f;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            var delta = elapsed_time / target_time;

            SetAlpha(1f - delta);

            yield return null;
        }

        SetAlpha(0f);
        m_controller.Drop.Drop();

        Return();
    }
}
