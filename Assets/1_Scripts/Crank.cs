using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crank : Interactable
{
  [SerializeField] private float maxElectricity = 100;
  [SerializeField] public float drainRate = 1;
  [SerializeField] private float fillRate = 10;
  private float baseFillRate;
  
  [Space]
  
  //todo maybe use event Action OnLightsOut to avoid dependencies
  
  [SerializeField] private Transform barTransform;
  [SerializeField] private GameObject redLight;
  [SerializeField] private GameObject[] lightBulbs;
  [SerializeField] private Transform crankObject;
  
  [HideInInspector] public float currentElectricity;
  private GameObject[] lights;

  [HideInInspector] public GameObject[] lightSwitches;

  private float maxScaleX;

  private int lightsOn = 0;
  private bool isDraining = true;
  private bool isDark = false;
  private void Start()
  {
    base.Start();

    lightSwitches = GameObject.FindGameObjectsWithTag("LightSwitch");
    lightSwitches = lightSwitches.OrderBy(obj => obj.transform.position.x).ToArray();
    
    lightBulbs = lightBulbs.OrderBy(obj => obj.transform.position.x).ToArray();

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
      if (lightsOn == 1)
      {
        drainRate = 2;
      }

      if (lightsOn == 2)
      {
        drainRate = 4;
      }

      if (lightsOn == 3)
      {
        currentElectricity = 0;
      }

      isDraining = true;
      currentElectricity -= drainRate * Time.deltaTime;
      // Debug.Log(currentElectricity);
      if (currentElectricity < 0)
      {
        GoDark();
      }
    }
    
    float scale = (currentElectricity / maxElectricity) * maxScaleX; // Calculate scaled value
    barTransform.localScale = new Vector3(scale, barTransform.localScale.y, barTransform.localScale.z);
  }

  void GoDark()
  {
    //todo for sure C# event and increase exposure so all is dark
    currentElectricity = 0;
    lightsOn = 0;
    if (isDark) return;
    foreach (var light in lights)
    {
      light.SetActive(false);
      redLight.SetActive(true);
      isDark = true;
    }
      
    //todo prob change so they are lightswitches from the beginning and dont need to getComponent, might use list.
    foreach (GameObject lightSwitch in lightSwitches)
    {
      lightSwitch.GetComponent<LightSwitch>().TurnOff();
    }
        
    fillRate *= 0.5f;
  }
  
  public override void Interact(Player player)
  {
    if (isDark && currentElectricity >= 20)
    {
      foreach (var light in lights)
      {
        light.SetActive(true);
      }
      redLight.SetActive(false);
      isDark = false;
      fillRate = baseFillRate;
    }
    
    crankObject.Rotate(Vector3.up * 200 * Time.deltaTime, Space.Self);
    isDraining = false;
    currentElectricity+= fillRate * Time.deltaTime;
    
    if (currentElectricity > maxElectricity)
    {
      currentElectricity = maxElectricity;
    }
    
    StopAllCoroutines();
    StartCoroutine(EnableDrain());
  }
  
  
  
  public void ToggleLightSwitch(GameObject lightSwitch, bool isOn)
  {
    for (int i = 0; i < lightSwitches.Length; i++)
    {
      if (lightSwitches[i] == lightSwitch)
      {
        lightsOn += isOn ? 1 : -1;
        lightBulbs[i].SetActive(!isOn);
      }
    }
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
