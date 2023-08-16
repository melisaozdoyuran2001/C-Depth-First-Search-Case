using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random; 

public class GridController : MonoBehaviour
{
    public GridCreator gridCreator; 

    void Start()
    {
        if(gridCreator == null)
        {
            gridCreator = FindObjectOfType<GridCreator>(); 
        }
    }

    public List<XCell> GetNeighbors(XCell cell)
    {
        int size = gridCreator.gridSize;
        List<XCell> neighbors = new List<XCell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0) && (x == 0 || y == 0))
                {
                    int newX = cell.i + x;
                    int newY = cell.j + y;
                    
                    if (newX >= 0 && newY >= 0 && newX < size && newY < size)
                    {
                        int index = newX * size + newY;
                        neighbors.Add(gridCreator.grid[index]);
                        
                    }
                }
            }
        }
        return neighbors; 
    }
    
    public int GetXNum(int size, float percentage)
    {
        int gridNum = size * size;
        return  Mathf.RoundToInt(gridNum * percentage / 100);
    }

}

