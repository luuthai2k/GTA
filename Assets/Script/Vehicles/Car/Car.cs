using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    [Header("CarInfo")]
    public List<Transform> enterFormPos;
    public Rigidbody rigidbodycar;
    public GameObject car;
    public GameObject driver;
    public Transform driverSit;
    public Animator animOpenDoor;
    public Transform camtarget;
    public VehicleSensor sensor;
    [Header("Wheels Collider")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transform")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;

    [Header("Wheel Engine")]
    public float maxspeed;
    public float accelerationForce ;
    private float presentAcceleration = 0f;

    [Header("Vehicle Steering")]  
    private float presentTurnAngle = 0f;
    public float wheelsTorque = 20f;
    [Header("Vehicle breaking")]
    public float breakingForce = 500f;
    public float breakingForceMax = 1000f;

    public NPCControl npcDrive;
    public NavMeshObstacle carObstacle;

    public void MoveVehicle(float Vertical,float speed)
    {
        rigidbodycar.centerOfMass = Vector3.zero;
        float forwardSpeed = transform.InverseTransformDirection(rigidbodycar.velocity).z;     
        if (Vertical != 0)
        {
            ApplyBreaks(0);
            if (Vertical * forwardSpeed < 0 &&rigidbodycar.velocity.magnitude >= 1)
            {

                ApplyBreaks(breakingForceMax);
                presentAcceleration = 0;

            }
            else
            {
                ApplyBreaks(0);
                if (rigidbodycar.velocity.magnitude <= speed)
                {
                    presentAcceleration = Vertical * accelerationForce;
                }
                else
                {
                    ApplyBreaks(breakingForceMax);
                    presentAcceleration = 0;
                }
              
            }

        }
        else
        {
            ApplyBreaks(breakingForce);
            presentAcceleration = 0;
        }
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
       
    }
    public void UpdateVehicleSteering()
    {
        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);

    }
    public void VehicleSteering(float Horizontal)
    {
       
        presentTurnAngle = Horizontal * wheelsTorque;
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
       

    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 pos;
        Quaternion rotation;
        WC.GetWorldPose(out pos, out rotation);
        WT.rotation = rotation;
        WT.position=pos;
    }

    public void ApplyBreaks(float breakingForce)
    {
        
        frontRightWheelCollider.brakeTorque = breakingForce;
        frontLeftWheelCollider.brakeTorque = breakingForce;
        backRightWheelCollider.brakeTorque = breakingForce;
        backLeftWheelCollider.brakeTorque = breakingForce;
    }

    


}
