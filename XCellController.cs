using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XCellController : MonoBehaviour
{
    public GameObject overlay;
    private GameObject overlayCopy;
    private bool _isO = false;
    ObjectPoolManager objectPoolManager = ObjectPoolManager.Instance;

    private void Awake()
    {
        objectPoolManager = FindObjectOfType<ObjectPoolManager>();
    }
    void OnMouseOver()
    {
        if ( !_isO )
        {
            overlayCopy = objectPoolManager.Get(ObjectPoolManager.ObjectPoolType.Overlay); 
            overlayCopy.transform.localScale = transform.localScale;
            overlayCopy.transform.position = transform.position;
            _isO = true;
        }
    }
    void OnMouseExit()
    {
        if(_isO )
        {
            objectPoolManager.ReturnToPool(overlayCopy);
            _isO = false; 
        }
    }
}
