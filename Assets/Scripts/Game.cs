using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private List<Entity> entityPrefabs = new List<Entity>();
    [SerializeField]
    private List<Entity> entities = new List<Entity>();
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private MainSlot mainSlot;
    [SerializeField]
    private CameraManager cameraManager;
    [SerializeField]
    private CanvasController canvasController;

    public int musorCount = 5;
    public int stepCount = 0;
    public int dayNightLength = 8;

    void Start()
    {
     

        //cameraManager = Camera.main.GetComponent<CameraManager>();

        grid.Generate();
        cameraManager.FitContent();
        CollisionResolver.Init(this);

        for (int i = 0; i < musorCount; i++)
        {
            Spawn<MusorEntity>(grid.RandomCell());
        }
        //Spawn<Musor>(grid.GetCell(0, 0));
        Spawn<BomjEntity>(grid.GetCell(0, 1));
        Spawn<SvinEntity>(grid.GetCell(1, 0));
        Spawn<SvinEntity>(grid.GetCell(1, 0));

        mainSlot.game = this;
        mainSlot.grid = grid;
        mainSlot.SetRandomItem();
    }

    void Update()
    {

    }

    public Entity GetRandomEntity(EntityType type)
    {
        var entitiesOfType = entities.FindAll(e => e.type == type);
        if (entitiesOfType != null && entitiesOfType.Count > 0)
        {
            return entitiesOfType[Random.Range(0, entitiesOfType.Count)];
        }
        return null;
    }

    public void DoStep()
    {

        canvasController.TimeOfDay = stepCount % dayNightLength;
        stepCount++;
        
        if (entities.Exists(e => e.isMoving))
        {
            return;
        }
        foreach (var entity in entities)
        {
            entity.Step();
        }
    }



    public void Spawn<T>(Cell at) where T : Entity
    {
        var prefab = entityPrefabs.Find(e => e is T);
        if (prefab != null)
        {
            Spawn(prefab.type, at);
        }
    }

    public void Spawn(EntityType type, Cell at)
    {
        var prefab = entityPrefabs.Find(e => e.type == type);
        if (prefab != null)
        {
            var entity = Instantiate(prefab, GameObject.Find("Entities").transform);
            entity.Init(this, grid);
            entity.AttachTo(at);
            entity.SetPosition(at);
            entity.OnSpawn();
            entities.Add(entity);
        }
        else
        {
            Debug.LogError("Prefab not found.");
        }
    }

    public void Despawn(Entity entity)
    {
        entities.Remove(entity);
        entity.currentCell.Remove(entity);
        Destroy(entity.gameObject);
    }
}
