using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UILogicHandler : MonoBehaviour
{
    public GridManager g;
    public TMP_InputField wInput, hInput;
    public Button mazeDrawMode, setStartButton, setGoalButton, generateButton, deleteButton, resetStats, resetMaze;
    public TMP_Text speedLabel;
    public TMP_Dropdown pathfindAlgOption, metricDropdown;
    public Slider simSpeedSlider;
    public DrawMaze drawMaze;
    public int simSpeed;

    public DepthFirstSearch dfs;
    public BreadthFirstSearch bfs;
    public Greedy greedyfs;
    public AStarSearch astar;

    public SaveLoad saver;
    public float timeSinceLastPressedEnter;
    public float timeBetweenEnterPressThreshold;

    public Toggle showStats;
    public SwitchUIMode uIMode;

    public GridManager.DistanceMetric metric;

    public void Generate()
    {
        int width = 0, height = 0;
        int.TryParse(wInput.text, out width);
        int.TryParse(hInput.text, out height);

        if (width > 0 && height > 0)
        {
            g.gridWidth = width;
            g.gridHeight = height;
            g.GenerateGrid();
        }

        mazeDrawMode.interactable = true;
        setStartButton.interactable = true;
        setGoalButton.interactable = true;
        deleteButton.interactable = true;
        generateButton.interactable = false;
    }

    public void DeleteGrid()
    {
        g.DeleteGrid();
        generateButton.interactable = true;
        deleteButton.interactable = false;

        g.startCoord = Vector2Int.left;
        g.endCoord = Vector2Int.left;
        drawMaze.currentStart = Vector2Int.left;
        drawMaze.currentGoal = Vector2Int.left;
    }

    private void Start()
    {
        mazeDrawMode.interactable = false;
        setStartButton.interactable = false;
        setGoalButton.interactable = false;
        deleteButton.interactable = false;
    }
    public void ChangeSimSpeed()
    {
        simSpeed = (int)simSpeedSlider.value;
        speedLabel.text = simSpeed.ToString() + "x";
    }
    public void StartPathfind()
    {
        mazeDrawMode.interactable = false;
        setStartButton.interactable = false;
        setGoalButton.interactable = false;
        deleteButton.interactable = false;
        resetStats.interactable = false;
        resetMaze.interactable = false;

        if(showStats.isOn)
        {
            uIMode.HideAllUI();
            uIMode.ShowUIElement(uIMode.stats);
        }

        saver.Save("BEFORE_PATHFIND_START", g.currentGrid);

        switch(metricDropdown.value)
        {
            case 0:
                metric = GridManager.DistanceMetric.Absolute;
                break;
            case 1:
                metric = GridManager.DistanceMetric.Manhattan;
                break;
            case 2:
                metric = GridManager.DistanceMetric.Chebyshev; 
                break;
        }

        switch(pathfindAlgOption.value)
        {
            case 0:
                StartCoroutine(dfs.DFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], metric));
                break;
            case 1:
                StartCoroutine(bfs.BFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], metric));
                break;
            case 2:
                StartCoroutine(greedyfs.GreedyBestFirstSearch(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], false, metric));
                break;
            case 3:
                StartCoroutine(greedyfs.GreedyBestFirstSearch(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], true, metric));
                break;
            case 4:
                StartCoroutine(astar.AStar(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], false, metric));
                break;
            case 5:
                StartCoroutine(astar.AStar(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y], true, metric));
                break;
        }
    }

    private void Update()
    {
        timeSinceLastPressedEnter += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(timeSinceLastPressedEnter < timeBetweenEnterPressThreshold)
            {
                StartPathfind();
            }
            timeSinceLastPressedEnter = 0f;
        }
    }

    public void ResetPathfind()
    {
        saver.Load("BEFORE_PATHFIND_START");
    }

    public void PathfindEnded()
    {
        g.UpdateRed();

        mazeDrawMode.interactable = true;
        setStartButton.interactable = true;
        setGoalButton.interactable = true;
        deleteButton.interactable = true;
        resetStats.interactable = true;
        resetMaze.interactable = true;
    }
}
