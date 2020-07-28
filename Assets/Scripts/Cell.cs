using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public Vector2Int position
    {
        get => new Vector2Int((int)transform.position.x, (int)transform.position.y);
        set => transform.position = new Vector3(value.x, value.y);
    }
    public int x
    {
        get => position.x;
        set => position = new Vector2Int(value, position.y);
    }
    public int y
    {
        get => position.y;
        set => position = new Vector2Int(position.x, value);
    }
    public bool IsEmpty
    {
        get { return content.Count <= 0; }
    }

    private Game game;
    private Transform grid;
    private Image background;
    private Text debug;

    public List<Entity> content = new List<Entity>();

    void Awake()
    {
        game = FindObjectOfType<Game>();
        grid = FindObjectOfType<Grid>()?.transform;
        background = transform.Find("background")?.GetComponent<Image>();
        debug = transform.Find("debug")?.GetComponent<Text>();
    }

    void Update()
    {

    }

    //public Entity GetFirst()
    //{
    //    if (content.Count > 0)
    //    {
    //        return content[0];
    //    }
    //    return null;
    //}

    public Entity GetLast()
    {
        if (content.Count > 0)
        {
            return content[content.Count - 1];
        }
        return null;
    }
    public Entity GetBefore(Entity entity)
    {
        if (content.Count > 0)
        {
            var entIndex = content.IndexOf(entity);
            if (entIndex > 0)
            {
                return content[entIndex - 1];
            }
        }
        return null;
    }
    public bool Contains(Entity entity)
    {
        return content.Contains(entity);
    }
    public bool Contains(EntityType type)
    {
        return content.Exists(e => e.type == type);
    }

    public void Push(Entity entity)
    {
        content.Add(entity);
    }

    public void Remove(Entity entity)
    {
        content.Remove(entity);
    }


    public void SetColor(Color c)
    {
        if (background)
        {
            background.color = c;
        }
    }

    public void ShowDebug(string text)
    {
        if (debug)
        {
            debug.text = text;
        }
    }

    private void OnMouseDown()
    {
        SetColor(Color.blue);
        Debug.Log($"click {x}  {y}");
    }

    private void OnMouseUp()
    {
    }

    public void OnDropItem(Item item)
    {
        game.Spawn(item.type, this);
        game.DoStep();
    }

    private void OnGUI()
    {
        if (game != null && game.DebugCellContent)
        {
            float padding = 0.2f;
            var cellWorldPos = transform.position;
            cellWorldPos.x -= 0.5f - padding;
            cellWorldPos.y += 0.5f - padding;
            var cellPosOnScreen = Camera.main.WorldToScreenPoint(cellWorldPos);

            cellPosOnScreen.y = Screen.height - cellPosOnScreen.y;


            GUILayout.BeginArea(new Rect(cellPosOnScreen.x, cellPosOnScreen.y, 100, 100));
            GUILayout.Label($"{x} {y}");

            foreach (var entity in content)
            {

                GUILayout.Label(entity.type.ToString());
            }
            GUILayout.EndArea();
        }

    }
}
