using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
    public GridManager g;
    public UILogicHandler gUI;
    private int runThisFrameCounter = 0;

    public Stats stats;

    public IEnumerator GreedyBestFirstSearch(Node[,] grid, Node root, Node goal, bool is8Directional, GridManager.DistanceMetric metric)
    {
        stats.StartSearch("Greedy");
        bool foundPath = false;
        PriorityQueue pq = new PriorityQueue();
        root.type = GridManager.NodeType.Explored;
        pq.Enqueue(root);
        //use this to trace path back to start after the goal is found
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

                        neighbor.fCost = g.Distance(neighbor, goal, metric);
                        if (neighbor.fCost < stats.closest) { stats.closest = neighbor.fCost; }

                        neighbor.parent = v;

                        if (neighbor == goal)
                        {
                            foundPath = true;
                            traceback = neighbor;
                            pq.heap.Clear();
                            break;
                        }

                        pq.Enqueue(neighbor);

                        //prevents the start node from changing color so it will always be shown and never turn red
                        if (neighbor != goal || neighbor.type != GridManager.NodeType.Root)
                        {
                            g.UpdateNode(neighbor.x, neighbor.y, GridManager.NodeType.Explored);
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
            Debug.Log("found path yay");
            stats.Stop();
            while (traceback != root)
            {
                stats.shortestPath += g.Distance(traceback, traceback.parent, GridManager.DistanceMetric.Absolute);
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
}