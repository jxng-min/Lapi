public class SmallHPPotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(50f);

        return true;
    }
}
