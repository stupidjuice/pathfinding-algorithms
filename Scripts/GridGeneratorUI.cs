using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GridGeneratorUI : MonoBehaviour
{
    public GridManager g;
    public TMP_InputField wInput, hInput;
    public Button mazeDrawMode, setStartButton, setGoalButton;
    public TMP_Text speedLabel;
    public Slider simSpeedSlider;
    public float simSpeed;

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
        simSpeed = simSpeedSlider.value;
        //the "#.##" rounds to the hundreds place
        speedLabel.text = simSpeed.ToString("#.##") + "x";
    }
}
