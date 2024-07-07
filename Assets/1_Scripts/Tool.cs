using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tool : Interactable
{
    public ToolType toolType;
    
    private Vector3 originalPos;
    
    private Vector3 lookPos;
    
    [SerializeField] Transform toolObject;
    
    void Awake()
    {
        // base.Start();
        originalPos = transform.position;
        lookPos = new Vector3(originalPos.x, transform.position.y + 0.07f, originalPos.z);
    }

    private void OnEnable()
    {
        transform.position = originalPos;
    }

    public override void Interact(Player player)
    {
        player.SelectTool(toolType);
        gameObject.SetActive(false);
    }

    public override void OnLook()
    {

        toolObject.DOMove(lookPos, 0.15f);
        StopAllCoroutines();
        StartCoroutine(GoToOriginalPos());
    }
    
    private IEnumerator GoToOriginalPos()
    {
        // canMove = false;
        yield return new WaitForEndOfFrame();
        toolObject.DOMove(originalPos, 0.3f);

    }
}
