using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    public abstract void Interact(Player player);
    public abstract void OnLook();

    [HideInInspector]  public Collider col;

    protected void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
