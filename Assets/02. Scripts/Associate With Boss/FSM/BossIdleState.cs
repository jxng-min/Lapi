using System.Collections;
using UnityEngine;

public class BossIdleState : MonoBehaviour, IState<BossCtrl>
{
    private BossCtrl m_controller;
    private Coroutine m_idle_coroutine;

    private const float m_min_idle_time = 0f;
    private const float m_max_idle_time = 2f;

    public void ExecuteEnter(BossCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        if (m_controller.IsInit)
        {
            m_controller.Animator.SetBool("Move", false);

            var idle_duration = Random.Range(m_min_idle_time, m_max_idle_time);
            m_idle_coroutine = StartCoroutine(Co_Idle(idle_duration));
        }

        if (m_controller.Status.IsDead)
        {
            return;
        }
    }

    private IEnumerator Co_Idle(float duration)
    {
        float trace_check_interval = 0.3f;
        float trace_timer = trace_check_interval;

        while (duration > 0f)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
            
            duration -= Time.deltaTime;
            trace_timer -= Time.deltaTime;

            if (trace_timer <= 0f)
            {
                trace_timer = trace_check_interval;

                if (m_controller.Attack.CanTrace())
                {
                    m_controller.ChangeState(EnemyState.TRACE);
                    yield break;
                }
            }

            yield return null;
        }

        m_controller.ChangeState(EnemyState.MOVE);
    }

    public void ExecuteExit()
    {
        if (m_idle_coroutine != null)
        {
            StopCoroutine(m_idle_coroutine);
            m_idle_coroutine = null;
        }
    }
}
