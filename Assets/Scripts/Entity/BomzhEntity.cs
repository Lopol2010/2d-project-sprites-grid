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
        var randCell = GetNeighborCell();

        if (randCell != null)
        {
            nextCell = randCell;
            AttachTo(nextCell);
            isMoving = true;
        }
    }
}
