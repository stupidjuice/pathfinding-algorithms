using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMaze : MonoBehaviour
{
    public GridManager g;
    public Camera cam;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2Int coordinates = Vector2Int.CeilToInt(cam.ScreenToWorldPoint(Input.mousePosition));
            coordinates.x += 
        }
    }
}
