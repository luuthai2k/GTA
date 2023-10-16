using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriverHelicopter : MonoBehaviour
{

    public HelicopterControl helicopterControl;
    public Helicopter helicopter;
    public void GetInHelicopter(Transform enterFormPos,int side)
    {
        if (side == 0)
        {
            while (Player.ins.gameObject.transform.eulerAngles != helicopter.transform.eulerAngles + new Vector3(0f, 90f, 0f) && Player.ins.gameObject.transform.position != enterFormPos.position)
            {

                Player.ins.gameObject.transform.eulerAngles = helicopter.transform.eulerAngles + new Vector3(0f, 90f, 0f);
                Player.ins.gameObject.transform.position = enterFormPos.position;
            }
            Player.ins.animator.Play("GetInHelicopterLeft");
        }
        if (side == 1)
        {

            while (Player.ins.gameObject.transform.eulerAngles != helicopter.transform.eulerAngles + new Vector3(0f, -90f, 0f) && Player.ins.gameObject.transform.position != enterFormPos.position)
            {

                Player.ins.gameObject.transform.eulerAngles = helicopter.transform.eulerAngles + new Vector3(0f, -90f, 0f);
                Player.ins.gameObject.transform.position = enterFormPos.position;
            }
            Player.ins.animator.Play("GetInHelicopterRight");
        }
        helicopter.driver = Player.ins.player;
        CameraManager.ins.ChangeCam(3, helicopter.camtarget);
    }
    public void OpenDoorHelicopter()
    {
        helicopter.animOpenDoor.SetBool("Open", true);
        helicopter.animOpenDoor.SetBool("Close", false);
    }
    public void FinishGetInHelicopter()
    {
        //Transform Playertransform = Player.ins.player.transform;
        //transform.position = Playertransform.position;
        //Player.ins.player.transform.localPosition= Vector3.zero;
        helicopter.animOpenDoor.SetBool("Open", false);
        helicopter.animOpenDoor.SetBool("Close", true);
    }

    private void Update()
    {
        helicopter.CheckOnGround(helicopterControl.joystick, helicopterControl.Vertical);
        helicopter.shootingHelicopter.ShootingBullet(helicopterControl.shootingBullet);
        helicopter.shootingHelicopter.ShootingRocket(helicopterControl.shootingRocket);
    }
    private void FixedUpdate()
    {
        helicopter.HelicopterHover();
        helicopter.HelicopterMovement();
        helicopter.HelicopterTilting();
    }
    public void GetOutHelicopter()
    {
        helicopter.driver = null;
        helicopter.animOpenDoor.SetBool("Open", true);
        helicopter.animOpenDoor.SetBool("Close", false);
        if (helicopter.onSurface)
        {
            Player.ins.animator.SetTrigger("GetOut");

        }
        else
        {
            Player.ins.animator.Play("JumpOutFromHelicopter");
        }
           
    }
    public void FinishGetOutHelicopter()
    {
        Player.ins.ChangeControl(0);
        helicopter.animOpenDoor.SetBool("Open", false);
        helicopter.animOpenDoor.SetBool("Close", true);
        Player.ins.characterController.enabled = true;

    }
}
