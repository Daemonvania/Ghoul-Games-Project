using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateIdle : PlantBaseState
{
    public override void EnterState(PlantStateManager plant)
    {
        plant._text.text = "Idle";
        // plant._text2.text= "Nothing";
        // plant.GetComponent<Animator>().SetTrigger("Idle");
    }
    
    public override void Initialize(PlantStateManager plant)
    {
    }
    
    public override void Progress(PlantStateManager plant)
    {
        //todo random based on existing states (array seems like the move)
        plant.currentState = Random.value < 0.5f ? plant.sheddingState : plant.shakingState;
        plant.currentState.EnterState(plant);
    }
    
    public override void ToolUsed(PlantStateManager plant, ToolType selectedTool)
    {
        Debug.Log("Tool used in idle state");
    }
    
    public override void Update(PlantStateManager plant)
    {
        
    }
}