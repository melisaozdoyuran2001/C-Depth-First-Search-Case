using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum ObjectPoolType
    {
        XCell = 0,
        GridSquare = 1,
        Overlay = 2
    };
    public static ObjectPoolManager Instance { get; private set; }

    public List<ObjectPool> Pools;
    [System.Serializable]
    
    public class ObjectPool
    {
        public ObjectPoolType objectPoolType; 
        public GameObject OriginalPrefab;
        public HashSet<GameObject> ActiveObjects = new HashSet<GameObject>();
        public Queue<GameObject> ObjectsInQueue = new Queue<GameObject>();
        public int InitialSize; 
    }
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void InitializePool()
    {
        foreach (ObjectPool pool in Pools)
        {
            for (int i = 0; i < pool.InitialSize; i++)
            {
                GameObject obj = Instantiate(pool.OriginalPrefab);
                obj.SetActive(false); 
                pool.ObjectsInQueue.Enqueue(obj);
            }
        }
    }
    
    
    public GameObject Get(ObjectPoolType type)
    {
        ObjectPool targetPool = Pools.Find(x => x.objectPoolType == type);
    
        if (targetPool.ObjectsInQueue.Count > 0)
        {
            GameObject obj = targetPool.ObjectsInQueue.Dequeue();
            targetPool.ActiveObjects.Add(obj);
            obj.SetActive(true); // Activate
            return obj;
        }
        GameObject obj2 = Instantiate(targetPool.OriginalPrefab);
        targetPool.ActiveObjects.Add(obj2);
        obj2.SetActive(true); // Activate
        return obj2;
    }

    
    public void ReturnToPool(GameObject obj)
    { 
        ObjectPoolType type = obj.GetComponent<PoolObject>().type;
        ObjectPool targetPool = Pools.Find(x => x.objectPoolType == type);
        
        if (targetPool.ActiveObjects.Contains(obj))
        {
            targetPool.ActiveObjects.Remove(obj);
            targetPool.ObjectsInQueue.Enqueue(obj);
            obj.SetActive(false);
            obj.transform.parent = this.transform; 
        }
        else
        {
            Debug.LogError("Duplication error");
        }
    }
    
    
    public void ReturnAllToPoolByType(ObjectPoolType type)
    {
        ObjectPool targetPool = Pools.Find(x => x.objectPoolType == type);
        List<GameObject> tmp = new List<GameObject>(targetPool.ActiveObjects);
            
        int count = targetPool.ActiveObjects.Count; 
        Debug.Log(count);
        for (int i = 0; i < count; i++)
        {
            ReturnToPool(tmp[i]);
        }
    }
    
    

    public void ResetAll()
    {
        foreach (ObjectPool pool in Pools)
        {
            ReturnAllToPoolByType(pool.objectPoolType);
        }
    }
}
