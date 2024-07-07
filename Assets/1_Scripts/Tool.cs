using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tool : Interactable
{
    public ToolType toolType;

    private bool isLookedAt;
    
    private Vector3 originalPos;
    
    private Vector3 lookPos;
    
    void Awake()
    {
        // base.Start();
        originalPos = transform.position;
        lookPos = new Vector3(originalPos.x, transform.position.y + 0.15f, originalPos.z);
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
        isLookedAt = true;
        print(lookPos);
        transform.DOMove(lookPos, 0.3f);
        StopAllCoroutines();
        StartCoroutine(GoToOriginalPos());
    }
    
    private IEnumerator GoToOriginalPos()
    {
        yield return new WaitForEndOfFrame();
        isLookedAt = false;
        transform.DOMove(originalPos, 0.3f);
            isLookedAt = false;
    }
}
