using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateManager : Interactable
{
    public PlantBaseState currentState;

    
    //todo make this better, can be managed by diff script or jus diff way    
    [SerializeField] public TMP_Text _text;
    
    [SerializeField] public TMP_Text shedNeedText;
    [SerializeField] public TMP_Text shakeNeedText;
    
    //Could make it an array..?
    public PlantStateIdle idleState = new PlantStateIdle();
    public PlantStateShed sheddingState = new PlantStateShed();
    public PlantStateShake shakingState = new PlantStateShake();
    
    private void Start()
    {
        base.Start();
        currentState = idleState;
        currentState.EnterState(this);
        
        idleState.Initialize(this);
        sheddingState.Initialize(this);
        shakingState.Initialize(this);
        
        //todo set a lot of this up for the editor 
        InvokeRepeating(nameof(ProgressCheck), Random.Range(4, 6), Random.Range(13,16));
    }

    public void ProgressCheck()
    {
        Debug.Log("ProgressCheck");
        //change this dynamically as well
        if (Random.Range(0, 20) > 12)
        {
            currentState.Progress(this);
        }
    }
    
    public override void Interact(Player player)
    {
        currentState.ToolUsed(this, player.selectedTool);
    }

    public override void OnLook()
    {
        
    }
}
