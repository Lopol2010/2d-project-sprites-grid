using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// Dijkstra pathfinding algorythm optimized for grid-graphs
/// </summary>
public class DijkstraGrid
{

    Node start;
    Node end;
    Node[,] nodes;
    int columns;
    int rows;

    public DijkstraGrid(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;

        nodes = new Node[columns, rows];

        for (int x = 0; x < this.columns; x++)
        {
            for (int y = 0; y < this.rows; y++)
            {
                Node node = new Node(new Vector2Int(x, y));
                nodes[x, y] = node;
            }
        }
    }

    public void SetStart(Vector2Int p)
    {
        this.start = nodes[p.x, p.y];
        this.start.cost = 0;
    }
    public void SetEnd(Vector2Int p)
    {
        this.end = nodes[p.x, p.y];
    }
    public void Reset()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var node = nodes[x, y];
                node.Reset();
            }
        }
    }
    public void UpdateNodes(Func<Node, int, int, bool> callback)
    {
        Reset();
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var node = nodes[x, y];
                callback(node, x, y);
            }
        }
    }
    public List<Node> GetShortestPath(Vector2Int startPos, Vector2Int endPos)
    {

        bool allVisited = false;
        var path = new List<Node>();

        SetStart(startPos);
        SetEnd(endPos);


        while (!allVisited)
        {
            var curNode = FindCheapestNode(true);
            if (curNode == null)
            {
                allVisited = true;
                break;
            }

            var neighbors = getNeighbors(curNode);

            foreach (var nbor in neighbors)
            {
                if (!nbor.visited)
                {
                    exploreNeigbhor(curNode, nbor);
                    nbor.parent = curNode;
                }
            }

            curNode.visited = true;

            //if (curNode == end)
            //{
            //    allVisited = true;
            //    break;
            //}
        }

        Node node = end;
        while (node != start)
        {
            path.Add(node);
            node = node.parent;
        }
        path.Reverse();
        return path;
    }

    public List<Node> GetPath(Node[,] nodes)
    {
        Node node = end;
        var path = new List<Node>();

        var escape = 100;
        while (node != start)
        {
            path.Add(node);
            var neighbors = getNeighbors(node);
            Node cheapestNbor = neighbors[0];
            foreach (var nbor in neighbors)
            {
                if (cheapestNbor.cost > nbor.cost)
                {
                    cheapestNbor = nbor;
                }
            }

            node = cheapestNbor;
            escape--;
            if (escape < 0)
            {
                break;
            }
        }


        return path;
    }

    /// <summary>
    /// Find node with lowest cost.
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="ignoreVisited">Accept visited nodes too.</param>
    /// <returns></returns>
    public Node FindCheapestNode(bool ignoreVisited)
    {
        Node node;
        Node cheapest = null;
        float lowestCost = Mathf.Infinity;


        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int o = 0; o < nodes.GetLength(1); o++)
            {
                node = nodes[i, o];
                if (node.visited && ignoreVisited)
                {
                    continue;
                }
                if (node.cost < lowestCost)
                {
                    cheapest = node;
                    lowestCost = cheapest.cost;
                }
            }
        }
        return cheapest;
    }
    public void exploreNeigbhor(Node curNode, Node nbor)
    {
        if (!nbor.walkable)
        {
            nbor.cost = Mathf.Infinity;
            return;
        }

        // Manhattan's distance 
        //float dist = Vector2Int.Distance(nbor.position, end.position);
        float dist = Mathf.Abs(nbor.position.x - end.position.x) + Mathf.Abs(nbor.position.y - end.position.y);

        if (nbor.cost > curNode.cost + dist)
        {
            nbor.cost = curNode.cost + dist;
        }
    }
    public List<Node> getNeighbors(Node node)
    {

        var neighbors = new List<Node>();
        int x = node.position.x;
        int y = node.position.y;

        if (x != 0)
        {
            neighbors.Add(nodes[x - 1, y]);
        }
        if (x + 1 != columns)
        {
            neighbors.Add(nodes[x + 1, y]);
        }
        if (y != 0)
        {
            neighbors.Add(nodes[x, y - 1]);
        }
        if (y + 1 != rows)
        {
            neighbors.Add(nodes[x, y + 1]);
        }

        return neighbors;
    }
}
