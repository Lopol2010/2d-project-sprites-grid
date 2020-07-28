public class DvornikEntity : Entity
{
    private void Awake()
    {
        type = EntityType.Dvornik;

        preyList.Add(EntityType.Musorka);
        preyList.Add(EntityType.Poroh);

        predatorList.Add(EntityType.Ment);
        predatorList.Add(EntityType.Masson);
        predatorList.Add(EntityType.Tank);
    }
}

