using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootLaser : MonoBehaviour
{
    public Transform muzzleTransform;
    public bool canshoot;
    public float startTime;
    public float currentStartTime;
    public float fireRate;
    private GameObject laser;
    private float speedcam;
    public bool isStart;
   
    public void StartShootLaser()
    {
        if (isStart) return;
        isStart = true;
        StartCoroutine(CouroutineCanShoot());
        FreeLookCameraControl.ins.TargetHeading(true, 0.5f, 0.5f);
        speedcam = FreeLookCameraControl.ins._touchSpeedSensitivity;
        FreeLookCameraControl.ins._touchSpeedSensitivity = 0.02f;
        FreeLookCameraControl.ins.cameraWhenShootLaser.SetCamWhenShootLaser();
        Player.ins.animator.SetBool("IsLaser", true);
    }

    IEnumerator CouroutineCanShoot()
    {
        yield return new WaitForSeconds(startTime);
        canshoot = true;
    }
    public void ShootLaser()
    {
        StartShootLaser();
        if (!canshoot) return;
        FreeLookCameraControl.ins.TargetHeading(false);
        Player.ins.playerControl.playerRigControl.ShootLaser();
        Player.ins.transform.rotation = Quaternion.Euler(new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f));
        if (laser == null)
        {
            laser = BulletPooling.ins.GetLaserPool(muzzleTransform.position);
            laser.transform.parent = muzzleTransform;
            laser.transform.localRotation = Quaternion.identity;
        }
        else
        {
            laser.SetActive(true);
        }

    }
    public void FinishShootLaser()
    {
        Player.ins.playerControl.playerRigControl.ReturnShootLaser();
        Player.ins.playerControl.characterControl.isLaser = false;
        FreeLookCameraControl.ins.cameraWhenShootLaser.ReturnCamBase();
        FreeLookCameraControl.ins._touchSpeedSensitivity = speedcam;
        if (laser != null)
        {
            laser.SetActive(false);
        }
        canshoot = false;
        isStart = false;
        Player.ins.animator.SetBool("IsLaser", false);
    }



}




