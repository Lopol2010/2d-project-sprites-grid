using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

public class Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public Vector3 defaultSize;
    public Vector3 defaultPosition;
    public Vector2 cursorStartDragPosition;
    public float sizeOverSlot = 1.3f;
    public float sizeOverGrid = 0.7f;

    public EntityType type;

    public RectTransform rectTransform;
    public GameObject slot;
    public Game game;
    public Grid grid;

    void Start()
    {

        rectTransform = (RectTransform)transform;
        defaultSize = rectTransform.localScale;
        defaultPosition = rectTransform.anchoredPosition;

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.localScale = defaultSize * sizeOverSlot;
        cursorStartDragPosition = eventData.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //var hit = Physics2D.Raycast(ray.origin, ray.direction, 10, LayerMask.GetMask("cell"));
        //if (hit)
        //{
        //    rectTransform.localScale = defaultSize * sizeOverGrid;
        //}
        //else
        //{
        //    rectTransform.localScale = defaultSize * sizeOverSlot;
        //}
        //rectTransform.anchoredPosition = eventData.position - cursorStartDragPosition;

        Vector2 cursorPosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(cursorPosInWorld.x);
        int y = Mathf.RoundToInt(cursorPosInWorld.y);
        Vector2 bindPos = Camera.main.WorldToScreenPoint(new Vector2(x, y));
        if (grid.Exists(x, y))
        {
            rectTransform.localScale = defaultSize * sizeOverGrid;
            //rectTransform.anchoredPosition = bindPos - cursorStartDragPosition;

        }
        else
        {
            rectTransform.localScale = defaultSize * sizeOverSlot;
            //rectTransform.anchoredPosition = eventData.position - cursorStartDragPosition;

        }
        rectTransform.anchoredPosition = eventData.position - cursorStartDragPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.localScale = defaultSize;
        rectTransform.anchoredPosition = Vector3.zero;

        Vector2 cursorPosInWorld = Camera.main.ScreenToWorldPoint(eventData.pointerCurrentRaycast.screenPosition);
        int x = Mathf.RoundToInt(cursorPosInWorld.x);
        int y = Mathf.RoundToInt(cursorPosInWorld.y);

        if (grid.Exists(x, y))
        {
            var dropAtCell = grid.GetCell(x, y);
            dropAtCell.OnDropItem(this);
        }
    }
}
