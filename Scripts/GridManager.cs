using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public Color unexploredColor, exploredColor, obstacleColor, pathColor;
    public Dictionary<NodeType, Color> gridColors;
    public enum NodeType
    {
        Unexplored,
        Explored,
        Obstacle,
        Path,
    }

    public Node[,] currentGrid;
    public GameObject squarePrefab;
    public SpriteRenderer[,] squareRenderers;
    public Camera cam;
    public float extraOrthoScale = 1.0f;

    public void GenerateGrid()
    {
        //get offsets so the grid is centered at (0, 0)
        float offsetX, offsetY;
        if (gridWidth % 2 == 0) { offsetX = gridWidth / 2 - 0.5f; }
        else { offsetX = gridWidth / 2; }

        if (gridHeight % 2 == 0) { offsetY = gridHeight / 2 - 0.5f; }
        else { offsetY = gridHeight / 2; }

        currentGrid = new Node[gridWidth, gridHeight];
        squareRenderers = new SpriteRenderer[gridWidth, gridHeight];
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                currentGrid[i, j] = new Node(NodeType.Unexplored, i, j);

                GameObject newSquare = Instantiate(squarePrefab, new Vector3(i - offsetX, j - offsetY, 0f), Quaternion.identity);
                squareRenderers[i, j] = newSquare.GetComponent<SpriteRenderer>();
                squareRenderers[i, j].color = gridColors[NodeType.Unexplored];
            }
        }

        //set orthographic scale so that the entire map fits within the camera view
        if(gridHeight * cam.pixelWidth > gridWidth * cam.pixelHeight) { cam.orthographicSize = gridHeight / 2 + extraOrthoScale * gridHeight; }
        else { cam.orthographicSize = gridWidth * (9f / 16f) / 2 + extraOrthoScale * gridWidth; }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;
                if(node.x + i < gridWidth && node.x + i >= 0)
                {
                    if (node.y + j < gridHeight && node.y + j >= 0)
                    {
                        neighbors.Add(currentGrid[i + node.x, j + node.y]);
                    }
                }
            }
        }
        return neighbors;
    }
    
    public void UpdateNode(int x, int y, NodeType type)
    {
        currentGrid[x, y].type = type;
        squareRenderers[x, y].color = gridColors[type];
    }

    private void Start()
    {
        //THIS MUST BE DONE FIRST BECAUSE OTHER FUNCTIONS RELY ON THIS
        //I DONT KNOW HOW TO DO THIS DURING FIELD DECLARATION PLEASE HELP ME
        //allows nodetypes to be used as lookups lol
        gridColors = new Dictionary<NodeType, Color>()
        {
            { NodeType.Unexplored, unexploredColor },
            { NodeType.Explored, exploredColor },
            { NodeType.Obstacle, obstacleColor },
            { NodeType.Path, pathColor }
        };

        GenerateGrid();

        List<Node> neighbors = GetNeighbors(currentGrid[10, 10]);
        /*
        foreach(Node node in neighbors)
        {
            Debug.Log(node.x.ToString() + " " + node.y.ToString());
        }
        */
    }
}

public class Node
{
    public GridManager.NodeType type;
    public int x, y;
    public Node parent;

    public Node(GridManager.NodeType type, int x, int y)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }
}
