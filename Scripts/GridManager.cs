using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public Color unexploredColor, exploredColor, obstacleColor, pathColor, rootColor, goalColor, neigborColor, lastExplored;
    public Dictionary<NodeType, Color> gridColors;
    public float offsetX, offsetY;
    public Vector2Int startCoord, endCoord;
    public enum NodeType
    {
        Unexplored,
        Explored,
        Obstacle,
        Path,
        Root,
        Goal
    }

    public struct NodeInfo
    {
        public float hCost;
        public float gCost;
        public float fCost;
    }

    public Node[,] currentGrid;
    public SpriteRenderer[,] squareRenderers;
    public GameObject squarePrefab;
    public GameObject linePrefab;
    public Transform parent;
    public float lineWidth;
    public Camera cam;
    public float extraOrthoScale = 1.0f;

    public List<Node> lastExploredNodes;
    public bool updateToRed;

    public void GenerateGrid()
    {
        //get offsets so the grid is centered at (0, 0)
        if(gridWidth % 2 == 0) { offsetX = gridWidth / 2 - 0.5f; }
        else { offsetX = gridWidth / 2; }

        if(gridHeight % 2 == 0) { offsetY = gridHeight / 2 - 0.5f; }
        else { offsetY = gridHeight / 2; }
            
        currentGrid = new Node[gridWidth, gridHeight];
        squareRenderers = new SpriteRenderer[gridWidth, gridHeight];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                currentGrid[i, j] = new Node(NodeType.Unexplored, i, j);

                GameObject newSquare = Instantiate(squarePrefab, new Vector3(i - offsetX, j - offsetY, 0f), Quaternion.identity, parent);
                squareRenderers[i, j] = newSquare.GetComponent<SpriteRenderer>();
                squareRenderers[i, j].color = gridColors[NodeType.Unexplored];
            }
        }

        //lines
        for(int i = 0; i < gridWidth + 1; i++)
        {
            Transform currentLine = Instantiate(linePrefab, new Vector3(i - offsetX - 0.5f, 0f, -2f), Quaternion.identity, parent).transform;
            currentLine.localScale = new Vector3(lineWidth, gridHeight, 1f);
        }
        for (int i = 0; i < gridHeight + 1; i++)
        {
            Transform currentLine = Instantiate(linePrefab, new Vector3(0f, i - offsetY - 0.5f, -2f), Quaternion.identity, parent).transform;
            currentLine.localScale = new Vector3(gridWidth, lineWidth, 1f);
        }

        //set orthographic scale so that the entire map fits within the camera view
        if (gridHeight * cam.pixelWidth > gridWidth * cam.pixelHeight) { cam.orthographicSize = gridHeight / 2 + extraOrthoScale * gridHeight; }
        else { cam.orthographicSize = gridWidth * (cam.pixelHeight / cam.pixelWidth) / 2 + extraOrthoScale * gridWidth; }
    }

    public void DeleteGrid()
    {
        lastExploredNodes.Clear();
        squareRenderers = null;
        currentGrid = null;
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(i == 0 && j == 0) continue;
                if(node.x + i < gridWidth && node.x + i >= 0)
                {
                    if(node.y + j < gridHeight && node.y + j >= 0)
                    {
                        neighbors.Add(currentGrid[i + node.x, j + node.y]);
                    }
                }
            }
        }
        return neighbors;
    }
    public List<Node> Get4Neighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        //uwu
        if(node.x + 1 < gridWidth)  { neighbors.Add(currentGrid[node.x + 1, node.y]); }
        if(node.x - 1 >= 0)         { neighbors.Add(currentGrid[node.x -1, node.y]); }
        if(node.y + 1 < gridHeight) { neighbors.Add(currentGrid[node.x, node.y + 1]); }
        if (node.y - 1 >= 0)        { neighbors.Add(currentGrid[node.x, node.y - 1]); }

        return neighbors;
    }

    public void UpdateNode(int x, int y, NodeType type)
    {
        if(updateToRed)
        {
            foreach (Node node in lastExploredNodes)
            {
                squareRenderers[node.x, node.y].color = exploredColor;
            }
            lastExploredNodes.Clear();
            updateToRed = false;
        }
        currentGrid[x, y].type = type;
        if (type == NodeType.Explored)
        {
            squareRenderers[x, y].color = lastExplored;
            lastExploredNodes.Add(currentGrid[x, y]);
        }
        else
        {
            squareRenderers[x, y].color = gridColors[type];
        } 
    }

    public void UpdateRed()
    {
        foreach (Node node in lastExploredNodes)
        {
            squareRenderers[node.x, node.y].color = exploredColor;
        }
        lastExploredNodes.Clear();
        updateToRed = false;
    }

    private void LateUpdate()
    {
        updateToRed = true;
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
            { NodeType.Path, pathColor },
            { NodeType.Goal, goalColor },
            { NodeType.Root, rootColor }
        };
    }
}

[System.Serializable]
public class Node
{
    public GridManager.NodeType type;
    public int x, y;
    public Node parent;
    public float hCost = 0f, gCost = Mathf.Infinity, fCost = 0f;

    public Node(GridManager.NodeType type, int x, int y)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }
    public Node(GridManager.NodeType type, int x, int y, float fCost)
    {
        this.type = type;
        this.x = x;
        this.y = y;
        this.fCost = fCost;
    }
}
//coconut