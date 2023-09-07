using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawMaze : MonoBehaviour
{
    public GridManager g;
    public Camera cam;
    public bool mazeDrawMode;
    public TMP_Text enterOrExit;

    void Update()
    {
        if(mazeDrawMode)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 coordinates = cam.ScreenToWorldPoint(Input.mousePosition);

                //idk why the fuck this doesnt count 2d objects but it doesnt
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector2Int coordinatesInt;

                    coordinates.x += g.gridWidth % 2 == 0 ? 1.0f : 1.5f;
                    coordinates.y += g.gridHeight % 2 == 0 ? 1.0f : 1.5f;

                    coordinatesInt = Vector2Int.CeilToInt(coordinates);

                    g.UpdateNode(coordinatesInt.x, coordinatesInt.y, GridManager.NodeType.Obstacle);

                    Debug.Log(coordinatesInt.x.ToString() + " " + coordinatesInt.y.ToString());
                }
            }
        }
    }

    public void MazeDrawMode()
    {
        mazeDrawMode = !mazeDrawMode;
        enterOrExit.text = mazeDrawMode ? "Exit Maze Drawing Mode" : "Enter Maze Drawing Mode";
    }
}
