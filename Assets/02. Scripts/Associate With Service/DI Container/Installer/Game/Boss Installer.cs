using System.Collections.Generic;
using InventoryService;
using QuestService;
using UnityEngine;
using UserService;

public class BossInstaller : MonoBehaviour, IInstaller
{
    [Header("보스 스포너 목록")]
    [SerializeField] private List<BossSpawner> m_boss_spawner_list;

    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    [Header("플레이어 컨트롤러")]
    [SerializeField] private PlayerCtrl m_player_ctrl;

    public void Install()
    {
        foreach (var spawner in m_boss_spawner_list)
        {
            spawner.Inject(ServiceLocator.Get<IInventoryService>(),
                           ServiceLocator.Get<IUserService>(),
                           ServiceLocator.Get<IQuestService>(),
                           m_cursor_db,
                           m_player_ctrl);
        }
    }
}
