using UnityEngine;

public class EnemyMoveState : MonoBehaviour, IState<EnemyCtrl>
{
    private EnemyCtrl m_controller;

    public void ExecuteEnter(EnemyCtrl sender)
    {
        if (!m_controller)
        {
            m_controller = sender;
        }
    }

    public void Execute()
    {
        m_controller.Attack.SearchTarget();

        if (!m_controller.Movement.IsMove)
        {
            m_controller.Movement.Move();
        }
    }

    public void ExecuteExit()
    {
        m_controller.Movement.Reset();
    }
}
