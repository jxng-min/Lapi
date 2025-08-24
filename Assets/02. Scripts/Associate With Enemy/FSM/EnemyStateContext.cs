public class EnemyStateContext
{
    private readonly EnemyCtrl m_controller;
    private IState<EnemyCtrl> m_current_state;

    public IState<EnemyCtrl> State => m_current_state;

    public EnemyStateContext(EnemyCtrl controller)
    {
        m_controller = controller;
    }

    public void Transition(IState<EnemyCtrl> state)
    {
        if (State == state)
        {
            return;
        }

        State?.ExecuteExit();
        m_current_state = state;
        State?.ExecuteEnter(m_controller);
    }
}