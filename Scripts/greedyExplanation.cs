using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********************************************************************//

//FYI, this class is just for explanation and has no functional purpose

//*********************************************************************//
public class greedyExplanation : GridManager
{
    public Node GreedyBestFirstSearch(Node[,] grid, Node root, Node goal)
    {
        Vector2 goalCoordinate = new Vector2(goal.x, goal.y);
        PriorityQueue pq = new PriorityQueue();
        root.type = NodeType.Explored;
        root.gCost = 0f;
        pq.Enqueue(root);

        if (root == goal)
        {
            return goal;
        }

        while (pq.Count > 0)
        {
            Node v = pq.Dequeue();

            foreach (Node neighbor in GetNeighbors(v))
            {
                if (neighbor.type != NodeType.Obstacle)
                {
                    if (neighbor.type != NodeType.Explored)
                    {
                        if (neighbor == goal)
                        {
                            return neighbor;
                        }

                        neighbor.hCost = Distance(neighbor, goal);
                        neighbor.type = NodeType.Explored;
                        neighbor.parent = v;
                        pq.Enqueue(neighbor);
                    }
                }
            }
        }
        return null;
    }

    void nyaaaaa()
    {
        GreedyBestFirstSearch(new Node[5, 5], new Node(NodeType.Unexplored, 0, 0), new Node(NodeType.Unexplored, 0, 0));
    }
    public float Distance(Node from, Node to)
    {
        return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y));
    }
}
