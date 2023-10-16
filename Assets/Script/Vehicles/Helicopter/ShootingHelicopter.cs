using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHelicopter : MonoBehaviour
{
    public float power;
    [Header("Shooting Bullet")]
    public GameObject bulleteffect;
    public Transform launcherLeft;
    public Transform launcherRight;
    public ParticleSystem flashL;
    public ParticleSystem flashR;  
    public float fireRateBullet;
    private float fireRatebullet = 0;
    [Header("Shooting Rocket")]
    public GameObject rocket;
    public Transform launcherMid;
    public float fireRateRocket;
   
    private float fireRaterocket = 0;
    void Start()
    {
        
    }

    public void ShootingRocket(bool isshooting)
    {
        if (fireRaterocket > 0)
        {
            fireRaterocket -= Time.deltaTime;
            return;
        }
        if (fireRaterocket <= 0 && isshooting)
        {
            fireRaterocket = fireRateRocket;
            Vector3 direction = PointCenterSceenToWorld.ins.targetTransform.position - launcherRight.position;
            var Rocket = Instantiate(rocket, launcherMid.position, Quaternion.LookRotation(direction));
            Rocket.GetComponent<Rigidbody>().AddForce(direction * power);
            
        }
    }
    public void ShootingBullet(bool isshooting)
    {
        if (fireRatebullet > 0)
        {
            fireRatebullet -= Time.deltaTime;
            return;
        }
        if (fireRatebullet <= 0&& isshooting)
        {
            fireRatebullet = fireRateBullet ;
            flashL.Emit(1);
            flashR.Emit(1);
            var bulletLeft = Instantiate(bulleteffect, launcherLeft.position, Quaternion.identity);
            var bulletRight = Instantiate(bulleteffect, launcherRight.position, Quaternion.identity);
            Vector3 directionLeft = PointCenterSceenToWorld.ins.targetTransform.position - launcherLeft.position;
            bulletLeft.GetComponent<Rigidbody>().AddForce(directionLeft * power);
            Vector3 directionRight = PointCenterSceenToWorld.ins.targetTransform.position - launcherRight.position;
            bulletRight.GetComponent<Rigidbody>().AddForce(directionRight * power);
            Debug.Log(fireRatebullet);
        }
        
       
    }
}
