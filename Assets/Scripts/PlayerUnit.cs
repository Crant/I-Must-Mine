//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerUnit : BaseUnit
//{
//    [SerializeField] private KeyCode moveRight;
//    [SerializeField] private KeyCode moveLeft;
//    [SerializeField] private KeyCode jumpButton;

//    [SerializeField] private Material material;

//    private ClientRenderer renderer;

//    public PlayerUnit(Vector2 position, Vector2 size, float maxVelocity, float speed, float mass)
//    {
//        this.position = position;
//        this.size = size;
//        this.maxVelocity = maxVelocity;
//        this.accel = speed;
//        this.mass = mass;
//        UnitController.activeUnits.Add(this);
//    }

//    public override void HandleMovement()
//    {
//        if (Input.GetKey(moveRight))
//        {
//            MovementRight();
//        }
//        if (Input.GetKey(moveLeft))
//        {
//            MovementLeft();
//        }
//        if (Input.GetKey(jumpButton) && isGrounded)
//        {
//            MovementJump();
//        }
//        base.HandleMovement();
//    }
//    //public void Set
//}
