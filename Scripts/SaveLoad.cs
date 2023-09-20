using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    public GridManager g;
    public DrawMaze dm;
    public UILogicHandler ui;
    public TMP_Text saveIsPresentText;
    public string isSave, noSave;

    void Start()
    {
        string path = Application.persistentDataPath + "/save.grid";
        if(File.Exists(path))
        {
            if(File.ReadAllLines(path)[0] == "empty")
            {
                saveIsPresentText.text = noSave;
            }
            else
            {
                saveIsPresentText.text = isSave;
            }
        }
        else
        {
            saveIsPresentText.text = noSave;
        }
    }

    public void Save(string saveName, Node[,] grid)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".grid";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        bf.Serialize(fs, grid);
        fs.Close();
    }
    public void Load(string saveName)
    {
        ui.deleteButton.interactable = true;
        g.DeleteGrid();
        string path = Application.persistentDataPath + "/" + saveName + ".grid";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Open);
        Node[,] grid = bf.Deserialize(fs) as Node[,];
        fs.Close();

        g.gridWidth = grid.GetLength(0);
        g.gridHeight = grid.GetLength(1);
        g.GenerateGrid();

        for (int i = 0; i < g.gridWidth; i++)
        {
            for (int j = 0; j < g.gridHeight; j++)
            {
                g.UpdateNode(i, j, grid[i, j].type);

                if (grid[i, j].type == GridManager.NodeType.Root) 
                {
                    dm.currentStart = new Vector2Int(i, j);
                    g.startCoord = dm.currentStart;
                }
                if (grid[i, j].type == GridManager.NodeType.Goal)
                {
                    dm.currentGoal = new Vector2Int(i, j);
                    g.endCoord = dm.currentGoal;
                }
            }
        }

        ui.mazeDrawMode.interactable = true;
        ui.setStartButton.interactable = true;
        ui.setGoalButton.interactable = true;
        dm.generate.interactable = false;
    }

    public void SaveGrid()
    {
        Save("save", g.currentGrid);
    }
    public void LoadGrid()
    {
        Load("save");
    }
}