using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("팩토리 매니저")]
    [SerializeField] private FactoryManager m_factory_manager;
    private SpawnerManager m_spawner_manager;

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

    private int m_current_count;

    public int Count
    {
        get => m_current_count;
        set => m_current_count = value;
    }

    public int ID => m_id;

    private void Awake()
    {
        m_spawner_manager = transform.parent.GetComponent<SpawnerManager>();

        StartCoroutine(Co_SpawnEnemy());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_spawn_radius);       
    }

    private IEnumerator Co_SpawnEnemy()
    {
        float elasped_time = 0f;

        while (true)
        {
            yield return new WaitUntil(() => m_current_count < m_max_count);

            while (elasped_time <= m_spawn_interval)
            {
                elasped_time += Time.deltaTime;
                yield return null;
            }

            CreateEnemy();

            m_current_count++;

            elasped_time = 0f;
        }
    }

    public void UpdateCount(int count)
    {
        m_current_count += count;
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
        enemy_ctrl.Initialize(so, m_spawner_manager, ID);
    }   
}
