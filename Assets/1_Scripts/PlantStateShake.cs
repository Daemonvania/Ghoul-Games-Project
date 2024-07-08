using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateShake : PlantBaseState
{
    private ToolType toolNeeded;
    private int angerLevel = 0;
    public override void EnterState(PlantStateManager plant)
    {
        plant._text.text = "Shaking";

        // plant.GetComponent<Animator>().SetTrigger("Shake");
    }
    
    public override void Initialize(PlantStateManager plant)
    {
        ToolType[] toolTypes = (ToolType[]) Enum.GetValues(typeof(ToolType));
        toolNeeded = toolTypes[Random.Range(0, toolTypes.Length)];
        plant.shakeNeedText.text = "Shaking:" + toolNeeded;
    }
    
    public override void Progress(PlantStateManager plant)
    {
        angerLevel++;
        if (angerLevel > 3)
        {
            Debug.Log("ANGY");
        }
    }
    
    public override void ToolUsed(PlantStateManager plant, ToolType selectedTool)
    {
        if (selectedTool == toolNeeded)
        {
            plant.currentState = plant.idleState;
            plant.currentState.EnterState(plant);
        }
        else
        {
            angerLevel++;
            if (angerLevel > 3)
            {
                Debug.Log("ANGY");
            }
        }
    }
}
