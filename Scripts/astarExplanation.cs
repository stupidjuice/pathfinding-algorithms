using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********************************************************************//

//FYI, this class is just for explanation and has no functional purpose

//*********************************************************************//
public class astarExplanation : GridManager
{
    public Node AStar(Node[,] grid, Node root, Node goal)
    {
        PriorityQueue pq = new PriorityQueue();
        root.type = NodeType.Explored;
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

                        neighbor.hCost = Distance(goal, neighbor);

                        float tentativeGCost = v.gCost + Distance(v, neighbor);
                        if (tentativeGCost < neighbor.gCost)
                        {
                            neighbor.parent = v;
                            neighbor.gCost = tentativeGCost;
                            neighbor.fCost = tentativeGCost + neighbor.hCost;

                            neighbor.type = NodeType.Explored;
                            pq.Enqueue(neighbor);
                        }
                    }
                }
            }
        }
        return null;
    }

    public float Distance(Node from, Node to)
    {
        return Mathf.Sqrt((from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y));
    }
}
