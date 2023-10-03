using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTester : MonoBehaviour
{
    public GridManager g;
    public GridManager.DistanceMetric metric;
    public Color dist1, dist2, dist3, dist4;
    public void ShowDistances()
    {
        for(int x = 0; x < g.gridWidth; x++)
        {
            for (int y = 0; y < g.gridHeight; y++)
            {
                switch (g.Distance(g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[x, y], metric))
                {
                    case 1:
                        g.squareRenderers[x, y].color = dist1;
                        break;
                    case 2:
                        g.squareRenderers[x, y].color = dist2;
                        break;
                    case 3:
                        g.squareRenderers[x, y].color = dist3;
                        break;
                    case 4:
                        g.squareRenderers[x, y].color = dist4;
                        break;
                }
            }
        }
    }
}
