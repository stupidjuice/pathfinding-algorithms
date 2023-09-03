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
    }
}
