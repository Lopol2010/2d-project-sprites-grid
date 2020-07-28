public class BuldozerEntity : Entity
{


    private void Awake()
    {
        type = EntityType.Buldozer;

        preyList.Add(EntityType.Svalka);

    }
}

