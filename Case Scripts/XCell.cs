using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class XCell : MonoBehaviour
{
    
    public GameObject X;
    public GameObject XCopy;
    public int i { get; set; }
    public int j { get; set; }
    public bool _isX = false;
    private HashSetPool hashSetPool;
    ObjectPoolManager objectPoolManager = ObjectPoolManager.Instance;

    private void Awake()
    {
        hashSetPool = FindObjectOfType<HashSetPool>();
        objectPoolManager = FindObjectOfType<ObjectPoolManager>();
    }
    
    private void OnMouseDown()
    {
        if (X.activeInHierarchy)  {_isX = true;}
        if (_isX) { return; }
        
        _isX = true;
        
        XCopy =  objectPoolManager.Get(ObjectPoolManager.ObjectPoolType.XCell);
        XCopy.transform.localScale = transform.localScale;
        XCopy.transform.position = transform.position;

        var visited = hashSetPool.Get();
        DepthFirstSearch(this, visited); 
        
        if(visited.Count >= 3)
        { 
            foreach (XCell cell in visited)
            {
                StartCoroutine(DeactivateAfterDelay(0.1f, cell.XCopy));
                cell._isX = false;
            }
        }
        hashSetPool.clearSet(visited);
        
    }
    
    private IEnumerator DeactivateAfterDelay(float duration,  GameObject copy) {
        yield return new WaitForSeconds(duration);
        objectPoolManager.ReturnToPool( copy);
    }
    
    public void DepthFirstSearch(XCell start, HashSet<XCell> visited)
    {
        GridController gridController = FindObjectOfType<GridController>();

        Stack<XCell> stack = new Stack<XCell>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            XCell vertex = stack.Pop();

            if (!visited.Contains(vertex))
            {
                visited.Add(vertex);
                List<XCell> neighbors = gridController.GetNeighbors(vertex);

                foreach (XCell neighbor in neighbors)
                {
                    if (neighbor._isX )
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }
    }
}

        
        
       


            