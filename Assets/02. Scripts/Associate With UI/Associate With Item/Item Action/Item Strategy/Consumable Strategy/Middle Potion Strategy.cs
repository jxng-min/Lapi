public class MiddlePotionStrategy : ItemStrategy
{
    public override void Activate(Item item)
    {
        m_player_ctrl.Status.UpdateHP(120f);
        m_player_ctrl.Status.UpdateMP(80f);
    }
}
