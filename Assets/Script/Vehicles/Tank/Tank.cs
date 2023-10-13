using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("TankInfo")]
    public List<Transform> enterFormPos;
    public Rigidbody rigidbodytank;
    public GameObject tank;
    public GameObject driver;
    public Transform driverSitPos;
    public Transform exitPos;
    public Transform camtarget;
    [Header("Tank Chain")]
    public float offset = 0f;
    public float offsetValue = 0f;
    public Renderer trackLeft;
    public Renderer trackRight;
    [Header("Wheels Collider")]
    public List<WheelCollider> LeftWheelCollider;
    public List<WheelCollider> RightWheelCollider;
    [Header("Wheels Transform")]
    public List<Transform> LeftWheelColliderTransform;
    public List<Transform> RightWheelColliderTransform;
    public List<Transform> LeftUselessGrearTransform;
    public List<Transform> RightUselessGrearTransform;
    [Header("Chain Bone")]
    [SerializeField] private Transform[] LeftTrackBones;
    [SerializeField] private Transform[] RightTrackBones;
    [Header("Wheel Engine")]
    public float accelerationForce;
    private float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    public float trackThiccness = 0.1f;
    public float wheelsTorque = 20f;
    [Header("Vehicle breaking")]
    public float breakingForce = 500f;

    [Header("TankTurret")]
    public GameObject mainGun;
    public GameObject barrel;
    public int maxGunAngle_elevation = 35;
    public int minGunAngle_depression = 8;
    public Transform target;
    public float speed;



    public void MoveVehicle(float Vertical)
    {
       
        float forwardSpeed = transform.InverseTransformDirection(rigidbodytank.velocity).z;
        ChainSteering(forwardSpeed);
        if (Vertical != 0)
        {
            Debug.Log("move");
            for (int i = 0; i < LeftWheelCollider.Count; i++)
            {
                LeftWheelCollider[i].brakeTorque = 0;
                RightWheelCollider[i].brakeTorque = 0;
            }
            if (Vertical * forwardSpeed < 0)
            {

                presentAcceleration = Vertical * accelerationForce * 5;


            }
            else
            {
                presentAcceleration = Vertical * accelerationForce;
            }
           

        }
        else
        {
            presentAcceleration = 0;
            for (int i = 0; i < LeftWheelCollider.Count; i++)
            {
                LeftWheelCollider[i].brakeTorque = breakingForce;
                RightWheelCollider[i].brakeTorque = breakingForce;
            }
           
        }

        for (int i = 0; i < LeftWheelCollider.Count; i++)
        {          
            LeftWheelCollider[i].motorTorque = presentAcceleration;
            RightWheelCollider[i].motorTorque = presentAcceleration;
        }
      

    }
    
    public void ChainSteering(float forwardSpeed)
    {
        if (forwardSpeed > 0)
        {
            offset +=  rigidbodytank.velocity.magnitude/ offsetValue;
        }
        if (forwardSpeed < 0)
        {
            offset -= rigidbodytank.velocity.magnitude/ offsetValue;
        }
        trackLeft.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        trackRight.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
    public void HorizontalMove(float Horizontal)
    {
        if (Horizontal != 0)
        {
            Quaternion newRotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y + Horizontal * wheelsTorque, transform.rotation.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, wheelsTorque * Time.fixedDeltaTime);
        }
       

    }
   

    public void UpdateVehicleSteering()
    {
       
        for(int i=0;i< LeftWheelCollider.Count; i++)
        {
            SteeringWheels(LeftWheelCollider[i], LeftWheelColliderTransform[i], LeftTrackBones[i]);
            SteeringWheels(RightWheelCollider[i], RightWheelColliderTransform[i], RightTrackBones[i]);
            float speed = LeftWheelCollider[i].attachedRigidbody.velocity.magnitude;

        
        }
        SteeringUselessGrear(LeftWheelCollider[0], LeftUselessGrearTransform[0]);
        SteeringUselessGrear(LeftWheelCollider[5], LeftUselessGrearTransform[1]);
        SteeringUselessGrear(RightWheelCollider[0], RightUselessGrearTransform[0]);
        SteeringUselessGrear(RightWheelCollider[5], RightUselessGrearTransform[1]);


    }
    void SteeringWheels(WheelCollider WC, Transform WT, Transform TrackBones)
    {
        Vector3 pos;
        Quaternion rotation;
        WC.GetWorldPose(out pos, out rotation);
        WT.rotation = rotation;
        WT.position = pos + new Vector3(0, trackThiccness, 0);
        TrackBones.position = WT.position + transform.up * -1f * (WC.radius+0.1f);
    }
    void SteeringUselessGrear(WheelCollider WC, Transform WT)
    {    
        Quaternion rotation;
        WC.GetWorldPose(out _, out rotation);
        WT.rotation = rotation;
      

    }

    public void MoveTurret()
    {


        mainGun.transform.rotation = Quaternion.RotateTowards(mainGun.transform.rotation, Quaternion.Euler(new Vector3(0f, Camera.main.transform.rotation.eulerAngles.y, 0f)), speed * Time.deltaTime);


        Vector3 directionToTarget = target.position - barrel.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        float targetXAngle = targetRotation.eulerAngles.x;

        if (targetXAngle > 180f)
        {
            targetXAngle -= 360f;
        }
        float clampedXAngle = Mathf.Clamp(targetXAngle, maxGunAngle_elevation, minGunAngle_depression);
        Quaternion newrotate= Quaternion.RotateTowards(barrel.transform.localRotation, Quaternion.Euler(clampedXAngle, 0f, 0f), speed * Time.deltaTime);
        barrel.transform.localRotation = newrotate;


        Debug.Log(clampedXAngle);


    }
}
