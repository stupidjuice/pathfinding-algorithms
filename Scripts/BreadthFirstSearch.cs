using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    public GridManager g;
    public GridGeneratorUI gUI;
    private int runThisFrameCounter;

    IEnumerator BFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        bool foundPath = false;
        Queue<Node> queue = new Queue<Node>();
        root.type = GridManager.NodeType.Explored;
        queue.Enqueue(root);
        Node traceback = null;

        while (queue.Count > 0)
        {
            Node v = queue.Dequeue();

            if (v == goal)
            {
                foundPath = true;
                traceback = v;
                break;
            }
            foreach (Node neighbor in g.Get4Neighbors(v))
            {
                if (neighbor.type != GridManager.NodeType.Obstacle)
                {
                    if (neighbor.type != GridManager.NodeType.Explored)
                    {
                        foreach (Node neighbor2 in g.Get4Neighbors(neighbor))
                        {
                            if (neighbor2.type == GridManager.NodeType.Unexplored) { g.squareRenderers[neighbor2.x, neighbor2.y].color = g.neigborColor; }
                        }
                        neighbor.parent = v;
                        queue.Enqueue(neighbor);

                        if (neighbor != goal || neighbor.type != GridManager.NodeType.Root)
                        {
                            g.UpdateNode(neighbor.x, neighbor.y, GridManager.NodeType.Explored);
                        }

                        if (runThisFrameCounter > gUI.simSpeed - 1)
                        {
                            runThisFrameCounter = 0;
                            yield return null;
                        }
                    }
                }
            }
            runThisFrameCounter++;
        }

        if (foundPath)
        {
            while (traceback != root)
            {
                traceback = traceback.parent;
                g.UpdateNode(traceback.x, traceback.y, GridManager.NodeType.Path);
                if (runThisFrameCounter > gUI.simSpeed - 1)
                {
                    runThisFrameCounter = 0;
                    yield return null;
                }
                runThisFrameCounter++;
            }
        }
    }

    public void StartSearch()
    {
        Debug.Log(g.startCoord);
        Debug.Log(g.endCoord);
        StartCoroutine(BFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y]));
    }
}
