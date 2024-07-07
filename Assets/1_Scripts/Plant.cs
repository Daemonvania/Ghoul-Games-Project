using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : Interactable
{
    public override void Interact(Player player)
    {
        print("interacted with plant");
         UseTool(player.selectedTool);
    }

    public abstract void UseTool(ToolType toolType);
}
