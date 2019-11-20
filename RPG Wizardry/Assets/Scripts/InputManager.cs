﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private MovementManager movementManager;

    //serialized for easy inspector switching
    [SerializeField]
    private GameState gameState = GameState.gameplay;
    [SerializeField]
    private ControlScheme controlScheme;
    public int MovementMultiplier = 1;

    //outgoing movement values
    public Vector3 InputMovement { get; private set; }
    //outgoing aiming values
    public Vector3 InputAiming { get; private set; }
    public bool Abutton;

    private Image draggedImage;
    private Vector3 mousePos;

    [SerializeField]
    private GraphicRaycaster caster;
    [SerializeField]
    private EventSystem eventSystem;

    //public delegate void OnMovement(Vector3 movement);
    //public static OnMovement onMovement; 

    private void Start()
    {
        movementManager = GetComponent<MovementManager>();
    }

    //checks inputs based on gamestate
    private void Update()
    {
        if (gameState == GameState.gameplay)
        {
            MovementInputs();
            AimingInputs();
        }
        else if (gameState == GameState.menu)
        {
            MouseInputs();
        }
    }

    /// <summary>
    /// collects unity input while in gameplay state
    /// </summary>
    private void MovementInputs()
    {
        //collect movement input, multiply for effectiveness,  save for movementmanager
        InputMovement = new Vector3(Input.GetAxis("Horizontal") * MovementMultiplier,
            Input.GetAxis("Vertical") * MovementMultiplier, 0.0f);
    }

    private void AimingInputs()
    {
        //collect aiming input
        InputAiming = new Vector3(Input.GetAxis("RightX"),
            Input.GetAxis("RightY"), 0.0f);
        Abutton = Input.GetButton("Fire1");
    }

    private void MouseInputs()
    {
        // Click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            // Raycast and select

            PointerEventData data = new PointerEventData(eventSystem);
            data.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            caster.Raycast(data, results);

            if (results.Count > 0)
            {
                Debug.Log("Hit");
                draggedImage = results[0].gameObject.GetComponent<Image>();
                mousePos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) && draggedImage != null) // Held (Drag)
        {
            Debug.Log("Drag");
            // Move Image-Pos
            float deltaY = Input.mousePosition.y - mousePos.y;
            draggedImage.transform.Translate(0, deltaY, 0, Space.Self);
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Released
        {
            Debug.Log("Release");
            // Deselect
            draggedImage = null;
        }
    }
}

/// <summary>
/// enum for facing the right direction
/// </summary>
public enum WalkDirection
{
    East = 0,
    North = 1,
    West = 2,
    South = 3
}

/// <summary>
/// enum for switching game modes easily
/// </summary>
public enum GameState
{
    menu,
    gameplay
}

public enum ControlScheme
{
    controller,
    keyboard
}