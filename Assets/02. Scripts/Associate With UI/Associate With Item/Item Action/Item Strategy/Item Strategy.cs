public abstract class ItemStrategy
{
    protected PlayerCtrl m_player_ctrl;

    public void Inject(PlayerCtrl player_ctrl)
    {
        m_player_ctrl = player_ctrl;
    }

    public abstract bool Activate(Item item);
}