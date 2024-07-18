using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using Unity.VisualScripting;
using UnityEngine;

public class LightSwitch : Interactable
{
    
    [SerializeField] private GameObject light;
    [SerializeField] private PlantStateManager plant;
    [SerializeField] private bool canInteract = true;
    
    private bool isOn = false;

    private Crank crank;
    
    void Start()
    {
        base.Start();
        crank = GameObject.FindGameObjectWithTag("Crank").GetComponent<Crank>();
        light.SetActive(false);
    }
    
    public override void Interact(Player player)
    {
        isOn = !isOn;
        light.SetActive(isOn);
        plant.LightChange(isOn);
        
        
        crank.ToggleLightSwitch(gameObject, isOn);
    }

    public void TurnOff()
    {
        isOn = false;
        light.SetActive(isOn);
        plant.LightChange(isOn);
    }
    
    public override void OnLook()
    {
        //todo move up a bit   , prob put this script and collider on the parent
    }
}
