using System.Collections;
using UnityEngine;

public class BossMoveState : MonoBehaviour, IState<BossCtrl>
{
    private BossCtrl m_controller;
    private Coroutine m_move_coroutine;

    public void ExecuteEnter(BossCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        m_move_coroutine = StartCoroutine(Co_Move());
    }

    private IEnumerator Co_Move()
    {
        var destination = m_controller.Movement.GetRandomDestination();

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

        m_controller.ChangeState(EnemyState.IDLE);
    }

    public void ExecuteExit()
    {
        if (m_move_coroutine != null)
        {
            StopCoroutine(m_move_coroutine);
            m_move_coroutine = null;
        }

        m_controller.Movement.Reset();
    }
}
