using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStateAngry : PlantBaseState
{
    private Player _player;
    public override void EnterState(PlantStateManager plant)
    {
        if (plant.jumpScare)
        {
            JumpScare(plant);
        }
        // plant._text.text = "Angry";
        // plant.GetComponent<Animator>().SetTrigger("Angry");
    }
    
    public override void Initialize(PlantStateManager plant)
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    
    public override void Progress(PlantStateManager plant)
    {

    }
    
    public override void ToolUsed(PlantStateManager plant, ToolType selectedTool)
    {
       
    }
    
    public override void Update(PlantStateManager plant)
    {
        if (_player.currentTransform == _player.baseLocation)
        {
            JumpScare(plant);
        }
    }

    void JumpScare(PlantStateManager plant)
    {
        plant.jumpScareCanvas.SetActive(true);
        _player.enabled = false;
    }
}
