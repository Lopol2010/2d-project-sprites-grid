using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SvinEntity : Entity
{

    void Awake()
    {
        type = EntityType.svin;
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
