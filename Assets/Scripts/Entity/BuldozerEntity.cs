using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldozerEntity : Entity
{

    
    private void Awake()
    {
        type = EntityType.Buldozer;

        preyList.Add(EntityType.Svalka);

    }
}

