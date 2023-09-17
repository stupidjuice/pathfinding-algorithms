using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********************************************************************//

//FYI, this class is just for explanation and has no functional purpose

//*********************************************************************//
public class bfsExplanation : GridManager
{
    Node BFS(Node[,] grid, Node root, Node goal)
    {
        Queue<Node> queue = new Queue<Node>();
        root.type = NodeType.Explored;
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node v = queue.Dequeue();

            if (v == goal)
            {
                return v;
            }
            foreach (Node neighbor in GetNeighbors(v))
            {
                if (neighbor.type != NodeType.Obstacle)
                {
                    if (neighbor.type != NodeType.Explored)
                    {
                        neighbor.type = NodeType.Explored;
                        neighbor.parent = v;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        return null;
    }



    void UwU()
    {
        BFS(new Node[5,5], new Node(NodeType.Unexplored, 0, 0), new Node(NodeType.Unexplored, 0, 0));
    }
}
