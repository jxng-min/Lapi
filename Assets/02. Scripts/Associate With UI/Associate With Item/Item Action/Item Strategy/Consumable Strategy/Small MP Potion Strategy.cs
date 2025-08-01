public class SmallMPPotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateMP(30f);

        return true;
    }
}
