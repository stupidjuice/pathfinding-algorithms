using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    public GridManager g;
    public UILogicHandler gUI;
    private int runThisFrameCounter = 0;

    public IEnumerator BFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        bool foundPath = false;
        Queue<Node> queue = new Queue<Node>();
        root.type = GridManager.NodeType.Explored;
        queue.Enqueue(root);
        //use this to trace path back to start after the goal is found
        Node traceback = null;

        while (queue.Count > 0)
        {
            Node v = queue.Dequeue();

            foreach (Node neighbor in g.Get4Neighbors(v))
            {
                //terrible doube if statement
                if (neighbor.type != GridManager.NodeType.Obstacle)
                {
                    if (neighbor.type != GridManager.NodeType.Explored)
                    {
                        //updates the colors of the nodes around the square
                        //this serves no actual pathfinding function, but makes the visualization a bit easier to understand
                        foreach (Node neighbor2 in g.Get4Neighbors(neighbor))
                        {
                            if (neighbor2.type == GridManager.NodeType.Unexplored) { g.squareRenderers[neighbor2.x, neighbor2.y].color = g.neigborColor; }
                        }
                        neighbor.parent = v;
                        queue.Enqueue(neighbor);

                        if (neighbor == goal)
                        {
                            //yippe! path was found
                            foundPath = true;
                            traceback = neighbor.parent;
                            queue.Clear();
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
}