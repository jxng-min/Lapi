using QuestService;
using UnityEngine;
using UserService;

public class MapLoaderInstaller : MonoBehaviour, IInstaller
{
    [Header("맵 로더의 목록")]
    [SerializeField] private MapLoader[] m_map_loaders;
    public void Install()
    {
        foreach(var loader in m_map_loaders)
        {
            loader.Inject(ServiceLocator.Get<IUserService>(),
                          ServiceLocator.Get<IQuestService>(),
                          DIContainer.Resolve<PlayerCtrl>());
        }
    }
}
