using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void CharacterControlDelegate();
public class CharacterControl : MonoBehaviour
{

    public event CharacterControlDelegate eventGetInVehicles;
    public Joystick joystick;
    public GameObject getInVehicles;
    public GameObject rope;
    public bool isJump;
    public bool isAttack;
    public bool isSprint;
    public bool isSwing;
    public bool isRope;
    public bool isLaser;
    public bool isRocket;
    public void Jump()
    {
        isJump = true;
    }
    public void FinishJump()
    {
        isJump = false;
    }
    public void Sprint()
    {
        isSprint = true;
    }
    public void FinishSprint()
    {
        isSprint = false;
    }
    public void Actack()
    {
       isAttack=true;
       
    }
    public void FinishAttack()
    {
        isAttack = false;

    }
    public void GetVehicles()
    {
       
        if (eventGetInVehicles != null)
        {
            eventGetInVehicles();
            Debug.Log("not mujll");
        }

    }
    public void Swing()
    {
        isSwing = true;
    }
    public void FinishSwing()
    {
        isSwing = false;
    }
    public void ShotSilk()
    {
        isRope = true;
    }
    public void FinishShotSilk()
    {
        isRope = false;
    }
    public void Laser()
    {
        isLaser = true;
    }
    public void FinishLaser()
    {
        isLaser = false;
    }
    public void Rocket()
    {
        isRocket = true;
    }
    public void FinishRocket()
    {
        isRocket = false;
    }
}
