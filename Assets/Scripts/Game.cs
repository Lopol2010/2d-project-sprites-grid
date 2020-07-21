using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class Game : MonoBehaviour
{


    public List<Entity> entityPrefabs = new List<Entity>();

    public Grid grid;
    public MainSlot mainSlot;
    public List<Entity> entities = new List<Entity>();
    public CameraManager cameraManager;

    public int musorCount = 5;


    void Start()
    {
     

        cameraManager = cameraManager ? cameraManager : Camera.main.GetComponent<CameraManager>();

        grid.Generate();
        cameraManager.FitContent();
        CollisionResolver.Init(this);

        for (int i = 0; i < musorCount; i++)
        {
            Spawn<Musor>(grid.RandomCell());
        }
        //Spawn<Musor>(grid.GetCell(0, 0));
        //Spawn<Ded>(grid.GetCell(0, 1));
        Spawn<Svin>(grid.GetCell(1, 0));
        Spawn<Svin>(grid.GetCell(1, 0));

        mainSlot.game = this;
        mainSlot.grid = grid;
    }

    void Update()
    {

    }

    public void DoStep()
    {
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
