using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
   
    public PlayerControl playerControl;
    public float speed;
    public float currenspeed;
    public float foward;
    public Quaternion rot;
    float turnCalmVelocity;
    [SerializeField] float turnCalmTime = 1f;
    public float speedrotatesprint;
    private float angle;

    [SerializeField]
    private PlayerHP playerHP;

    public void HandleInput(CharacterControl characterControl)
    {
        
        Vector3 direction = new Vector3(characterControl.joystick.Horizontal, 0f, characterControl.joystick.Vertical);

        Player.ins.animator.SetBool("OnGround", false);
        if (direction == Vector3.zero)
        {
            Player.ins.animator.SetInteger("IsSwimming", 1);
            playerHP.OnHit(HitDameState.Water, false, 10,Vector3.zero);
        }
        else
        {
            Player.ins.animator.SetInteger("IsSwimming", 2);
            playerHP.OnHit(HitDameState.Water, false, 10,Vector3.zero);
        }
        
        float targetAngle = Mathf.Atan2(direction.normalized.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
   
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
    public void Swim(CharacterControl characterControl)
    {
        
        HandleInput(characterControl);
        if (!Player.ins.animator.applyRootMotion)
        {
            Player.ins.animator.applyRootMotion = true;
        }
       

        //Player.ins.characterController.Move((transform.forward * currenspeed + new Vector3(0.0f, _verticalVelocity, 0.0f)) * Time.deltaTime);
        if (characterControl.joystick.Vertical != 0 || characterControl.joystick.Horizontal != 0 || characterControl.isSprint)
        {
            transform.rotation = rot;
        }


    }


    public void OnGround()
    {
        Player.ins.animator.SetInteger("IsSwimming", 0);
    }


}
