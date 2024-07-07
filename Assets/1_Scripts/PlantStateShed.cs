using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateShed : PlantBaseState
{
    public ToolType toolNeeded;
    private int angerLevel = 0;

    public override void EnterState(PlantStateManager plant)
    {
        plant._text.text = "Shed";
        plant._text2.text = "Need" + toolNeeded;
        // plant.GetComponent<Animator>().SetTrigger("Shed");
    }

    public override void Initialize()
    {
        ToolType[] toolTypes = (ToolType[]) Enum.GetValues(typeof(ToolType));
        toolNeeded = toolTypes[Random.Range(0, toolTypes.Length)];
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
            Debug.Log("ANGY");
        }
    }
}
