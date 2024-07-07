using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlantExample : Plant
{
    public override void UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Water:
                Debug.Log("Watering plant");
                break;
            case ToolType.Fertilizer:
                Debug.Log("Fertilizing plant");
                break;
            case ToolType.Pruner:
                Debug.Log("Pruning plant");
                break;
        }
    }
    
    public override void OnLook()
    {
    }
}