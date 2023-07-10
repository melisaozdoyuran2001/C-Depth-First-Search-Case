using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random; 
using UnityEngine.UI;
using TMPro; 


public class InputController : MonoBehaviour
{
    public TMP_InputField gridSizeInput;
    public TMP_InputField percentageInput;
    public GridCreator gridCreator;

    public void SetGridSize(string input)
    {
        if (int.TryParse(input, out int size))
        {
            gridCreator.gridSize = size;
        }
    }

    public void SetFillPercentage(string input)
    {
        if (int.TryParse(input, out int percent))
        {
            gridCreator.FillPercentage = Mathf.Clamp(percent, 0, 100);
            percentageInput.gameObject.SetActive(false);
            gridSizeInput.gameObject.SetActive(false);
            gridCreator.CreateGrid();
            gridCreator.FillGrid(); 
        }
    }
}
