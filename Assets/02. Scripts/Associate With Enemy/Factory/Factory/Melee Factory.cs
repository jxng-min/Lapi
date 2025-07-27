public class MeleeFactory : IEnemyFactory
{
    public EnemyCtrl Create()
    {
        var enemy_obj = ObjectManager.Instance.GetObject(ObjectType.MELEE_ENEMY);
        var enemy_ctrl = enemy_obj.GetComponent<EnemyCtrl>();

        return enemy_ctrl;
    }
}