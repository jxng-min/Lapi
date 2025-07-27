using UnityEngine;

public class EnemyTraceState : MonoBehaviour, IState<EnemyCtrl>
{
    private EnemyCtrl m_controller;

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateTrace));
    }

    public void ExecuteEnter(EnemyCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        InvokeRepeating(nameof(UpdateTrace), 0f, 0.5f);
    }

    public void Execute() {}

    public void ExecuteExit()
    {
        CancelInvoke(nameof(UpdateTrace));
        m_controller.Movement.Reset();
    }

    #region Helper Methods
    private void UpdateTrace()
    {
        var player = m_controller.Attack.SearchTarget();
        if (player == null)
        {
            m_controller.ChangeState(EnemyState.IDLE);
        }
        else
        {
            m_controller.Movement.Trace(player.transform.position);
        }
    }
    #endregion Helper Methods
}