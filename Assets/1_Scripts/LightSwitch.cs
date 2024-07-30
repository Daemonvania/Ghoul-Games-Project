using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Palmmedia.ReportGenerator.Core;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightSwitch : Interactable
{
    
    [SerializeField] private GameObject light;
    [SerializeField] private PlantStateManager plant;
    [SerializeField] private bool canInteract = true;
    
    
    [SerializeField] private Transform switchObject;
    private Vector3 originalPos;
    private Vector3 lookPos;
    private Vector3 pulledPos;
    
    private bool isOn = false;

    private Crank crank;
    
    void Start()
    {
        base.Start();
        crank = GameObject.FindGameObjectWithTag("Crank").GetComponent<Crank>();
        light.SetActive(false);
        originalPos = transform.position;
        lookPos = new Vector3(originalPos.x, transform.position.y + 0.035f, originalPos.z);
        pulledPos = new Vector3(originalPos.x, transform.position.y - 0.050f, originalPos.z);
    }
    
    public override void Interact(Player player)
    {
        if (!canInteract)
        {
            return;
        }
        canInteract = false;
        isOn = !isOn;
        
        switchObject.DOMove(pulledPos, 0.5f).OnComplete(() =>
        {
            light.SetActive(isOn);
            plant.LightChange(isOn);
            crank.ToggleLightSwitch(gameObject, isOn);

            switchObject.DOMove(originalPos, 0.5f).OnComplete(() =>
            {
                canInteract = true;
                Debug.Log("oiasd");
            });
        });
        
        if (isOn)
        {
            StartCoroutine(autoTurnOff());
        }
    }

    private IEnumerator autoTurnOff()
    {
        yield return new WaitForSeconds(Random.Range(12, 22));
        
        isOn = false;
        light.SetActive(isOn);
        plant.LightChange(isOn);
        crank.ToggleLightSwitch(gameObject, isOn);
    }
    
    
    //only for when energy runs out
    public void TurnOff()
    {
        isOn = false;
        light.SetActive(isOn);
        plant.LightChange(isOn);
    }
    
    public override void OnLook()
    {
        if (!canInteract)
        {
            return;
        }
        switchObject.DOMove(lookPos, 0.15f);
        StopAllCoroutines();
        StartCoroutine(GoToOriginalPos());
    }
    
    private IEnumerator GoToOriginalPos()
    {
        // canMove = false;
        yield return new WaitForEndOfFrame();
        switchObject.DOMove(originalPos, 0.3f);

    }
}
