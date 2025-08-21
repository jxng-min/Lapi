public class BossStateContext
{
    private readonly BossCtrl m_controller;
    private IState<BossCtrl> m_current_state;

    public IState<BossCtrl> State => m_current_state;

    public BossStateContext(BossCtrl controller)
    {
        m_controller = controller;
    }

    public void Transition(IState<BossCtrl> state)
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