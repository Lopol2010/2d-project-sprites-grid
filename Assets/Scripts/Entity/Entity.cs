using System.Collections.Generic;
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
    protected Entity chaseTarget;
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

        if (nextCell != null)
        {
            MoveTowards(nextCell);
        }
    }

    public virtual void Step()
    {
        chaseTarget = FindChaseTarget();

        if (chaseTarget)
        {
            nextCell = GetStepToward(chaseTarget.currentCell);
            AttachTo(nextCell);
            isMoving = nextCell != null;
        }
        else
        {
            var randCell = GetNeighborCell();

            if (randCell != null)
            {
                nextCell = randCell;
                AttachTo(nextCell);
                isMoving = true;
            }
        }

    }

    /// <summary>
    /// Move entity smoothly towards <paramref name="cell"/>'s world position. 
    /// </summary>
    /// <param name="cell">Destination cell.</param>
    /// <returns>True when movement is done.</returns>
    public void MoveTowards(Cell cell)
    {

        moveDelta += Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(nextCell.transform.position, transform.position, 1 - moveDelta);

        if (moveDelta >= 1)
        {

            isMoving = false;
            moveDelta = 0;
            nextCell = null;

            SetPosition(currentCell);

            OnEndMoveTowards();
        }
    }

    private void OnEndMoveTowards()
    {

        var collideWith = currentCell.GetBefore(this);
        if (collideWith)
        {
            OnCollision(collideWith);
        }

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

        var path = dijkstra.GetShortestPath(position, cell.position);
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
    /// Attach entity to new cell (and detach from old)
    /// </summary>
    /// <param name="newCell">Cell to attach to.</param>
    public void AttachTo(Cell newCell)
    {
        if (newCell)
        {
            currentCell?.Remove(this);

            newCell.Push(this);
            currentCell = newCell;
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

    /// <summary>
    /// Find adjacent cell that can be visited by this entity
    /// </summary>
    public Cell GetNeighborCell()
    {
        var neighbours = grid.GetNeighbors(currentCell);
        neighbours = neighbours.FindAll(e => CollisionResolver.CanCollide(this, e.GetLast()));

        if (neighbours.Count > 0)
        {
            var randomNbor = neighbours[Random.Range(0, neighbours.Count)];
            return randomNbor;
        }
        return null;
    }
}