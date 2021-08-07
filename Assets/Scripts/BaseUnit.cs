using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit
{
    public BaseUnit(Vector2 position, Vector2 size, float maxVelocity, float speed, float mass)
    {
        this.position = position;
        this.size = size;
        this.maxVelocity = maxVelocity;
        this.accel = speed;
        this.mass = mass;
        UnitController.activeUnits.Add(this);
    }

    //protected float posX;
    //protected float posY;

    protected Vector2 position;
    protected Vector2 size;

    //protected float sizeX = 2;
    //protected float sizeY = 3;

    protected float velocityX;
    protected float velocityY;

    protected float maxVelocity = 1f;
    protected float drag = 1f;
    protected float accel = 0.5f;

    protected float mass = 1f;

    protected bool isGrounded;

    public virtual void HandleMovement()
    {
        //UnitController.activeUnits.TryGetValue(this, out Vector2 newUnitPos);
        //newUnitPos = position;
        //UnitController.activeUnits[this] = newUnitPos;
        UpdateUnitPos();
    }
    protected virtual void HandleFunction()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    player.PrimaryFunction();
        //}
        //if (Input.GetMouseButton(1))
        //{
        //    player.PrimaryFunction();
        //}
    }
    protected virtual void MovementUp()
    {
        //UpdateVelocity(Vector2.up);
    }
    protected virtual void MovementDown()
    {
        //UpdateVelocity(Vector2.down);
    }
    public virtual void MovementLeft()
    {
        UpdateVelocity(new Vector2(-1 * (Time.fixedDeltaTime + accel), 0));
    }
    public virtual void MovementRight()
    {
        UpdateVelocity(new Vector2(1 * (Time.fixedDeltaTime + accel), 0));
    }
    public virtual void MovementJump()
    {
        UpdateVelocity(new Vector2(0, (maxVelocity * 10) - mass));
    }
    protected virtual void PrimaryFunction()
    {
        
    }
    protected virtual void SecondaryFunction()
    {
        
    }
    private void UpdateVelocity(Vector2 direction)
    {
        //velocityX += direction.x * (Time.deltaTime + accel);
        //velocityY += direction.y * (Time.deltaTime + accel);
        Debug.Log(direction);

        velocityX = isGrounded ? Mathf.Clamp(velocityX + direction.x, -maxVelocity, maxVelocity) : Mathf.Clamp(velocityX + direction.x, -drag, drag);
        velocityY = Mathf.Clamp(velocityY + direction.y, -maxVelocity, maxVelocity);

        //velocityX += isGrounded ? Mathf.Clamp(direction.x * Time.deltaTime, -maxVelocity, maxVelocity) : Mathf.Clamp(direction.x, -drag, drag);
        //velocityY += Mathf.Clamp(direction.y * Time.deltaTime, -maxVelocity, maxVelocity);

        //velocityX = isGrounded ? (Mathf.Clamp(velocityX, -maxVelocity, maxVelocity)) : (Mathf.Clamp(velocityX, -drag, drag));
        //velocityY = Mathf.Clamp(velocityY, -maxVelocity, maxVelocity);
    }
    public void Jump(float y)
    {
        velocityY = Mathf.Clamp(velocityY + y, -maxVelocity, maxVelocity);
    }
    public Vector2 UpdateUnitPos()
    {
        float newPosX = Mathf.Clamp(position.x + velocityX, 0, World.GetWidth() - 1);
        float newPosY = Mathf.Clamp((position.y + velocityY), 0, World.GetHeight() - 1);
        isGrounded = false;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {

                if (velocityX <= 0)
                {
                    if (World.IsBlock(newPosX + x, position.y + y) || World.IsBlock(newPosX + x, position.y + 0.9f + y))
                    {
                        //Debug.Log("Collided Left");
                        newPosX = Mathf.FloorToInt(newPosX) + 1;
                        velocityX = 0;
                    }
                }
                else
                {
                    if (World.IsBlock(newPosX + 1f + x, position.y + y) || World.IsBlock(newPosX + 1f + x, position.y + 0.9f + y))
                    {
                        //Debug.Log("Collided Right");
                        newPosX = Mathf.FloorToInt(newPosX);
                        velocityX = 0;
                    }
                }

                if (velocityY <= 0)
                {
                    if (World.IsBlock(newPosX + x, newPosY + y) || World.IsBlock(newPosX + 0.9f + x, newPosY + y))
                    {
                        //Debug.Log("Collided Down");
                        newPosY = Mathf.FloorToInt(newPosY) + 1;
                        velocityY = 0;
                        isGrounded = true;
                    }
                }
                else
                {
                    if (World.IsBlock(newPosX + x, newPosY + 1f + y) || World.IsBlock(newPosX + 0.9f + x, newPosY + 1f + y))
                    {
                        //Debug.Log("Collided Up ");
                        newPosY = Mathf.FloorToInt(newPosY);
                        velocityY = 0;
                    }
                }
            }
        }


        velocityY -= ((World.gravity + mass) * Time.fixedDeltaTime);

        velocityX = Mathf.MoveTowards(velocityX, 0, accel);
        position = new Vector2(newPosX, newPosY);

        return position;
    }
    //public Vector2 GetSize()
    //{
    //    return new Vector2(sizeX, sizeY);
    //}
    public Vector2 GetPosition()
    {
        return position;
    }
}
