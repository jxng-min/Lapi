using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraceState : MonoBehaviour, IState<EnemyCtrl>
{
    private EnemyCtrl m_controller;
    private Coroutine m_trace_coroutine;

    public void ExecuteEnter(EnemyCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        m_trace_coroutine = StartCoroutine(Co_Trace());
    }

    private IEnumerator Co_Trace()
    {
        var trace_check_interval = 0.5f;
        var trace_timer = trace_check_interval;

        List<Node> current_path = null;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            if (m_controller.Status.CanReturn)
            {
                m_controller.Status.Return();
            }

            trace_timer -= Time.deltaTime;
            if (trace_timer <= 0f)
            {
                trace_timer = trace_check_interval;

                if (!m_controller.Attack.CanTrace())
                {
                    m_controller.ChangeState(EnemyState.IDLE);
                    yield break;
                }

                var player_position = m_controller.Player.transform.position;
                current_path = m_controller.Pathfinder.Pathfind(transform.position, player_position);

                if (current_path == null || current_path.Count == 0)
                {
                    m_controller.ChangeState(EnemyState.IDLE);
                    yield break;
                }

                m_controller.Movement.MoveAlongPath(current_path);
            }

            yield return null;
        }
    }

    public void ExecuteExit()
    {
        if (m_trace_coroutine != null)
        {
            StopCoroutine(m_trace_coroutine);
            m_trace_coroutine = null;
        }

        m_controller.Movement.Reset();
    }
}