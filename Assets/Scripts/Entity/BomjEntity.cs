using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class BomjEntity : Entity
{

    void Awake()
    {
        // TODO: This might be removed. Then step() function refactored to use CanCollide instead of this 'list of interest'
        interestList = new List<EntityType>(){
            EntityType.musor
        };
    }
    public override void OnCollision(Entity collider)
    {
        if (collider == null)
        {
            return;
        }
        CollisionResolver.Resolve(this, collider);

    }
}
