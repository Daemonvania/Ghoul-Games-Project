using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsPoster : Interactable
{
    public GameObject[] pages;
    int currentPage = 0;
    void Start()
    {
        base.Start();
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        pages[currentPage].SetActive(true);
    }
    
    public override void Interact(Player player)
    {
        currentPage++;
        if (currentPage >= pages.Length)
        {
            currentPage = 0;
        }
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        pages[currentPage].SetActive(true);
    }
    
    public override void OnLook()
    {
        
    }
}
