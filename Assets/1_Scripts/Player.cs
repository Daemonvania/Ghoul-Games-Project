using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    
    //TODO FPS is fucked

    //todo script can be split into multiple
    //todo it breaks sometimes, might be the not switching inputs off
    
    //VARIABLES TO CHANGE
    [SerializeField] private float timeToArrive = 2;
    [Space]
    
    //MOVEMENT FUNCTIONALITY
    public Transform baseLocation;
    [SerializeField] Transform backLocation;

    [HideInInspector] public Transform currentTransform;
    private Transform lastTransform;
    private Location currentLocation;
    private Vector3 initCamPos;
    
    //TOOL FUNCTIONALITY
    [HideInInspector] public ToolType selectedTool = ToolType.None;
    [FormerlySerializedAs("_bench")] [SerializeField] private Counter counter;
    [SerializeField] public HandTool[] handTools;
    
    //Input
    public PlayerInputActions playerControls;
    private InputAction selectTool1;
    private InputAction selectTool2;
    private InputAction selectTool3;
    private InputAction interact;
    private bool isInteracting;

    
    
    //MISC
    [SerializeField] Animator cameraAnimator;
 

    private enum states
    {
        Unfocused,
        Focused
    }

    private states currenState = states.Unfocused;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        lastTransform = baseLocation;
        currentTransform = baseLocation;
        lastTransform = baseLocation;
        initCamPos = transform.position;

        transform.position = baseLocation.position;
        transform.rotation = baseLocation.rotation;
        foreach (HandTool handTool in handTools)
        {
            handTool.toolHandObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        selectTool1 = playerControls.Player.SelectItem1;
        selectTool2 = playerControls.Player.SelectItem2;
        selectTool3 = playerControls.Player.SelectItem3;
        selectTool1.Enable();
        selectTool2.Enable();
        selectTool3.Enable();
        interact.Enable();
        
        //todo make it interact only once with everything except button, might do it within Interact, idk man
        // interact.performed += ctx => isInteracting = !isInteracting;
        
        // interact.performed += Interact;
    }

    private void OnDisable()
    {
        interact.Disable();
        selectTool1.Disable();
        selectTool2.Disable();
        selectTool3.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectTool1.triggered)
        {
            Debug.Log("tool1");
            SelectTool(ToolType.Water);
        }
        else if (selectTool2.triggered)
        {
            SelectTool(ToolType.Fertilizer);
        }
        else if (selectTool3.triggered)
        {
            SelectTool(ToolType.Pruner);
        }
        
        var mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (currenState == states.Unfocused)
        {
            if (mousePosition.y <= 150)
            {
                //moveBack
                if (interact.triggered)
                {
                    if (currentTransform == baseLocation)
                    {
                        MoveToLocation(backLocation);
                    }
                    else
                    {
                        MoveToLocation(baseLocation);
                    }
                    return;
                }
            }
            // If player is looking around and clicks on a location, move to it
            if (Physics.Raycast(ray, out hitInfo, 300))
            {
                Location locationComponent = hitInfo.collider.GetComponent<Location>();
                if (locationComponent != null)
                {
                    if (interact.triggered)
                    {
                        currenState = states.Focused;
                        LocationClick(locationComponent);
                    }
                }
            }

            //CAMERA SCROLLING
            float edgeThreshold = 40.0f; 
            float moveSpeed = 2.0f;

            Vector3 cameraRight = Camera.main.transform.right; // Right direction relative to the camera

            if (mousePosition.x <= edgeThreshold)
            {
                transform.position -= cameraRight * moveSpeed * Time.deltaTime;
            }
            else if (mousePosition.x >= Screen.width - edgeThreshold)
            {
                transform.position += cameraRight * moveSpeed * Time.deltaTime;
            }
            

            //Clamp camera position
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, initCamPos.x - 3, initCamPos.x + 3);
            transform.position = clampedPosition;
        }
        else if (currenState == states.Focused)
        {
            //Move back to base location
            if (mousePosition.y <= 150)
            {
                if (interact.triggered)
                {
                    currenState = states.Unfocused;
                    currentLocation.OnExit();
                    currentLocation = null;
                    if (lastTransform == baseLocation)
                    {
                        MoveToLocation(baseLocation);
                    }
                    else
                    {
                        MoveToLocation(backLocation);
                    }
                    currenState = states.Unfocused;
                    return;
                }
            }
            
            if (Physics.Raycast(ray, out hitInfo, 300))
            {
              
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.OnLook();
                 
                    if (hitInfo.collider.CompareTag("Crank"))
                    {
                        // Continuously interact while the key is held down
                        if (interact.IsPressed())
                        {
                            interactable.Interact(this);
                        }
                    }
                    else
                    {
                        if (interact.triggered)
                        {
                            Debug.Log("interacting");
                            interactable.Interact(this);
                        }
                        // wasInteracting = isInteracting;
                    }
                }
            }

          
        }
    }


    void LocationClick(Location locationComponent)
    {
        //todo delay stuff and remove input until the player is at the location
        currentLocation = locationComponent;
        currentLocation.OnArrive();
        interact.Disable();
        MoveToLocation(currentLocation.cameraPosition);

    }


    void MoveToLocation(Transform location)
    {
        if (currenState == states.Unfocused)
        {
            if (currentTransform == baseLocation && location == backLocation ||
                currentTransform == backLocation && location == baseLocation)
            {
                Turn(location);
                return;
            }
            else
            {
                print(currentTransform + "and" + baseLocation);
                transform.DORotate(new Vector3(60f, location.rotation.y + 180, location.rotation.z), 0.6f,
                    RotateMode.LocalAxisAdd);
            }
        }
        else
        {
            transform.DORotate(new Vector3(30f, location.rotation.y, location.rotation.z), 0.5f,
                    RotateMode.LocalAxisAdd);
        }

        transform.DOMove(location.position, timeToArrive);
        // Play the sequence
        cameraAnimator.SetBool("headbop", true);
        StartCoroutine(ArriveAtLocation(location));
    }

    private void Turn(Transform location)
    {
        transform.DORotate(location.rotation.eulerAngles, 0.6f);
        transform.DOMove(location.position, 0.6f);
        lastTransform = currentTransform;
        currentTransform = location;
    }

    private IEnumerator ArriveAtLocation(Transform location)
    {
        float timeToLook = timeToArrive * 0.57f;
        yield return new WaitForSeconds(timeToLook);
        
        cameraAnimator.SetBool("headbop", false);
        interact.Enable();
        transform.DORotate(location.rotation.eulerAngles, 0.7f);
        lastTransform = currentTransform;
        currentTransform = location;
        
        //might needa to better
        if (currentLocation != null)
        {
             Pot pot = currentLocation.GetComponent<Pot>();
                if (pot)
                {
                    pot.ShowTools();
                }
        }
     
    }


    public void SelectTool(ToolType tool)
    {
        //todo probably should do this somewhere else
        // foreach (Interactable item in counter.items)
        // {
        //     item.gameObject.SetActive(true);
        // }
        //
        selectedTool = tool;
        foreach (HandTool handTool in handTools)
        {
            if (handTool.toolType == tool)
            {
                print("Selected tool: " + tool);
                handTool.toolHandObject.SetActive(true);
            }
            else
            {
                print("Selected tool: " + tool);
                handTool.toolHandObject.SetActive(false);
            }
        }
    }

    // void Interact(InputAction.CallbackContext context)
    // {
    //     isInteracting = !isInteracting;
    // }
}