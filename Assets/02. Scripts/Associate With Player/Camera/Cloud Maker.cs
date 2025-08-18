using System.Collections.Generic;
using UnityEngine;

public class CloudMaker : MonoBehaviour
{
    #region Variables
    [Header("구름이 보일 영역")]
    [SerializeField] private Vector2 m_cloud_space;

    [Header("구름 오브젝트의 목록")]
    [SerializeField] private List<CloudTable> m_cloud_list;

    #endregion Variables

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        for (int i = 0; i < m_cloud_list.Count; i++)
        {
            m_cloud_list[i].Cloud.transform.Translate(Vector2.right * Time.deltaTime * m_cloud_list[i].SPD);

            if (Mathf.Abs(m_cloud_list[i].Cloud.transform.localPosition.x - m_cloud_space.x / 2) < 0.1f)
            {
                ResetPosition(m_cloud_list[i]);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, m_cloud_space);
    }

    #region Helper Methods
    private void Initialize()
    {
        for (int i = 0; i < m_cloud_list.Count; i++)
        {
            m_cloud_list[i].Cloud.transform.localPosition = new Vector3(Random.Range(-m_cloud_space.x / 2, m_cloud_space.x / 2),
                                                                        Random.Range(-m_cloud_space.y / 2, m_cloud_space.y / 2),
                                                                         10f);
            m_cloud_list[i].SPD = Random.Range(1f, 4f);
        }        
    }
    private void ResetPosition(CloudTable table)
    {
        table.Cloud.transform.localPosition = new Vector3(-m_cloud_space.x / 2,
                                                          Random.Range(-m_cloud_space.y / 2, m_cloud_space.y / 2),
                                                           10f);
        table.SPD = Random.Range(1f, 4f);
    }
    #endregion Helper Methods
}