using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUIMode : MonoBehaviour
{
    public GameObject startup, gridGenerator, pathfindingStarter;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideAllUI();
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            HideAllUI();
            gridGenerator.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            HideAllUI();
            pathfindingStarter.SetActive(true);
        }
    }

    void HideAllUI()
    {
        startup.SetActive(false);
        gridGenerator.SetActive(false);
        pathfindingStarter.SetActive(false);
    }

    void Start()
    {
        HideAllUI();
        startup.SetActive(true);
    }
}
