public class RangedFactory : IEnemyFactory
{
    public EnemyCtrl Create()
    {
        var enemy_obj = ObjectManager.Instance.GetObject(ObjectType.RANGED_ENEMY);
        var enemy_ctrl = enemy_obj.GetComponent<EnemyCtrl>();

        return enemy_ctrl;
    }
}
