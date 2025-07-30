public class SmallHPPotionStrategy : ItemStrategy
{
    public override void Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(50f);
    }
}
