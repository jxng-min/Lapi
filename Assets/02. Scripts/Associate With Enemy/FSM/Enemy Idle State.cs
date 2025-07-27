using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IState<EnemyCtrl>
{
    private EnemyCtrl m_controller;
    private float m_idle_time;

    public void ExecuteEnter(EnemyCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        if (m_controller.IsInit)
        {
            m_controller.Animator.SetBool("Move", false);
            m_idle_time = Random.Range(0f, 1f);
        }
    }

    public void Execute()
    {
        m_controller.Attack.SearchTarget();

        m_idle_time -= Time.deltaTime;
        if (m_idle_time <= 0f)
        {
            m_controller.ChangeState(EnemyState.MOVE);
        }
    }

    public void ExecuteExit() {}
}
