using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Movement")]
    public PlayerControl playerControl;
    public float speed;
    public float currenspeed;
    public float foward;
    public Quaternion rot;
    float turnCalmVelocity;
    [SerializeField] float turnCalmTime = 1f;
    public float speedrotatesprint;
    private float angle;
   
    public void OffRagdoll()
    {

        Player.ins.OffRagdoll();


    }
    public void HandleInput(CharacterControl characterControl)
    {
        Vector3 direction = new Vector3(characterControl.joystick.Horizontal, 0f, characterControl.joystick.Vertical);
        float targetAngle = Mathf.Atan2(direction.normalized.x, direction.z) * Mathf.Rad2Deg +   Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
        Player.ins.animator.SetBool("IsSwing", false);
        if (characterControl.isSprint)
        {
            currenspeed = speed * 2.5f;
            foward = 1.5f;
            rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f)), speedrotatesprint);
            return;
        }
        foward = Mathf.Clamp01(direction.magnitude);
            rot = Quaternion.Euler(0, angle, 0);
       
    }
    public void Move(CharacterControl characterControl)
    {
        HandleInput(characterControl);
        if (!Player.ins.animator.applyRootMotion)
        {
            Player.ins.animator.applyRootMotion = true;
        }
        Player.ins.animator.SetFloat("Forward", foward);
        Player.ins.animator.SetFloat("Turn", characterControl.joystick.Horizontal);
        //Player.ins.characterController.Move((transform.forward * currenspeed + new Vector3(0.0f, _verticalVelocity, 0.0f)) * Time.deltaTime);
        if (characterControl.joystick.Vertical != 0 || characterControl.joystick.Horizontal != 0|| characterControl.isSprint)
        {
            transform.rotation = rot;
        }
       

    }
  
  
}






