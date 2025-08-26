using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction; //Declares a public event named OnInteractAction. The event will be fired whenever the player performs an Interact action.
    private PlayerInputActions inputActions;
    private void Awake()
    {
        // Create a new instance of the PlayerInputActions class
        // (this is the auto-generated class from your Input Actions asset)
        inputActions = new PlayerInputActions();

        //this is the Player Move Map that we created in the PlayerInputActions window
        // Enable the "Player" action map so Unity starts listening to inputs defined under it
        inputActions.Player.Enable();

        // Subscribe to the "Interact" action's performed event
        // This means whenever the player presses the bound Interact key/button,
        // the Interact_performed method will be called
        inputActions.Player.Interact.performed += Interact_performed; 
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty); //Fires the OnInteractAction Event. All the methods subscribed to it will also get fired
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        //Old System
        //if (Input.GetKey(KeyCode.W))
        //{
        //    inputVector.y = 1;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    inputVector.x = -1;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    inputVector.y = -1;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    inputVector.x = 1;
        //}
        //this is to ensure when 2 keys are pressed at the same time, the magnitude of the speed of the player is still 1 and not more than that
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
