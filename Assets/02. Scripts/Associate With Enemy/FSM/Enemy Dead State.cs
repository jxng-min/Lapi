using UnityEngine;

public class EnemyDeadState : MonoBehaviour, IState<EnemyCtrl>
{
    private EnemyCtrl m_controller;

    public void ExecuteEnter(EnemyCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }

        m_controller.Movement.Reset();
        m_controller.Status.Death();
    }

    public void ExecuteExit()
    {

    }
}
