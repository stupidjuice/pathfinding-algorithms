using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********************************************************************//

//FYI, this class is just for explanation and has no functional purpose

//*********************************************************************//
public class dfsExplanation : GridManager
{
    Node DepthFirstSearch(Node[,] grid, Node root, Node goal)
    {
        Stack<Node> stack = new Stack<Node>();
        root.type = NodeType.Explored;
        stack.Push(root);

        if (root == goal)
        {
            return goal;
        }

        while (stack.Count > 0)
        {
            Node v = stack.Pop();

            foreach (Node neighbor in GetNeighbors(v))
            {
                if (neighbor.type != NodeType.Obstacle)
                {
                    if (neighbor == goal)
                    {
                        return neighbor;
                    }

                    if (neighbor.type != NodeType.Explored)
                    {
                        neighbor.type = NodeType.Explored;
                        neighbor.parent = v;
                        stack.Push(neighbor);
                    }
                }
            }
        }
        return null;
    }
    
    void OwO()
    {
        DepthFirstSearch(new Node[5, 5], new Node(NodeType.Unexplored, 0, 0), new Node(NodeType.Unexplored, 0, 0));
    }
}
