using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********************************************************************//

//FYI, this class is just for explanation and has no functional purpose

//*********************************************************************//
public class dfsExplanation : GridManager
{
    Node DFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        Stack<Node> stack = new Stack<Node>();
        root.type = NodeType.Explored;
        stack.Push(root);

        while (stack.Count > 0)
        {
            Node v = stack.Pop();

            foreach (Node neighbor in GetNeighbors(v))
            {
                if (neighbor.type != NodeType.Obstacle)
                {
                    if (neighbor.type != NodeType.Explored)
                    {
                        neighbor.type = NodeType.Explored;
                        neighbor.parent = v;
                        stack.Push(neighbor);

                        if (neighbor == goal)
                        {
                            return neighbor;
                        }
                    }
                }
            }
        }
        return null;
    }
}
