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
    public Button mazeDrawMode, setStartButton, setGoalButton;
    public TMP_Text speedLabel;
    public TMP_Dropdown pathfindAlgOption;
    public Slider simSpeedSlider;
    public int simSpeed;

    public DepthFirstSearch dfs;
    public BreadthFirstSearch bfs;
    public Greedy greedyfs;
    public AStar astar;

    public SaveLoad saver;

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
    }

    private void Start()
    {
        mazeDrawMode.interactable = false;
        setStartButton.interactable = false;
        setGoalButton.interactable= false;
    }
    public void ChangeSimSpeed()
    {
        simSpeed = (int)simSpeedSlider.value;
        speedLabel.text = simSpeed.ToString() + "x";
    }
    public void StartPathfind()
    {
        saver.Save("BEFORE_PATHFIND_START", g.currentGrid);
        switch(pathfindAlgOption.value)
        {
            case 0:
                StartCoroutine(dfs.DFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y]));
                break;
            case 1:
                StartCoroutine(bfs.BFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y]));
                break;

        }
    }
    
    public void ResetPathfind()
    {
        saver.Load("BEFORE_PATHFIND_START");
    }
}
