using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
    public GridManager g;

    private void Start()
    {
        StartCoroutine(GreedyBestFirstSearch(new Node[10,10], new Node(GridManager.NodeType.Unexplored, 10, 10), new Node(GridManager.NodeType.Unexplored, 50, 50)));
    }
    public IEnumerator GreedyBestFirstSearch(Node[,] grid, Node root, Node goal)
    {
        PriorityQueue pq = new PriorityQueue();
        pq.Insert(new Node(GridManager.NodeType.Unexplored, 10, 10, 0.5f));
        pq.Insert(new Node(GridManager.NodeType.Unexplored, 10, 10, 100f));
        pq.Insert(new Node(GridManager.NodeType.Unexplored, 10, 10, 50f));
        pq.Insert(new Node(GridManager.NodeType.Unexplored, 10, 10, 0f));

        Debug.Log(pq.heap[0].fCost);
        Debug.Log(pq.heap[1].fCost);
        Debug.Log(pq.heap[2].fCost);
        Debug.Log(pq.heap[3].fCost);

        yield return null;
    }
}