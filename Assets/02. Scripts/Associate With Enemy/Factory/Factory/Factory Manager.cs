using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    private Dictionary<EnemyCode, IEnemyFactory> m_enemy_factory_dict = new();

    private void Awake()
    {
        RegisterEnemyFactory<MeleeFactory>(
            EnemyCode.GREEN_SLIME,
            EnemyCode.RED_SLIME,
            EnemyCode.PINK_SLIME,
            EnemyCode.BLUE_SLIME,
            EnemyCode.GREEN_HORN_SLIME,
            EnemyCode.RED_HORN_SLIME,
            EnemyCode.BLUE_HORN_SLIME,
            EnemyCode.GREEN_MUSHROON,
            EnemyCode.RED_MUSHROOM,
            EnemyCode.PINK_MUSHROOM,
            EnemyCode.WHITE_PYGMY,
            EnemyCode.GREEN_PYGMY,
            EnemyCode.BROWN_PYGMY,
            EnemyCode.BLACK_PYGMY
        );
    }

    private void RegisterEnemyFactory<TFactory>(params EnemyCode[] codes) where TFactory : IEnemyFactory, new()
    {
        foreach (var code in codes)
        {
            m_enemy_factory_dict[code] = new TFactory();
        }
    }

    public EnemyCtrl CreateEnemy(EnemyCode code)
    {
        if (m_enemy_factory_dict.TryGetValue(code, out var factory))
        {
            return factory.Create();
        }

        return null;
    }
}
