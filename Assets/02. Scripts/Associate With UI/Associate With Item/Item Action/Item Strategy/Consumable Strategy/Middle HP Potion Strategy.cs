public class MiddleHPPotionStrategy : ItemStrategy
{
    public override void Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(120f);
    }
}
