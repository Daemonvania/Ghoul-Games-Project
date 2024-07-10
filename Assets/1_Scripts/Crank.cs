using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : Interactable
{
  [SerializeField] private float maxElectricity = 100;
  [SerializeField] private float drainRate = 1;
  [SerializeField] private float fillRate = 10;
  private float baseFillRate;
  
  [SerializeField] private Transform barTransform;
  [SerializeField] private GameObject redLight;
  private float maxScaleX;
  [HideInInspector] public float currentElectricity;

  private GameObject[] lights;
  
  private bool isDraining = true;
  private bool isDark = false;
  private void Start()
  {
    base.Start();
    maxScaleX = barTransform.localScale.x;
    currentElectricity = maxElectricity;
    baseFillRate = fillRate;
    redLight.SetActive(false);
    lights = GameObject.FindGameObjectsWithTag("Light");
  }
  
  private void Update()
  {
    if (isDraining)
    {
      Debug.Log("draining");

      isDraining = true;
      currentElectricity -= drainRate * Time.deltaTime;
      // Debug.Log(currentElectricity);
      if (currentElectricity < 0)
      {
        currentElectricity = 0;
        if (isDark) return;
        foreach (var light in lights)
        {
          light.SetActive(false);
          redLight.SetActive(true);
          isDark = true;
        }
      }
    }
    
    float scale = (currentElectricity / maxElectricity) * maxScaleX; // Calculate scaled value
    barTransform.localScale = new Vector3(scale, barTransform.localScale.y, barTransform.localScale.z);
  }
  
  public override void Interact(Player player)
  {
    if (isDark && currentElectricity >= 20)
    {
      foreach (var light in lights)
      {
        //todo make it slow down fill rate or take a delay to start, punishment for letting it like this and tense
        light.SetActive(true);
      }
      redLight.SetActive(false);
      isDark = false;
    }
    
    isDraining = false;
    currentElectricity+= fillRate * Time.deltaTime;
    
    if (currentElectricity > maxElectricity)
    {
      currentElectricity = maxElectricity;
    }
    
    StopAllCoroutines();
    StartCoroutine(EnableDrain());
  }

  private IEnumerator EnableDrain()
  {
    yield return new WaitForSeconds(0.1f);
    isDraining = true;
    }
  public override void OnLook()
  {
    
  }
  
}
