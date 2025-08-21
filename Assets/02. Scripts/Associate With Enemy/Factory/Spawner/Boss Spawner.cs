using System.Collections;
using InventoryService;
using QuestService;
using UnityEngine;
using UserService;

public class BossSpawner : MonoBehaviour
{
    private IInventoryService m_inventory_service;
    private IUserService m_user_service;
    private IQuestService m_quest_service;
    private ICursorDataBase m_cursor_db;
    private PlayerCtrl m_player_ctrl;

    [Header("스폰될 보스 몬스터 프리펩")]
    [SerializeField] private GameObject m_boss_prefab;

    [Header("스폰될 보스 몬스터 정보")]
    [SerializeField] private Enemy m_so;

    [Header("보스 관련 퀘스트 ID")]
    [SerializeField] private int m_quest_id;

    [Header("스폰 대기 시간")]
    [SerializeField] private float m_spawn_interval = 60f;

    private bool m_can_spawn;

    public void Inject(IInventoryService inventory_service,
                       IUserService user_service,
                       IQuestService quest_service,
                       ICursorDataBase cursor_db,
                       PlayerCtrl player_ctrl)
    {
        m_inventory_service = inventory_service;
        m_user_service = user_service;
        m_quest_service = quest_service;
        m_cursor_db = cursor_db;
        m_player_ctrl = player_ctrl;

        StartCoroutine(Co_SpawnBoss());
    }

    private IEnumerator Co_SpawnBoss()
    {
        float elapsed_time = 0f;

        CreateBoss();

        while (true)
        {
            if (m_quest_service.GetQuestState(m_quest_id) == QuestState.CLEARED)
            {
                yield break;
            }

            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
            yield return new WaitUntil(() => m_can_spawn == true);

            elapsed_time = 0f;

            while (elapsed_time <= m_spawn_interval)
            {
                yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
                elapsed_time += Time.deltaTime;
                yield return null;
            }

            CreateBoss();
        }
    }

    private void CreateBoss()
    {
        var boss_obj = Instantiate(m_boss_prefab, transform.position, Quaternion.identity);

        var boss = boss_obj.GetComponent<BossCtrl>();
        boss.Initialize(m_so,
                        m_inventory_service,
                        m_user_service,
                        m_quest_service,
                        m_player_ctrl);

        var mouse_detector = boss_obj.GetComponent<MouseDetector>();
        mouse_detector.Inject(m_cursor_db);

        boss.Status.OnDead -= OnEnemyDeadHandler;
        boss.Status.OnDead += OnEnemyDeadHandler;
    }

    private void OnEnemyDeadHandler(BossCtrl boss_ctrl)
    {
        m_can_spawn = true;

        boss_ctrl.Status.OnDead -= OnEnemyDeadHandler;
    }
}
