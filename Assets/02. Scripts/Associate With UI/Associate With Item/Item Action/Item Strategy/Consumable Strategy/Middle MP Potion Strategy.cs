public class MiddleMPPotionStrategy : ItemStrategy
{
    public override void Activate(Item item)
    {
        m_player_ctrl.Status.UpdateMP(80f);
    }
}
