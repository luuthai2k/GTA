using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    [Header("MotorInfo")]
    public GameObject motor;
    public List<Transform> enterFormPos;
    public Rigidbody rigidbodymotor;    
    public GameObject biker;
    public Transform camtarget;
    public float maxspeed;
    public VehicleSensor sensor;
    [Header("Wheels Collider")]
    public WheelCollider frontWheelCollider; 
    public WheelCollider backWheelCollider;

    [Header("Wheels Transform")]
    public Transform frontWheelTransform;
    public Transform backWheelTransform;

    [Header("Wheel Engine")]
    public float accelerationForce = 100f;
    private float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    private float presentTurnAngle = 0f;
    public float wheelsTorque = 20f;
    private Vector2 Movement = Vector2.zero;
    [Header("Vehicle Tilting")]
    private Vector2 Tilting = Vector2.zero;
    public float ForwardtiltFoce;
    public float TurntiltFoce;
    [Header("Vehicle breaking")]
    public float breakingForce = 500f;
    public float breakingForceMax = 1000f;

   

    public void HandleInput(Joystick joystick, float Vertical)
    {


        Movement.x = joystick.Horizontal;
        Movement.y = joystick.Vertical;
        if (Vertical < 0)
        {
         
           

        }

        if (Vertical > 0)
        {
            
        }
        else if (Vertical == 0 )
        {
            


        }
        else if (Vertical <= 0 )
        {
          
        }


    }



    public void VerticalMove(float Vertical,float speed)
    {
       
        float forwardSpeed = transform.InverseTransformDirection(rigidbodymotor.velocity).z;
        //Vertical=Mathf.Clamp(Vertical, -0.4f, 1);
        if (Vertical != 0)
        {
            ApplyBreaks(0);
            if (Vertical * forwardSpeed < 0 && rigidbodymotor.velocity.magnitude >= 1)
            {

                presentAcceleration = 0;
                ApplyBreaks(breakingForceMax);
                

            }
            else
            {
                ApplyBreaks(0);
                if (rigidbodymotor.velocity.magnitude <= speed)
                {
                    presentAcceleration = Vertical * accelerationForce;
                }
                else
                {
                    ApplyBreaks(breakingForceMax);
                    presentAcceleration = 0;
                }
                presentAcceleration = Vertical * accelerationForce;           
                backWheelCollider.brakeTorque = 0;
            }

        }
        else
        {
            ApplyBreaks(breakingForce);
           
            presentAcceleration = 0;
        }

        frontWheelCollider.motorTorque = presentAcceleration;
        backWheelCollider.motorTorque = presentAcceleration;

    }

    public void HorizontalMove(float Horizontal)
    {

        presentTurnAngle = wheelsTorque * Horizontal;
        frontWheelCollider.steerAngle = presentTurnAngle;
        SteeringWheels(frontWheelCollider, frontWheelTransform);
        SteeringWheels(backWheelCollider, backWheelTransform);

    }

    public void ApplyBreaks(float breakingForce)
    {


        frontWheelCollider.brakeTorque = breakingForce;
        backWheelCollider.brakeTorque = breakingForce;
    }
    public void TiltingToMotorcycle(float Vertical, float Horizontal)
    {

        Tilting.y = Mathf.Lerp(Tilting.y, Vertical * ForwardtiltFoce, 5 * Time.deltaTime);
        Tilting.x = Mathf.Lerp(Tilting.x, Horizontal * TurntiltFoce, 5 * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Tilting.y, rigidbodymotor.transform.localEulerAngles.y, -Tilting.x);     
      
    }
    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 pos;
        Quaternion rotation;
        WC.GetWorldPose(out pos, out rotation);
        WT.rotation = rotation;
        WT.position = pos;
    }




}
