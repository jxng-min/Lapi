public class MiddleHPPotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(120f);

        return true;
    }
}
