using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AStarSearch : MonoBehaviour
{
    public GridManager g;
    public UILogicHandler gUI;

    private int runThisFrameCounter;
    public IEnumerator AStar(Node[,] grid, Node root, Node goal, bool is8Directional)
    {
        bool foundPath = false;
        PriorityQueue pq = new PriorityQueue();
        root.type = GridManager.NodeType.Explored;
        root.gCost = 0f;
        pq.Enqueue(root);
        Node traceback = null;

        while (pq.heap.Count > 0)
        {
            Node v = pq.Dequeue();

            foreach (Node neighbor in is8Directional ? g.GetNeighbors(v) : g.Get4Neighbors(v))
            {
                if (neighbor.type != GridManager.NodeType.Obstacle)
                {
                    if (neighbor.type != GridManager.NodeType.Explored)
                    {
                        //updates the colors of the nodes around the square
                        //this serves no actual pathfinding function, but makes the visualization a bit easier to understand
                        foreach (Node neighbor2 in is8Directional ? g.GetNeighbors(neighbor) : g.Get4Neighbors(neighbor))
                        {
                            if (neighbor2.type == GridManager.NodeType.Unexplored) { g.squareRenderers[neighbor2.x, neighbor2.y].color = g.neigborColor; }
                        }

                        if (neighbor == goal)
                        {
                            foundPath = true;
                            traceback = v;
                            pq.heap.Clear();
                            break;
                        }

                        neighbor.hCost = Distance(goal, neighbor);

                        float tentativeGCost = v.gCost + Distance(v, neighbor);
                        if (tentativeGCost < neighbor.gCost)
                        {
                            neighbor.parent = v;
                            neighbor.gCost = tentativeGCost;
                            neighbor.fCost = tentativeGCost + neighbor.hCost;

                            if (!pq.heap.Contains(neighbor))
                            {
                                pq.Enqueue(neighbor);
                                Debug.Log(neighbor.fCost);
                            }
                            g.UpdateNode(neighbor.x, neighbor.y, GridManager.NodeType.Explored);
                        }

                        //delays the yield return null until simSpeed nodes were explored (lets us control the speed of the simulation)
                        if (runThisFrameCounter > gUI.simSpeed - 1)
                        {
                            runThisFrameCounter = 0;
                            yield return null;
                        }
                    }
                }
                runThisFrameCounter++;
            }
        }
        if (foundPath)
        {
            while (traceback != root)
            {
                if (traceback != root)
                {
                    g.UpdateNode(traceback.x, traceback.y, GridManager.NodeType.Path);
                }
                traceback = traceback.parent;

                if (runThisFrameCounter > (gUI.simSpeed - 1) / 8f)
                {
                    runThisFrameCounter = 0;
                    yield return null;
                }
                runThisFrameCounter++;
            }
        }
    }

    public float Distance(Node from, Node to)
    {
        return Mathf.Sqrt((from.x -  to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y));
    }
}
