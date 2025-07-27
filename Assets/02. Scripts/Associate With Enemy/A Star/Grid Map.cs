using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [Header("맵의 크기")]
    [SerializeField] private Vector2 m_map_size;

    [Header("노드의 크기")]
    [SerializeField] private float m_node_size;

    [Header("장애물 레이어")]
    [SerializeField] private LayerMask m_obstacle_layer;

    [Space(30f)]
    [Header("예상 경로")]
    [SerializeField] private List<Node> m_path;

    private Node[,] m_grid;
    private int m_row_count;
    private int m_col_count;

    public List<Node> Path
    {
        get => m_path;
        set => m_path = value;
    }

    private void Awake()
    {
        Initialize();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, m_map_size);
        if (m_grid != null)
        {
            foreach (var node in m_grid)
            {
                Gizmos.color = node.CanWalk ? Color.white : Color.red;

                if (Path != null)
                {
                    if (Path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.World, Vector2.one * (m_node_size / 2));
            }
        }
    }

    #region Methods
    private void Initialize()
    {
        m_row_count = Mathf.CeilToInt(m_map_size.y / m_node_size);
        m_col_count = Mathf.CeilToInt(m_map_size.x / m_node_size);

        var top_left_offset = (Vector2)transform.position + new Vector2(-m_map_size.x, m_map_size.y) / 2f;

        m_grid = new Node[m_col_count, m_row_count];
        for (int col = 0; col < m_col_count; col++)
        {
            for (int row = 0; row < m_row_count; row++)
            {
                var position = top_left_offset + new Vector2((col + 0.5f) * m_node_size, -(row + 0.5f) * m_node_size);

                var hit = Physics2D.OverlapBox(position, new Vector2(m_node_size, m_node_size), 0f, m_obstacle_layer);
                m_grid[col, row] = new Node(hit == null, position, new Vector2(col, row));
            }
        }
    }

    public Node GetNode(Vector3 position)
    {
        int row = Mathf.RoundToInt(position.y / m_node_size);
        int col = Mathf.RoundToInt(position.x / m_node_size);

        if (0 <= row && row < m_row_count && 0 <= col && col < m_col_count)
        {
            return m_grid[col, row];
        }

        return null;
    }

    public List<Node> GetNeighborNode(Node node)
    {
        var node_list = new List<Node>();

        for (int col = -1; col < 2; col++)
        {
            for (int row = -1; row < 2; row++)
            {
                if (col == 0 && row == 0)
                {
                    continue;
                }

                var new_col = (int)node.Local.x + col;
                var new_row = (int)node.Local.y + row;

                if (0 <= new_row && new_row < m_row_count && 0 <= new_col && new_col < m_col_count)
                {
                    node_list.Add(m_grid[new_col, new_row]);
                }
            }
        }

        return node_list;
    }
    #endregion Methods
}
