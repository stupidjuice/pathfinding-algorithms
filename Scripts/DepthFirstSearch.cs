using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour
{
    public GridManager g;
    public UILogicHandler gUI;

    private int runThisFrameCounter = 0;

    public Stats stats;
    public IEnumerator DFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        stats.StartSearch("A*");
        bool foundPath = false;
        Stack<Node> stack = new Stack<Node>();
        root.type = GridManager.NodeType.Explored;
        stack.Push(root);
        //use this when the path is found to trace back to the start
        Node traceback = null;

        while (stack.Count > 0)
        {
            Node v = stack.Pop();

            foreach (Node neighbor in g.Get4Neighbors(v))
            {
                if (neighbor.type != GridManager.NodeType.Obstacle)
                {
                    if (neighbor.type != GridManager.NodeType.Explored)
                    {
                        stats.explored++;
                        //updates the colors of the nodes around the square
                        //this serves no actual pathfinding function, but makes the visualization a bit easier to understand
                        foreach (Node neighbor2 in g.Get4Neighbors(neighbor))
                        {
                            if (neighbor2.type == GridManager.NodeType.Unexplored) { g.squareRenderers[neighbor2.x, neighbor2.y].color = g.neigborColor; }
                        }

                        neighbor.parent = v;
                        stack.Push(neighbor);

                        float distToEnd = Distance(neighbor, goal);
                        if (distToEnd < stats.closest) { stats.closest = distToEnd; }

                        if (neighbor == goal)
                        {
                            foundPath = true;
                            traceback = neighbor;
                            //quit while loop
                            stack.Clear();
                            break;
                        }

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
                
            if (foundPath)
            {
                stats.Stop();
                while (traceback != root)
                {
                    stats.shortestPath += Distance(traceback, traceback.parent);
                    if(traceback != root && traceback != goal)
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
    }
    public float Distance(Node from, Node to)
    {
        return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y));
    }
}
