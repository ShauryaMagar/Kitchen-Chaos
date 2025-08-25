using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        //this is the Player Move Map that we created in the PlayerInputActions window
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log(obj);
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
