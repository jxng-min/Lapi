public class WendigoCtrl : BossCtrl
{
    protected override void Awake()
    {
        base.Awake();
        m_attack_state = gameObject.AddComponent<WendigoAttackState>();
    }
}
