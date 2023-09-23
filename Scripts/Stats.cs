using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool isSearching;

    public int explored;
    public float closest;
    public float shortestPath;

    public TMP_Text currentAlgorithm, exploredText, closestText, shortestPathText;
    public string currentAlgorithmStarter, exploredStarter, closestStarter, shortestPathStarter;

    public void StartSearch(string alg)
    {
        explored = 0;
        closest = float.PositiveInfinity;
        shortestPath = 1;
        currentAlgorithm.text = currentAlgorithmStarter + alg;
        isSearching = true;
        shortestPathText.text = shortestPathStarter + "None";
    }
    void LateUpdate()
    {
        if (isSearching)
        {
            exploredText.text = exploredStarter + explored.ToString();
            if (closest == 0) { closestText.text = closestStarter + "0un"; }
            else { closestText.text = closestStarter + closest.ToString("#.#") + "un"; };
        }
        else
        {
            shortestPathText.text = shortestPathStarter + shortestPath.ToString("#.#") +"un";
        }
    }

    public void Stop()
    {
        exploredText.text = exploredStarter + explored.ToString();
        closestText.text = closestStarter + closest.ToString("#.#") + "un";
        shortestPathText.text = shortestPathStarter + shortestPath.ToString("#.#") +"un";
        isSearching = false;
    }
}
