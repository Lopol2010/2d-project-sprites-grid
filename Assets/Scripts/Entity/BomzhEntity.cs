using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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
        //var neighbours = grid.GetNeighbors(currentCell);
        //if (neighbours.Count > 0)
        //{
        //    var randomNbor = neighbours[Random.Range(0, neighbours.Count)];
        //    nextCell = randomNbor;
        //    isMoving = true;
        //}
    }
}
