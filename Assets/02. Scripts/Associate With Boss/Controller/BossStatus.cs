using System;
using System.Collections;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    private BossCtrl m_controller;

    private Coroutine m_damage_coroutine;

    public float HP { get; private set; }
    public bool IsDead { get; set; }

    public event Action<BossCtrl> OnDead;

    private void Awake()
    {
        m_controller = GetComponent<BossCtrl>();
    }

    public void Initialize(float hp)
    {
        HP = hp;
    }

    public void UpdateHP(float amount)
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
                m_controller.ChangeState(EnemyState.DEAD);
            }
            else
            {
                Damage();
            }
        }
    }

    public virtual void Damage()
    {
        if (m_damage_coroutine == null)
        {
            m_damage_coroutine = StartCoroutine(SetDamage());
        }
    }

    public virtual void Death()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        m_controller.Animator.SetTrigger("Death");

        m_controller.Rigidbody.simulated = false;
        m_controller.Collider.enabled = false;

        m_controller.QuestService.UpdateKillCount(m_controller.SO.Code, 1);

        OnDead?.Invoke(m_controller);
    }

    private IEnumerator SetDamage()
    {
        float elapsed_time = 0f;
        float target_time = 0.25f;

        m_controller.Renderer.color = Color.red;

        while (elapsed_time <= target_time)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            elapsed_time += Time.deltaTime;

            yield return null;
        }

        m_controller.Renderer.color = Color.white;

        m_damage_coroutine = null;
    }

    public void DestroyObject()
    {
        m_controller.Drop.Drop();
        Destroy(gameObject);
    }
}
