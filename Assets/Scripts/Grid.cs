//using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int columns = 9;
    public int rows = 7;

    [SerializeField]
    private Cell cellPrefab;
    [SerializeField]
    private Cell[,] cells;


    void Awake()
    {
    }

    void Update()
    {

    }

    public List<Cell> GetNeighbors(Cell cell)
    {

        if (cell == null)
        {
            return null;
        }

        var neighbors = new List<Cell>();
        int x = cell.position.x;
        int y = cell.position.y;

        if (x != 0)
        {
            neighbors.Add(cells[x - 1, y]);
        }
        if (x + 1 != columns)
        {
            neighbors.Add(cells[x + 1, y]);
        }
        if (y != 0)
        {
            neighbors.Add(cells[x, y - 1]);
        }
        if (y + 1 != rows)
        {
            neighbors.Add(cells[x, y + 1]);
        }

        return neighbors;
    }

    public bool Exists(int x, int y)
    {
        return (x >= 0 && x <= columns - 1) && (y >= 0 && y <= rows - 1);
    }

    public void Generate()
    {
        cells = new Cell[columns, rows];
        for (int x = 0; x < columns; x++)
        {

            for (int y = 0; y < rows; y++)
            {
                var cell = cells[x, y] = Instantiate(cellPrefab, transform);
                cell.position = new Vector2Int(x, y);
            }

        }
    }
    public Cell GetRandomCell(bool empty = true)
    {
        if (empty)
        {
            var empties = GetEmptyCells();
            if (empties.Count > 0)
            {
                return empties[Random.Range(0, empties.Count)];
            }
            else
            {
                return null;
            }
        }

        int x = Random.Range(0, columns);
        int y = Random.Range(0, rows);

        return cells[x, y];
    }

    public Cell GetCell(int x, int y)
    {
        //if ((x < 0 || x > columns - 1) || (y < 0 || y > rows - 1))
        //{
        //    return null;
        //}
        return cells[x, y];
    }

    public Cell GetCell(Vector2Int v)
    {
        return GetCell(v.x, v.y);
    }

    public List<Cell> GetEmptyCells()
    {
        var empty = new List<Cell>();
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var cell = cells[x, y];
                if (cell.IsEmpty)
                {
                    empty.Add(cell);
                }
            }

        }
        return empty;
    }

}
