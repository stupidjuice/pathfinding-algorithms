using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AStarSearch : MonoBehaviour
{
    public GridManager g;
    public UILogicHandler gUI;
    public Stats stats;

    private int runThisFrameCounter;
    public IEnumerator AStar(Node[,] grid, Node root, Node goal, bool is8Directional)
    {
        stats.StartSearch("A*");
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
                        stats.explored++;
                        //updates the colors of the nodes around the square
                        //this serves no actual pathfinding function, but makes the visualization a bit easier to understand
                        foreach (Node neighbor2 in is8Directional ? g.GetNeighbors(neighbor) : g.Get4Neighbors(neighbor))
                        {
                            if (neighbor2.type == GridManager.NodeType.Unexplored) { g.squareRenderers[neighbor2.x, neighbor2.y].color = g.neigborColor; }
                        }

                        neighbor.hCost = Distance(goal, neighbor);
                        if (neighbor.hCost < stats.closest) { stats.closest = neighbor.hCost; }

                        float tentativeGCost = v.gCost + Distance(v, neighbor);
                        if (tentativeGCost < neighbor.gCost)
                        {
                            neighbor.parent = v;
                            neighbor.gCost = tentativeGCost;
                            neighbor.fCost = tentativeGCost + neighbor.hCost;

                            if (neighbor.type != GridManager.NodeType.Explored)
                            {
                                pq.Enqueue(neighbor);
                            }
                            if(neighbor != goal) { g.UpdateNode(neighbor.x, neighbor.y, GridManager.NodeType.Explored); }
                        }

                        if (neighbor == goal)
                        {
                            foundPath = true;
                            traceback = neighbor;
                            pq.heap.Clear();
                            break;
                        }

                        //delays the yield return null until simSpeed nodes were explored (lets us control the speed of the simulation)
                        if (runThisFrameCounter > gUI.simSpeed - 1)
                        {
                            runThisFrameCounter = 0;
                            yield return null;
                        }
                        runThisFrameCounter++;
                    }
                }
            }
        }
        if (foundPath)
        {
            stats.Stop();
            while (traceback != root)
            {
                stats.shortestPath += Distance(traceback, traceback.parent);
                if (traceback != root && traceback != goal)
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

        gUI.PathfindEnded();
    }

    public float Distance(Node from, Node to)
    {
        return Mathf.Sqrt((from.x -  to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y));
    }
}
