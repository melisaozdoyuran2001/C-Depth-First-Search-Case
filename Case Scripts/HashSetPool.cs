using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

public class HashSetPool : MonoBehaviour
{
    private Queue<HashSet<XCell>> pool;

    public Queue<HashSet<XCell>> Pool
    {
        get
        {
            if (pool == null)
            {
                pool = new Queue<HashSet<XCell>>();
            }
            return pool;
        }
    }

    public HashSet<XCell> Get()
    {
        if (Pool.Count > 0)
        {
            return Pool.Dequeue();
        }

        return new HashSet<XCell>();
    }

    public void clearSet(HashSet<XCell> set)
    {
        set.Clear();
        Pool.Enqueue(set);
    }
}

