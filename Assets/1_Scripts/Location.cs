using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class  Location : MonoBehaviour
{

    public Transform cameraPosition;

    [HideInInspector] public Collider col;

    [FormerlySerializedAs("Items")] public Interactable[] items;

    protected void Start()
    {
        col = GetComponent<Collider>();
    }

    public void OnArrive()
    {
        col.enabled = false;
        foreach (var item in items)
        {
            item.col.enabled = true;
        }
    }
    public void OnExit()
    {
        col.enabled = true;
        foreach (var item in items)
        {
            item.col.enabled = false;
        }
    }
}
