using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    public PlayerUnit(Vector2 position, Vector2 size, float maxVelocity, float speed, float mass) : base(position, size, maxVelocity, speed, mass) { }

    [SerializeField] private Material material;

    private Player player;

    public override void HandleMovement()
    {
        base.HandleMovement();
    }
    //public void Set
}
