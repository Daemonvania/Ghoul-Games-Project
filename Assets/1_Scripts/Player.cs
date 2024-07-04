using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    //TODO FPS is fucked
    
    public PlayerInputActions playerControls;
    [SerializeField] Transform baseLocation;
    [SerializeField] Transform backLocation;

    private Transform currentLocation;
    private InputAction interact;
    private bool isInteracting;

    private Vector3 initCamPos;

    private enum states
    {
        Unfocused,
        Focused
    }

    private states currenState = states.Unfocused;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        currentLocation = baseLocation;
        initCamPos = transform.position;
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
            // If player is looking around and clicks on a location, move to it
            if (Physics.Raycast(ray, out hitInfo, 300))
            {
                Location locationComponent = hitInfo.collider.GetComponent<Location>();
                if (locationComponent != null)
                {
                    if (interact.triggered)
                    {
                        //todo disable location collider..? check what the video does as well
                        MoveToLocation(locationComponent.cameraPosition);
                        currenState = states.Focused;
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


            if (mousePosition.y <= 150)
            {
                if (interact.triggered)
                {
                    if (currentLocation == baseLocation)
                    {
                        MoveToLocation(backLocation);
                    }
                    else
                    {
                        MoveToLocation(baseLocation);
                    }
                }
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
                // Check if the object hit is the one you want (e.g., by tag or layer)
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (interact.triggered)
                    {
                       interactable.Interact();
                    }
                }
            }

            if (mousePosition.y <= 150)
            {
                if (interact.triggered)
                {
                    MoveToLocation(baseLocation);
                    currenState = states.Unfocused;
                }
            }
        }
    }

    void MoveToLocation(Transform location)
    {
        //todo make smooth turning to look at it and then walking with head bump (idk how but we'll manage)
        transform.position = location.position;
        transform.rotation = location.rotation;
        currentLocation = location;
    }


//     void Interact(InputAction.CallbackContext context)
//     {
//         if (currenState == states.Focused)
//         {
//       
//         }
}