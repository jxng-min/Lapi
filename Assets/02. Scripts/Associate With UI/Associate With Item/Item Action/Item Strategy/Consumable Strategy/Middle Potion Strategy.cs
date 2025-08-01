public class MiddlePotionStrategy : ItemStrategy
{
    public override bool Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(120f);
        m_player_ctrl.Status.UpdateMP(80f);

        return true;
    }
}
