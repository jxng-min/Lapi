public class SmallMPPotionStrategy : ItemStrategy
{
    public override void Activate(Item item)
    {
        m_player_ctrl.Status.UpdateMP(30f);
    }
}
