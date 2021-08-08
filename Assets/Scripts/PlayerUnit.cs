using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    public PlayerUnit(Vector2 position, Vector2 size, float maxVelocity, float speed, float mass) : base(position, size, maxVelocity, speed, mass) { }

    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode jumpButton;

    [SerializeField] private Material material;

    private ClientRenderer renderer;

    

    public override void HandleMovement()
    {
        if (Input.GetKey(moveRight))
        {
            MovementRight();
        }
        if (Input.GetKey(moveLeft))
        {
            MovementLeft();
        }
        if (Input.GetKey(jumpButton) && isGrounded)
        {
            MovementJump();
        }
        base.HandleMovement();
    }
    //public void Set
}
