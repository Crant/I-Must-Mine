using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController
{
    public static List<BaseUnit> activeUnits = new List<BaseUnit>();

    private List<BaseUnit> keys = new List<BaseUnit>();

    public void HandleUnitMovement()
    {
        for (int i = 0; i < activeUnits.Count; i++)
        {
            //if (Input.GetKey(KeyCode.A))
            //{
            //    activeUnits[i].MovementLeft();
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    activeUnits[i].MovementRight();
            //}
            //if (Input.GetKey(KeyCode.Space) && activeUnits[i].isGrounded)
            //{
            //    activeUnits[i].MovementJump();
            //}
            //Camera.main.transform.position = new Vector3(activeUnits[i].GetPosition().x, activeUnits[i].GetPosition().y, -10);
            activeUnits[i].HandleMovement();
        }
    }
}
