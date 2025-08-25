using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //always make the variables private to ensure that only the class they belong to can make changes to it
    //adding serializableField ensures that we can change the value while on the scene screen
    [SerializeField] private float moveSpeed = 7f;
    private bool isWalking;
    [SerializeField] private GameInput gameInput;
    // A LayerMask that tells Unity which layers the interaction raycast should detect (e.g., counters, interactable objects)
    [SerializeField] private LayerMask countersLayersMask;

    private Vector3 lastInteraction;
    private void Update()
    {
        HandleMovement(); 
        HandleInteractions();   
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        // Get the player's current movement input as a normalized 2D vector (x and y between -1 and 1)
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
 
        // Convert the 2D input into a 3D direction vector (x for left/right, z for forward/backward, y stays 0 since we are on a flat plane)
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Set the maximum distance the interaction raycast can reach
        float interactDistance = 2f;

        // If the player has provided movement input (not standing still), update the last interaction direction
        if (moveDir != Vector3.zero)
        {
            lastInteraction = moveDir;
        }

        // Declare a variable that will store information about objects hit by the raycast
        RaycastHit raycastHit;

        // Perform a raycast starting from the player's position,
        // shooting in the direction of the lastInteraction vector,
        // up to interactDistance units, and only hitting objects on the countersLayersMask
        if (Physics.Raycast(transform.position, lastInteraction, out raycastHit, interactDistance,countersLayersMask))
        {
            // Check if the object hit by the raycast has a ClearCounter component
            // (TryGetComponent returns true if found, false otherwise, and assigns the component if successful)
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //Hit a clearCounter
                clearCounter.Interact();
            }
        }
        else
        {

        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //we can move the player only in x and the z axis. However, it has the ability to move in 3 directions. 
        //Hence we convert the Vector2D to Vector3D by creating a new Vector3D and assigning its x and z values from the
        //inputVector
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //for collision
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance); // casts a laser in the direction. bool canMove tells whether there is something in the way or not

        if (!canMove)
        {
            //Cannot move towards moveDir direction
            //attempt only X direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized; // we normalize it to ensure that it moves in the available direction at 1 speed
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // can move only in x direction
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only in x, so attempt moving on the Z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized; // we normalize it to ensure that it moves in the available direction at 1 speed
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //can move in the z direction
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }

            }
        }
        if (canMove)
        {
            //deltaTime changes the speed based on the refresh rate of the screen.
            transform.position += moveDir * moveDistance;
        }


        float rotateSpeed = 10f;
        isWalking = moveDir != Vector3.zero;
        //to make the player face in the direction of movement. Slerp is used to smoothen the movement
        //Slerp has 3 args. First to where it begins, then where it should end, and Time.deltaTime to tell it should move smooth
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
