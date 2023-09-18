using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copied from https://www.geeksforgeeks.org/introduction-to-min-heap-data-structure/
public class PriorityQueue
{
    public List<Node> heap = new List<Node>();

    public void Insert(Node node)
    {
        heap.Add(node);
        int index = heap.Count - 1;

        while (index > 0 && heap[(index - 1) / 2].fCost > heap[index].fCost)
        {
            Node temp = heap[index];
            heap[index] = heap[(index - 1) / 2];
            heap[(index - 1) / 2] = temp;

            index = (index - 1) / 2;
        }
    }
}