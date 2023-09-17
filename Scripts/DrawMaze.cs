using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawMaze : MonoBehaviour
{
    public GridManager g;
    public Camera cam;
    public bool mazeDrawMode, setStartMode, setGoalMode;
    public TMP_Text enterOrExit;
    public Button generate, setStart, setGoal, pathfind, mazeDraw;
    public Vector2Int currentStart = Vector2Int.left, currentGoal = Vector2Int.left;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 coordinates = Input.mousePosition;
            coordinates.x += g.gridWidth % 2 == 0 ? -1.0f : -0.5f;
            coordinates.y += g.gridHeight % 2 == 0 ? -1.0f : -0.5f;

            coordinates = cam.ScreenToWorldPoint(coordinates);

            coordinates.x += g.offsetX;
            coordinates.y += g.offsetY;

            //Debug.Log(coordinates);
            //Debug.Log(g.gridHeight);
            Vector2Int coordinatesInt;
            //idk why the fuck this doesnt count 2d objects but it doesnt
            //this detects if the mouse is currently over the ui
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                coordinatesInt = Vector2Int.RoundToInt(coordinates);
                Debug.Log(coordinatesInt);
             
                //make sure this shit aint oob
                if(coordinatesInt.x >= 0 && coordinatesInt.x < g.gridWidth && coordinatesInt.y >= 0 && coordinatesInt.y < g.gridHeight)
                {
                    if (mazeDrawMode)
                    {
                        g.UpdateNode(coordinatesInt.x, coordinatesInt.y, GridManager.NodeType.Obstacle);
                    }
                    else if (setStartMode)
                    {
                        if (currentStart != Vector2Int.left) { g.UpdateNode(currentStart.x, currentStart.y, GridManager.NodeType.Unexplored); }
                        g.UpdateNode(coordinatesInt.x, coordinatesInt.y, GridManager.NodeType.Root);
                        currentStart = coordinatesInt;
                        g.startCoord = currentStart;
                        setStartMode = false;
                        mazeDraw.interactable = true;
                        setGoal.interactable = true;
                    }
                    else if (setGoalMode)
                    {
                        if (currentGoal != Vector2Int.left) { g.UpdateNode(currentGoal.x, currentGoal.y, GridManager.NodeType.Unexplored); }
                        g.UpdateNode(coordinatesInt.x, coordinatesInt.y, GridManager.NodeType.Goal);
                        currentGoal = coordinatesInt;
                        g.endCoord = currentGoal;
                        setGoalMode = false;
                        mazeDraw.interactable = true;
                        setStart.interactable = true;
                    }
                }
            }            
        }
    }

    public void MazeDrawMode()
    {
        mazeDrawMode = !mazeDrawMode;
        enterOrExit.text = mazeDrawMode ? "Exit Maze Drawing Mode" : "Enter Maze Drawing Mode";
        setGoal.interactable = !mazeDrawMode;
        setStart.interactable = !mazeDrawMode;
    }
    public void SetStart()
    {
        generate.interactable = false;
        pathfind.interactable = false;
        setGoal.interactable = false;
        mazeDraw.interactable = false;
        setStartMode = !setStartMode;
    }
    public void SetGoal()
    {
        generate.interactable = false;
        pathfind.interactable = false;
        setStart.interactable = false;
        mazeDraw.interactable = false;
        setGoalMode = !setGoalMode;
        pathfind.interactable = true;
    }
}
