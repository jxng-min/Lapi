using System.Collections;
using System.Collections.Generic;
using InventoryService;
using UnityEngine;
using UserService;

public class EnemySpawner : MonoBehaviour
{
    private IInventoryService m_inventory_service;
    private IUserService m_user_service;
    private ICursorDataBase m_cursor_db;
    private PlayerCtrl m_player_ctrl;

    [Header("팩토리 매니저")]
    [SerializeField] private FactoryManager m_factory_manager;

    [Header("스포너의 고유한 ID")]
    [SerializeField] private int m_id;

    [Header("스폰될 몬스터의 목록")]
    [SerializeField] private List<Enemy> m_enemy_list;

    [Header("최대 몬스터 수")]
    [SerializeField] private int m_max_count;

    [Header("스폰 대기 시간")]
    [SerializeField] private float m_spawn_interval = 5f;

    [Header("스폰 범위")]
    [SerializeField] private float m_spawn_radius;

    private List<EnemyCtrl> m_spawned_enemies;

    public int ID => m_id;

    private void Awake()
    {
        m_spawned_enemies = new();

        StartCoroutine(Co_SpawnEnemy());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_spawn_radius);
    }

    public void Inject(IInventoryService inventory_service,
                       IUserService user_service,
                       ICursorDataBase cursor_db,
                       PlayerCtrl player_ctrl)
    {
        m_inventory_service = inventory_service;
        m_user_service = user_service;
        m_cursor_db = cursor_db;
        m_player_ctrl = player_ctrl;
    }

    private IEnumerator Co_SpawnEnemy()
    {
        float elapsed_time = 0f;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
            yield return new WaitUntil(() => m_spawned_enemies.Count < m_max_count);

            elapsed_time = 0f;

            while (elapsed_time <= m_spawn_interval)
            {
                yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);
                elapsed_time += Time.deltaTime;
                yield return null;
            }

            CreateEnemy();
        }
    }

    private Enemy SelectRandomEnemy()
    {
        if (m_enemy_list.Count == 1)
        {
            return m_enemy_list[0];
        }

        return m_enemy_list[Random.Range(0, m_enemy_list.Count)];
    }

    private Vector2 GetRandomPosition()
    {
        var offset = Random.insideUnitCircle * m_spawn_radius;

        return (Vector2)transform.position + offset;
    }

    private void CreateEnemy()
    {
        var so = SelectRandomEnemy();

        var enemy_ctrl = m_factory_manager.CreateEnemy(so.Code);
        enemy_ctrl.transform.position = GetRandomPosition();
        enemy_ctrl.Initialize(so,
                              m_inventory_service,
                              m_user_service,
                              m_player_ctrl);

        enemy_ctrl.GetComponent<EnemyMouseDetector>().Inject(m_cursor_db);

        m_spawned_enemies.Add(enemy_ctrl);

        enemy_ctrl.Status.OnDead -= OnEnemyDeadHandler;
        enemy_ctrl.Status.OnDead += OnEnemyDeadHandler;
    }

    private void OnEnemyDeadHandler(EnemyCtrl enemy_ctrl)
    {
        m_spawned_enemies.Remove(enemy_ctrl);

        enemy_ctrl.Status.OnDead -= OnEnemyDeadHandler;
    }
}
