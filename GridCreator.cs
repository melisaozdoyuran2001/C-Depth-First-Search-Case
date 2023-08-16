using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random; 
using UnityEngine.UI;
using TMPro; 



public class GridCreator : MonoBehaviour
{
    public int gridSize;
    public int FillPercentage;
    public GameObject gridSquare;
    public float cellWidth;
    public List<XCell> grid;
    public GridController gridController;
    ObjectPoolManager objectPoolManager = ObjectPoolManager.Instance;

    private HashSetPool hashSetPool;

    private void Awake()
    {
        hashSetPool = new HashSetPool();
        objectPoolManager = FindObjectOfType<ObjectPoolManager>();

    }

    public ObjectPoolManager.ObjectPoolType objectPoolType;
    [ContextMenu("tests")]
    public void RemoveAllXCells()
    {
        objectPoolManager.ReturnAllToPoolByType(objectPoolType);
    }
    
    
    public void CreateGrid()   
    {
        Camera cam = Camera.main;
        float cameraHeight = cam.orthographicSize * 2f; 
        float cameraWidth = cameraHeight* cam.aspect; 
        cellWidth = cameraWidth / gridSize;
        grid = new List<XCell>();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j< gridSize;j++)
            {
                GameObject gridCopy = objectPoolManager.Get(ObjectPoolManager.ObjectPoolType.GridSquare);
                gridCopy.transform.parent = this.transform; 
                gridCopy.transform.localScale = new Vector2(cellWidth,cellWidth);
                gridCopy.transform.localPosition = new Vector2(
                    i * (cameraWidth / gridSize) + (cellWidth - cameraWidth) / 2,
                    j * (cameraWidth / gridSize) + (cellWidth - cameraWidth) / 2);

                XCell cell = gridCopy.GetComponent<XCell>();
                cell.i = i;
                cell.j = j;
                grid.Add(cell);
            }
        }
    }

    public void FillGrid()
    {
        List<XCell> validCells =  new List<XCell>(grid);
        XCell xcell = FindObjectOfType<XCell>();
       
        if (FillPercentage > 50)
        {
            Debug.Log("IMPOSSIBLE ACTION, SETTING PERCENTAGE TO MAXIMUM CAPACITY");
            FillPercentage = 50; 
        }
        
        int expectedXNum = gridController.GetXNum(gridSize, FillPercentage);
        int currentXNum = 0; 

        while (currentXNum < expectedXNum)
        {
            if (expectedXNum == 0 || validCells.Count == 0) {break;}

            int index = UnityEngine.Random.Range(0, validCells.Count);
            XCell randomCell = validCells[index];
            
            validCells.RemoveAt(index);

            var visited = hashSetPool.Get(); 
            xcell.DepthFirstSearch(randomCell, visited);
            
            if (visited.Count < 3)
            {
                randomCell._isX = true;
                randomCell.XCopy = objectPoolManager.Get(ObjectPoolManager.ObjectPoolType.XCell);
                randomCell.XCopy.transform.localScale = randomCell.transform.localScale;
                randomCell.XCopy.transform.position = randomCell.transform.position;
                currentXNum++;
            }
            hashSetPool.clearSet(visited);
        }
    }
    
    
    

}
