using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Location
{
    [SerializeField] private GameObject[] tools;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        HideTools();
    }

    public void ShowTools()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(true);
        }
    }
    
    public void HideTools()
    {
        foreach (var tool in tools)
        {
            tool.SetActive(false);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        HideTools();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
