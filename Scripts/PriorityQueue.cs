using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//copied from https://www.geeksforgeeks.org/introduction-to-min-heap-data-structure/
public class PriorityQueue
{
    public List<Node> heap = new List<Node>();
    public int Count = 0;

    public void Enqueue(Node node)
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
        Count++;
    }

    public Node Dequeue()
    {
        Node toReturn = heap[0];
        int index = 0;
        if (index == -1)
        {
            return null;
        }
        heap[index] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;
            if (leftChild < heap.Count && heap[leftChild].fCost < heap[smallest].fCost)
            {
                smallest = leftChild;
            }
            if (rightChild < heap.Count && heap[rightChild].fCost < heap[smallest].fCost)
            {
                smallest = rightChild;
            }
            if (smallest != index)
            {
                Swap(index, smallest);
                index = smallest;
            }
            else
            {
                break;
            }
        }

        Count--;
        return toReturn;
    }
    private void Swap(int i, int j)
    {
        Node temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}