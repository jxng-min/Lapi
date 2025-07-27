using UnityEngine;

public class Node
{
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;

    public Node Parent { get; set; }

    public Vector2 Local { get; set; }
    public Vector2 World { get; set; }

    public bool CanWalk { get; set; }

    public Node(bool can_walk, Vector2 world_pos, Vector2 local_pos)
    {
        CanWalk = can_walk;

        World = world_pos;
        Local = local_pos;
    }
}