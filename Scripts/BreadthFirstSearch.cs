using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : MonoBehaviour
{
    public GridManager g;
    Node BFS(Node[,] grid, Node root, Node goal)
    {
        Queue<Node> queue = new Queue<Node>();
        root.type = GridManager.NodeType.Explored;
        queue.Enqueue(root);

        while(queue.Count > 0)
        {
            Node v = queue.Dequeue();

            if(v == goal)
            {
                return v;
            }
            foreach(Node neighbor in g.Get4Neighbors(v))
            {
                if(neighbor.type != GridManager.NodeType.Explored)
                {
                    neighbor.type = GridManager.NodeType.Explored;
                    neighbor.parent = v;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return null;
    }

    IEnumerator BFSVisualizer(Node[,] grid, Node root, Node goal)
    {
        Queue<Node> queue = new Queue<Node>();
        root.type = GridManager.NodeType.Explored;
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node v = queue.Dequeue();

            if (v == goal)
            {
                break;
            }
            foreach (Node neighbor in g.Get4Neighbors(v))
            {
                if (neighbor.type != GridManager.NodeType.Explored)
                {
                    neighbor.type = GridManager.NodeType.Explored;
                    neighbor.parent = v;
                    queue.Enqueue(neighbor);

                    g.UpdateNode(neighbor.x, neighbor.y, GridManager.NodeType.Explored);
                    yield return new WaitForSeconds(0.001f);
                }
            }
        }
    }

    public void StartSearch()
    {
        g.UpdateNode(10, 10, GridManager.NodeType.Root);
        g.UpdateNode(40, 40, GridManager.NodeType.Goal);
        StartCoroutine(BFSVisualizer(g.currentGrid, g.currentGrid[10, 10], g.currentGrid[40, 40]));
    }
}
