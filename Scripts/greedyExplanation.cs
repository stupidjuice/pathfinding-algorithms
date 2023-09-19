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
        goal.type = NodeType.Explored;
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
                        neighbor.hCost = Vector2.Distance(new Vector2(neighbor.x, neighbor.y), goalCoordinate);
                        neighbor.type = NodeType.Explored;
                        neighbor.parent = v;
                        pq.Enqueue(neighbor);

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
