using InventoryService;
using QuestService;
using UnityEngine;
using UserService;

public class EnemySpawnerInstaller : MonoBehaviour, IInstaller
{
    [Header("스포너의 부모 트랜스폼")]
    [SerializeField] private Transform m_spawner_root;
    public void Install()
    {
        var spawners = m_spawner_root.GetComponentsInChildren<EnemySpawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Inject(ServiceLocator.Get<IInventoryService>(),
                               ServiceLocator.Get<IUserService>(),
                               ServiceLocator.Get<IQuestService>(),
                               DIContainer.Resolve<ICursorDataBase>(),
                               DIContainer.Resolve<PlayerCtrl>());
        }
    }
}
