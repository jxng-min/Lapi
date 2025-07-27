using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private readonly int m_diagnol = 14;
    private readonly int m_straight = 10;

    private GridMap m_grid_map;

    public void Inject(GridMap grid_map)
    {
        m_grid_map = grid_map;
    }

    public List<Node> Pathfind(Vector3 start_pos, Vector3 end_pos)
    {
        var open_list = new Heap();
        var closed_list = new HashSet<Node>();

        var start_node = m_grid_map.GetNode(start_pos);
        var end_node = m_grid_map.GetNode(end_pos);

        open_list.Push(start_node);

        while (open_list.Count > 0)
        {
            var current_node = open_list.Pop();
            closed_list.Add(current_node);

            if (current_node == end_node)
            {
                return BackTracking(start_node, end_node);
            }

            foreach (var node in m_grid_map.GetNeighborNode(current_node))
            {
                if (node.CanWalk && !closed_list.Contains(node))
                {
                    var new_cost = current_node.G + GetEuclid(node, current_node);
                    if (new_cost < node.G || !open_list.Contains(node))
                    {
                        node.G = new_cost;
                        node.H = GetManhattan(node, end_node);
                        node.Parent = current_node;

                        open_list.Push(node);
                    }
                }
            }
        }

        return null;
    }

    private List<Node> BackTracking(Node start_node, Node end_node)
    {
        var path = new List<Node>();

        var current_node = end_node;
        while (current_node != start_node)
        {
            path.Add(current_node);
            current_node = current_node.Parent;
        }

        path.Reverse();
        m_grid_map.Path = path;

        return path;
    }

    private int GetEuclid(Node arg1, Node arg2)
    {
        var x = Mathf.Abs((int)arg2.Local.x - (int)arg1.Local.x);
        var y = Mathf.Abs((int)arg2.Local.y - (int)arg1.Local.y);

        return x > y ? m_diagnol * y + m_straight * (x - y) : m_diagnol * x + m_straight * (y - x);
    }

    private int GetManhattan(Node arg1, Node arg2)
    {
        var x = Mathf.Abs((int)arg2.Local.x - (int)arg1.Local.x);
        var y = Mathf.Abs((int)arg2.Local.y - (int)arg1.Local.y);

        return (x + y) * m_straight;
    }
}