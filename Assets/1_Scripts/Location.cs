using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Location : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraPosition;

    [HideInInspector] public Collider col;
    
    // Update is called once per frame
    private void Start()
    {
        col = GetComponent<Collider>();
    }
    
}
