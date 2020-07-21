using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    Game game;
    Grid grid;
    Camera cam;

    void Awake()
    {
        game = FindObjectOfType<Game>();
        grid = game.grid;
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        
    }

    public void FitContent()
    {
        transform.position = new Vector3((float)grid.columns / 2 + 0.5f - 1, 
            (float)grid.rows / 2 + 0.5f - 1, transform.position.z);

        cam.orthographicSize = ((float)grid.columns / 2) / cam.aspect;
    }
}
