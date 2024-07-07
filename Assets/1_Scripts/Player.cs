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
    [SerializeField] Transform baseLocation;
    [SerializeField] Transform backLocation;

    private Transform currentTransform ;
    private Transform lastTransform;
    private Location currentLocation;
    private Vector3 initCamPos;
    
    //TOOL FUNCTIONALITY
    [HideInInspector] public ToolType selectedTool;
    [FormerlySerializedAs("_bench")] [SerializeField] private Counter counter;
    [SerializeField] public HandTool[] handTools;
    
    //INTERACTION
    public PlayerInputActions playerControls;
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
        interact.Enable();
        // interact.performed += Interact;
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    // Update is called once per frame
    void Update()
    {
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
            float edgeThreshold = 50.0f; // Adjust this value as needed
            float moveSpeed = 5.0f;

            Vector3 cameraRight = Camera.main.transform.right; // Right direction relative to the camera

            if (mousePosition.x <= edgeThreshold)
            {
                transform.position -= cameraRight * moveSpeed * Time.deltaTime;
            }
            else if (mousePosition.x >= Screen.width - edgeThreshold)
            {
                transform.position += cameraRight * moveSpeed * Time.deltaTime;
            }


         
            // else if (mousePosition.y >= Screen.height - 1)
            // {
            //     Debug.Log("Up");
            // }

            //Clamp camera position
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, initCamPos.x - 10, initCamPos.x + 10);
            transform.position = clampedPosition;
        }
        else if (currenState == states.Focused)
        {
            if (Physics.Raycast(ray, out hitInfo, 300))
            {
              
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.OnLook();
                    if (interact.triggered)
                    {
                       interactable.Interact(this);
                    }
                }
            }

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
                    
                    //maybe create last location, then that could help with the "base" and "back" location thing as well, you always just go to previous
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
            transform.DORotate(new Vector3(60f, location.rotation.y, location.rotation.z), 0.5f,
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
        float timeToLook = timeToArrive / 1.75f;
        yield return new WaitForSeconds(timeToLook);
        cameraAnimator.SetBool("headbop", false);
        interact.Enable();
        transform.DORotate(location.rotation.eulerAngles, 0.7f);
        lastTransform = currentTransform;
        currentTransform = location;
    }


    public void SelectTool(ToolType tool)
    {
        //todo probably should do this somewhere else
        foreach (Interactable item in counter.items)
        {
            item.gameObject.SetActive(true);
        }
        
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
//     void Interact(InputAction.CallbackContext context)
//     {
//         if (currenState == states.Focused)
//         {
//       
//         }
}