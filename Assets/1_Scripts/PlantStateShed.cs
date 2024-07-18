using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateShed : PlantBaseState
{
    public ToolType toolNeeded;

    public override void EnterState(PlantStateManager plant)
    {
        // plant._text.text = "Shedding";
        plant.angerLevel--;
        plant.shedParticle.Play();
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
      plant.angerLevel++;
      if (plant.angerLevel >= 3)
      {
          EnterAngryState(plant);
      }
    }
    
    public override void ToolUsed(PlantStateManager plant, ToolType selectedTool)
    {
        Debug.Log(selectedTool);
        if (selectedTool == ToolType.None) {return;}
        if (selectedTool == toolNeeded)
        {
            plant.shedParticle.Stop();
            
            plant.currentState = plant.idleState;
            plant.currentState.EnterState(plant);
        }
        else
        {
            plant.angerLevel++;
            if (plant.angerLevel >= 3)
            {
                EnterAngryState(plant);
            }
        }
    }

    void EnterAngryState(PlantStateManager plant)
    {
        plant.jumpScare = true;
        plant.currentState = plant.angryState;
        plant.currentState.EnterState(plant);
    }
    
    public override void Update(PlantStateManager plant)
    {
        
    }
}
