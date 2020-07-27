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
            if (nextCell == null)
            {
                isMoving = false;
            }

            transform.position = Vector3.Lerp(nextCell.transform.position, transform.position, 1 - moveDelta);
            moveDelta += Time.deltaTime * moveSpeed;

            if (moveDelta >= 1)
            {

                SetPosition(currentCell);

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
                isMoving = false;
                nextCell = null;
            }
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

    

    public virtual void Step()
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

    public Entity FindChaseTarget ()
    {
        Entity chaseTarget = null;
        foreach (var interestEntry in preyList)
        {
            var current = game.GetRandomEntity(interestEntry);
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
            node.walkable = OnDetermineWalkableCell(grid.GetCell(_x, _y));

            return true;
        });

        var path = dijkstra.GetShortestPath(position, target.position);
        if (path.Count > 0)
        {
            return grid.GetCell(path[0].position);
        }
        return null;
    }

    public virtual bool OnDetermineWalkableCell(Cell cell)
    {

        return CollisionResolver.CanCollide(this, cell.GetBefore(this));
    }

    public virtual void OnCollision(Entity collider)
    {
        CollisionResolver.Resolve(this, collider);
    }

    //TODO: должнал и коллизия происходить здесь?
    public void AttachTo(Cell to)
    {
        if (to)
        {
            to.Push(this);
            currentCell = to;
        }
    }


    public void SetPosition(Cell cell)
    {
        position = cell.position;
        transform.position = cell.transform.position;
    }
}