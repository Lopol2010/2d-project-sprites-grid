﻿using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{


    public bool DebugCellContent = false;

    [Space]

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

    public int stepCount = 0;
    public int dayNightLength = 8;

    // TODO: move this to level data
    public int musorCount = 5;

    void Start()
    {


        //cameraManager = Camera.main.GetComponent<CameraManager>();

        grid.Generate();
        cameraManager.FitContent();
        CollisionResolver.Init(this);

        for (int i = 0; i < musorCount; i++)
        {
            Spawn<MusorkaEntity>(grid.GetRandomCell());
        }
        //Spawn<Musor>(grid.GetCell(0, 0));
        Spawn<BomzhEntity>(grid.GetRandomCell());
        Spawn(EntityType.Dvornik, grid.GetRandomCell());

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
    public Entity GetClosestEntity(Entity entity, EntityType type)
    {
        Entity closest = null;
        float minDist = Mathf.Infinity;
        var entitiesOfType = entities.FindAll(e => e.type == type);
        foreach (var e in entitiesOfType)
        {
            var dist = Vector3.Distance(entity.transform.position, e.transform.position);
            if (minDist > dist)
            {
                minDist = dist;
                closest = e;
            }
        }
        return closest;
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
