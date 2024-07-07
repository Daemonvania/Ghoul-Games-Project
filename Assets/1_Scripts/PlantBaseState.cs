using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantBaseState
{
    // public abstract ToolType toolNeeded { get; }
    
    public abstract void EnterState(PlantStateManager plant);
    public abstract void Progress(PlantStateManager plant);
    public abstract void ToolUsed(PlantStateManager plant, ToolType selectedTool);

    public abstract void Initialize();
}
