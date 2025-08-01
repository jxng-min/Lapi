public class SmallPotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(50f);
        m_player_ctrl.Status.UpdateMP(30f);

        return true;
    }
}
