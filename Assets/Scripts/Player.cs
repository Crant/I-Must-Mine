using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{

    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode jumpButton;

    private ClientRenderer renderer;
    private PlayerUnit unit;

    private void FixedUpdate()
    {
        if (Input.GetKey(moveRight))
        {
            unit.MovementRight();
        }
        if (Input.GetKey(moveLeft))
        {
            unit.MovementLeft();
        }
        if (Input.GetKey(jumpButton) && unit.isGrounded)
        {
            unit.MovementJump();
        }
        unit.HandleMovement();
    }
}
