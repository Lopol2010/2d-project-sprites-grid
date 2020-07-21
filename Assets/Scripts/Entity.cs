using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum EntityType
{
    ded,
    musor,
    svin,
    banan,
    durak
}

public class Entity : MonoBehaviour
{


    protected Game game;
    protected Grid grid;

    public EntityType type;
    public Vector2Int position;
    public int x { get => position.x; set => position = new Vector2Int(value, position.y); }
    public int y { get => position.y; set => position = new Vector2Int(position.x, value); }

    public List<EntityType> interestList = new List<EntityType>();

    public float StepInterval = 0.25f;
    protected float stepTime;

    public Dijkstra dijkstra;
    public bool showNeighbors = true;
    public bool showPath = true;
    public bool showDistances = true;


    //private List<Node> path;
    protected Entity target;
    public bool isMoving;
    protected float moveDelta;
    [SerializeField]
    protected float moveSpeed;
    protected Cell nextCell;
    protected Transform moveFrom;

    public Cell currentCell;

    void Start()
    {

    }


    public void Init(Game game, Grid grid)
    {
        this.game = game;
        this.grid = grid;
        dijkstra = new Dijkstra(grid.columns, grid.rows);
    }

    void Update()
    {

        if (isMoving)
        {
            if (nextCell == null)
            {
                isMoving = false;
            }

            Vector3 dir = (nextCell.transform.position - moveFrom.position);
            Vector3 startOffset = dir * (moveDelta);
            transform.position = moveFrom.position + startOffset;

            moveDelta += Time.deltaTime * moveSpeed;
            moveDelta = Mathf.Clamp(moveDelta, 0, 1);

            if (moveDelta >= 1)
            {

                SetPosition(nextCell);
                var collideWith = nextCell.GetBefore(this);
                if (collideWith)
                {
                    OnCollision(collideWith);
                }
                currentCell = nextCell;


                target = null;
                moveDelta = 0;
                isMoving = false;
                nextCell = null;
                moveFrom = null;
            }
        }
        else
        {
            if (target != null && nextCell != null)
            {

                if (nextCell.content.Count > 0)
                {
                    if (CollisionResolver.CanCollide(this, nextCell.GetLast()))
                    {
                        isMoving = true;
                        moveFrom = currentCell.transform;
                        currentCell.Remove(this);
                        nextCell.Push(this);
                    }
                }
                else
                {
                    isMoving = true;
                    moveFrom = currentCell.transform;
                    currentCell.Remove(this);
                    nextCell.Push(this);
                }
            }
        }
    }

    public virtual void Step()
    {
        if (target == null)
        {
            Entity interestEntity = null;
            foreach (var interestEntry in interestList)
            {
                var current = game.entities.Find(e => e.type == interestEntry);
                if (current != null)
                {
                    interestEntity = current;
                    break;
                }
            }

            if (interestEntity)
            {
                target = interestEntity;
            }
            else
            {
                return;
            }
        }

        var targetCell = grid.GetCell(target.position);
        nextCell = GetStepTowardCell(targetCell);
    }

    public Cell GetStepTowardCell(Cell cell)
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
        Debug.Log($"{type} is collided with {collider.type} at {position}");
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