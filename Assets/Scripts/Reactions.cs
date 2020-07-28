using System.Collections.Generic;

public static class Reactions
{

    public enum ReactionType
    {
        Chase,
        Runaway,
    }

    public class Reaction
    {
        public EntityType self;
        public EntityType other;
        public ReactionType type;

        public Reaction(EntityType self, EntityType other, ReactionType type)
        {
            this.self = self;
            this.other = other;
            this.type = type;
        }
    }

    //public static Dictionary<EntityType, Reaction> reactions = new Dictionary<EntityType, Reaction>();
    private static List<Reaction> reactions = new List<Reaction>();


    public static void Init()
    {
        Add(EntityType.Dvornik, EntityType.Musorka, ReactionType.Chase);
    }

    private static void Add(EntityType self, EntityType other, ReactionType type)
    {
        reactions.Add(new Reaction(self, other, type));
    }

    //public static bool Exists(EntityType self, EntityType other)
    //{
    //    return reactions
    //}
    //public static ReactionType Get(EntityType self, EntityType other)
    //{
    //    return reactions
    //}
}
