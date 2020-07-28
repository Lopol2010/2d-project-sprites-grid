using System.Collections.Generic;
using UnityEngine;

public class MainSlot : MonoBehaviour
{

    public Item item;
    public List<Item> itemPrefabs = new List<Item>();
    public Game game;
    public Grid grid;

    void Start()
    {

    }



    void Update()
    {

    }

    public void SetRandomItem()
    {
        if (item != null)
        {
            Destroy(item.gameObject);
        }

        var prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        var newItem = Instantiate(prefab, transform);
        item = newItem;

        item.game = game;
        item.grid = grid;
        item.slot = gameObject;
    }

}
