public class MiddleMPPotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateMP(80f);

        return true;
    }
}
