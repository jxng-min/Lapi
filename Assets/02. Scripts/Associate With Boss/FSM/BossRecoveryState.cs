using System.Collections;
using UnityEngine;

public class BossRecoveryState : MonoBehaviour, IState<BossCtrl>
{
    private BossCtrl m_controller;
    private Coroutine m_recovery_coroutine;
    private Coroutine m_return_coroutine;

    public void ExecuteEnter(BossCtrl sender)
    {
        if (m_controller == null)
        {
            m_controller = sender;
        }

        m_recovery_coroutine = StartCoroutine(Co_Recovery());
        m_return_coroutine = StartCoroutine(Co_Return());
    }

    public void ExecuteExit()
    {
        if (m_recovery_coroutine != null)
        {
            StopCoroutine(m_recovery_coroutine);
            m_recovery_coroutine = null;
        }

        if (m_return_coroutine != null)
        {
            StopCoroutine(m_return_coroutine);
            m_return_coroutine = null;
        }
    }

    private IEnumerator Co_Recovery()
    {
        var break_check_interval = 0.5f;
        float break_timer = break_check_interval;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            break_timer -= Time.deltaTime;
            if (break_timer <= 0f)
            {
                break_timer = break_check_interval;

                if (m_controller.Attack.CanTrace())
                {
                    m_controller.ChangeState(EnemyState.TRACE);
                    yield break;
                }

                if (m_controller.Status.IsDead)
                {
                    yield break;
                }

                m_controller.Status.UpdateHP(m_controller.SO.HP * 0.2f);
            }

            yield return null;
        }
    }

    private IEnumerator Co_Return()
    {
        var destination = m_controller.OriginPosition;

        var trace_check_interval = 0.3f;
        float trace_timer = trace_check_interval;

        m_controller.Movement.IsMove = true;
        m_controller.Animator.SetBool("Move", true);

        while (true)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
            
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

            if (m_controller.Status.IsDead)
            {
                yield break;
            }

            if (((Vector2)(destination - transform.position)).sqrMagnitude <= 0.01f)
            {
                m_controller.Movement.IsMove = false;
                m_controller.Animator.SetBool("Move", false);

                m_controller.Animator.SetFloat("DirX", 0f);
                m_controller.Animator.SetFloat("DirY", -1f);   
                break;
            }

            var direction = (Vector2)(destination - transform.position).normalized;
            m_controller.Animator.SetFloat("DirX", direction.x);
            m_controller.Animator.SetFloat("DirY", direction.y);

            transform.position = Vector2.MoveTowards(transform.position,
                                                        destination,
                                                        Time.deltaTime * m_controller.Movement.SPD);
            yield return null;
        }
    }
}