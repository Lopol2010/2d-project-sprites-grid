using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BomzhEntity : Entity
{

    void Awake()
    {
        type = EntityType.Bomzh;

        predatorList.Add(EntityType.Masson);
        predatorList.Add(EntityType.Tank);
    }

    public override void Step()
    {
        var neighbours = grid.GetNeighbors(currentCell);
        neighbours = neighbours.FindAll(e => CollisionResolver.CanCollide(this, e.GetLast()));

        if (neighbours.Count > 0)
        {
            var randomNbor = neighbours[Random.Range(0, neighbours.Count)];
            nextCell = randomNbor;
            AttachTo(nextCell);
            isMoving = true;
        }
    }
}
