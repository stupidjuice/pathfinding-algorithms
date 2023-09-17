using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUIMode : MonoBehaviour
{
    public RectTransform startup, gridGenerator, pathfindingStarter;
    public float hiddenX;
    public float shownX;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideAllUI();
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            HideAllUI();
            ShowUIElement(gridGenerator);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            HideAllUI();
            ShowUIElement(pathfindingStarter);
        }
    }

    void HideAllUI()
    {
        startup.localPosition = new Vector3(hiddenX, startup.localPosition.y);
        gridGenerator.localPosition = new Vector3(hiddenX, gridGenerator.localPosition.y);
        pathfindingStarter.localPosition = new Vector3(hiddenX, pathfindingStarter.localPosition.y);
    }
    void ShowUIElement(RectTransform ui)
    {
        ui.localPosition = new Vector3(shownX, ui.localPosition.y);
    }

    void Start()
    {
        startup.gameObject.SetActive(true);
        gridGenerator.gameObject.SetActive(true);
        pathfindingStarter.gameObject.SetActive(true);
        HideAllUI();
        ShowUIElement(startup);
    }
}
