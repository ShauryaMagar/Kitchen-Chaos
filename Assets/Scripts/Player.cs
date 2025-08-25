using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //always make the variables private to ensure that only the class they belong to can make changes to it
    //adding serializableField ensures that we can change the value while on the scene screen
    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;
    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }
        //this is to ensure when 2 keys are pressed at the same time, the magnitude of the speed of the player is still 1 and not more than that
        inputVector = inputVector.normalized;

        //we can move the player only in x and the z axis. However, it has the ability to move in 3 directions. 
        //Hence we convert the Vector2D to Vector3D by creating a new Vector3D and assigning its x and z values from the
        //inputVector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //deltaTime changes the speed based on the refresh rate of the screen.
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir*Time.deltaTime*moveSpeed;

        float rotateSpeed = 10f;
        //to make the player face in the direction of movement. Slerp is used to smoothen the movement
        //Slerp has 3 args. First to where it begins, then where it should end, and Time.deltaTime to tell it should move smooth
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed);  
        Debug.Log(inputVector);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
