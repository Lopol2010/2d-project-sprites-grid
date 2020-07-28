using UnityEngine;

public class Node
{
    public bool visited;
    public bool walkable = true;
    public float cost = Mathf.Infinity;
    public Vector2Int position;
    public int x => position.x;
    public int y => position.y;
    public Node parent;

    public Node(Vector2Int position)
    {
        this.position = position;
    }

    internal void Reset()
    {
        visited = false;
        walkable = true;
        cost = Mathf.Infinity;
        parent = null;
    }
}
