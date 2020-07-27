using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public static class CollisionResolver
{


    public class Collision
    {
        public EntityType a;
        public EntityType b;

        public Collision(EntityType a, EntityType b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Equals(object obj)
        {
            if(obj is Collision collision)
            {
                return collision.a == a && collision.b == b;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return $"{a.GetHashCode()}{b.GetHashCode()}".GetHashCode();
        }

    }

    public class CollisionResult
    {
        public virtual void Execute(Entity a, Entity b)
        {

        }
    }
    public class EatResult : CollisionResult
    {
        public override void Execute(Entity a, Entity b)
        {
            //a.currentCell.Remove(a);
            //a.currentCell = b.currentCell;
            //a.currentCell.Push(a);

            game.Despawn(b);
        }
    }

    public class EvolveResult : CollisionResult
    {
        public EntityType newType;

        public EvolveResult(EntityType type)
        {
            newType = type;
        }

        public override void Execute(Entity a, Entity b)
        {
            var cell = b.currentCell; 
            game.Despawn(a);
            game.Despawn(b);
            game.Spawn(newType, cell);
        }
    }

    public static Dictionary<Collision, CollisionResult> CollisionRules = new Dictionary<Collision, CollisionResult>();
    private static Game game;

    public static void Resolve(Entity a, Entity b)
    {

        CollisionResult result;
        if(CollisionRules.TryGetValue(new Collision(a.type, b.type), out result))
        {
            result.Execute(a, b);
        }

    }

    public static bool CanCollide(Entity a, Entity b)
    {
        if (b == null)
        {
            return true;
        }
        CollisionResult result = null;
        return CollisionRules.TryGetValue(new Collision(a.type, b.type), out result);
    }

    public static void Init(Game _game)
    {
        game = _game;
        AddRule(EntityType.Dvornik, EntityType.Musorka, new EatResult());
    }

    public static void AddRule(EntityType a, EntityType b, CollisionResult result)
    {
        CollisionRules.Add(new Collision(a, b), result);
    }
}
