using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateShake : PlantBaseState
{
    private ToolType toolNeeded;

    public override void EnterState(PlantStateManager plant)
    {
        // plant._text.text = "Shaking";
        if (plant.angerLevel > 0)
        {
            plant.angerLevel--;
        }
        plant._animator.SetBool("Shake", true);
    }
    
    public override void Initialize(PlantStateManager plant)
    {
        ToolType[] toolTypes = (ToolType[]) Enum.GetValues(typeof(ToolType));
        toolNeeded = toolTypes[Random.Range(1, toolTypes.Length)];
        plant.shakeNeedText.text = "Shaking:" + toolNeeded;
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
        if (selectedTool == ToolType.None) {return;}
        if (selectedTool == toolNeeded)
        {
            plant.currentState = plant.idleState;
            plant.currentState.EnterState(plant);
            plant._animator.SetBool("Shake", false);
            // angerLevel--; todo hmmm anger stays somewhat in each state? No but then it has to be universal shit
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
