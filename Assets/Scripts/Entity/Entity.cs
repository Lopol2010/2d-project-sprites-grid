using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Entity : MonoBehaviour
{


    protected Game game;
    protected Grid grid;
    protected DijkstraGrid dijkstra;

    public List<EntityType> preyList = new List<EntityType>();
    public List<EntityType> predatorList = new List<EntityType>();

    public EntityType type;
    public Cell currentCell;

    public Vector2Int position;
    //{
    //    get => currentCell.position;
    //}
    public int x
    {
        get => position.x;
        set => position = new Vector2Int(value, position.y);
    }
    public int y
    {
        get => position.y;
        set => position = new Vector2Int(position.x, value);
    }


    [SerializeField]
    protected float moveSpeed = 4;
    protected float moveDelta;
    public bool isMoving;
    protected Entity target;
    protected Cell nextCell;
    protected Transform moveFrom;


    public void OnSpawn()
    {
        if (currentCell != null && !currentCell.IsEmpty)
        {
            var collideWith = currentCell.GetBefore(this);
            if (collideWith != null && CollisionResolver.CanCollide(this, collideWith))
            {
                OnCollision(collideWith);
            }
        }
    }



    public void Init(Game game, Grid grid)
    {
        this.game = game;
        this.grid = grid;
        dijkstra = new DijkstraGrid(grid.columns, grid.rows);
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowards(nextCell);
        }
        else
        {
            if (target != null && nextCell != null)
            {

                if (CollisionResolver.CanCollide(this, nextCell.GetLast()))
                {
                    isMoving = true;
                    currentCell.Remove(this);
                    nextCell.Push(this);

                    currentCell = nextCell;
                }

            }
        }

    }
    public void Step()
    {
        if (target == null)
        {
            Entity chaseTarget = FindChaseTarget();

            if (chaseTarget)
            {
                target = chaseTarget;
            }
            else
            {
                return;
            }
        }

        var targetCell = grid.GetCell(target.position);
        nextCell = GetStepToward(targetCell);
    }

    /// <summary>
    /// Move entity smoothly towards <paramref name="cell"/>'s world position. 
    /// </summary>
    /// <param name="cell">Destination cell.</param>
    /// <returns>True when movement is done.</returns>
    public void MoveTowards(Cell cell)
    {
        if (cell == null)
        {
            return;
        }

        moveDelta += Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(nextCell.transform.position, transform.position, 1 - moveDelta);
        if (moveDelta >= 1)
        {
            moveDelta = 0;
            isMoving = false;
            SetPosition(currentCell);

            OnEndMoveTowards();
        }
    }

    private void OnEndMoveTowards()
    {

        if (target != null && position == target.position)
        {
            target = null;
        }

        var collideWith = nextCell.GetBefore(this);
        if (collideWith)
        {
            OnCollision(collideWith);
        }

        moveDelta = 0;
        nextCell = null;
    }



    public Entity FindChaseTarget()
    {
        Entity chaseTarget = null;
        foreach (var interestEntry in preyList)
        {
            var current = game.GetClosestEntity(this, interestEntry);
            if (current != null)
            {
                chaseTarget = current;
                break;
            }
        }
        return chaseTarget;
    }

    public Cell GetStepToward(Cell cell)
    {

        dijkstra.UpdateNodes((node, _x, _y) =>
        {
            node.walkable = CollisionResolver.CanCollide(this, grid.GetCell(_x, _y).GetLast());
            return true;
        });

        var path = dijkstra.GetShortestPath(position, target.position);
        if (path.Count > 0)
        {
            return grid.GetCell(path[0].position);
        }
        return null;
    }

    public virtual void OnCollision(Entity collider)
    {
        CollisionResolver.Resolve(this, collider);
    }

    /// <summary>
    /// Attaches entity to cell.
    /// </summary>
    /// <param name="to">Cell to attach to.</param>
    public void AttachTo(Cell to)
    {
        if (to)
        {
            to.Push(this);
            currentCell = to;
        }
    }


    /// <summary>
    /// Teleport to <paramref name="cell"/>'s world position.
    /// </summary>
    /// <param name="cell">Destination cell.</param>
    public void SetPosition(Cell cell)
    {
        position = cell.position;
        transform.position = cell.transform.position;
    }
}