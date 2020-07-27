using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentEntity : Entity
{
    // Start is called before the first frame update
    void Awake()
    {
        type = EntityType.Ment;

        preyList.Add(EntityType.Dvornik);

        predatorList.Add(EntityType.Masson);
        predatorList.Add(EntityType.Tank);
    }


}
