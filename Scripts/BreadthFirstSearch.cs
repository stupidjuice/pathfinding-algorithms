using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    public GridManager g;
    public GridGeneratorUI gUI;

    IEnumerator BFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        Queue<Node> queue = new Queue<Node>();
        root.type = GridManager.NodeType.Explored;
        queue.Enqueue(root);
        Node traceback = null;

        while (queue.Count > 0)
        {
            for (int i = 0; i < gUI.simSpeed; i++)
            {
                Node v = queue.Dequeue();

                if (v == goal)
                {
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

                            yield return null;
                        }
                    }
                }
            }
        }

        while (traceback != root)
        {
            traceback = traceback.parent;
            g.UpdateNode(traceback.x, traceback.y, GridManager.NodeType.Path);
            yield return null;
        }
    }

    public void StartSearch()
    {
        Debug.Log(g.startCoord);
        Debug.Log(g.endCoord);
        StartCoroutine(BFSVisualizer(g.currentGrid, g.currentGrid[g.startCoord.x, g.startCoord.y], g.currentGrid[g.endCoord.x, g.endCoord.y]));
    }
}
