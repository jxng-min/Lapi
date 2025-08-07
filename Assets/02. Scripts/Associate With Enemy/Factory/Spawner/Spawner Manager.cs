using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("스포너의 부모 트랜스폼")]
    [SerializeField] private Transform m_spawner_root;

    private EnemySpawner[] m_spawner_list;
    private Dictionary<int, EnemySpawner> m_spawner_dict;

    private void Awake()
    {
        m_spawner_list = m_spawner_root.GetComponentsInChildren<EnemySpawner>();

        m_spawner_dict = new();
        foreach (var spawner in m_spawner_list)
        {
            m_spawner_dict.Add(spawner.ID, spawner);
        }
    }
}