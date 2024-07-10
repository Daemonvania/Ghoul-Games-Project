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
        plant._text.text = "Shedding";
        angerLevel = 0;
        
        // plant.GetComponent<Animator>().SetTrigger("Shed");
    }

    public override void Initialize(PlantStateManager plant)
    {
        
        ToolType[] toolTypes = (ToolType[]) Enum.GetValues(typeof(ToolType));
        toolNeeded = toolTypes[Random.Range(1, toolTypes.Length)];
        plant.shedNeedText.text = "Shedding:" + toolNeeded;
    }
    
    public override void Progress(PlantStateManager plant)
    {
      angerLevel++;
      if (angerLevel > 3)
      {
          plant.currentState = plant.angryState;
          plant.currentState.EnterState(plant);
      }
    }
    
    public override void ToolUsed(PlantStateManager plant, ToolType selectedTool)
    {
        if (selectedTool == ToolType.None) {return;}
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
                plant.jumpScare = true;
                plant.currentState = plant.angryState;
                plant.currentState.EnterState(plant);
            }
        }
    }
    
    public override void Update(PlantStateManager plant)
    {
        
    }
}
