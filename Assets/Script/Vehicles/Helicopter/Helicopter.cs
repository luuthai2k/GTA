using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [Header("HelicopterInfo")]
    public Rigidbody rigidbodyHelicopter;
    public List<Transform> enterFormPos;
    public GameObject helicopter;
    public GameObject driver;
    public Transform driverSitPos;
    public Transform camtarget;
    public Animator animOpenDoor;
    public ShootingHelicopter shootingHelicopter;
    public BladeRotation bladeRotation;
    [Header("Helicopter Engine")]
    public  float enginePower;
    public float EnginePower
    {
        get { return enginePower; }
        set { bladeRotation.BladeSpeed = value*250;
            enginePower = value;
        }
    }
    public float effectiveHeight;
    public float engineLift = 0.0075f;
    public float ForwardFoce;
    public float BackwardFoce;
    public float ForwardtiltFoce;
    public float TurntiltFoce;
    private Vector2 Movement = Vector2.zero;
    private Vector2 Tilting = Vector2.zero;
    public bool onSurface;
    [SerializeField] Transform surfaceCheck;
    [SerializeField] float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    
    void Start()
    {
       
    }

   
    public void CheckOnGround(Joystick joystick, float Vertical)
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (!onSurface)
        {
            Movement.x = joystick.Horizontal;
            Movement.y = joystick.Vertical;
            if (Vertical < 0)
            {
                EnginePower = 1;
                rigidbodyHelicopter.constraints &= ~RigidbodyConstraints.FreezePositionY;

            }
        }      
        if (Vertical > 0)
        {
            EnginePower += engineLift;
            rigidbodyHelicopter.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        else if(Vertical == 0&& !onSurface)
        {
            EnginePower = Mathf.Lerp(EnginePower, 10f, 0.01f);
            if (EnginePower - 10 <= 0.5f && driver != null)
            {
                rigidbodyHelicopter.constraints = RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                rigidbodyHelicopter.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
           

        }
        else if(Vertical <= 0 && onSurface)
        {
            EnginePower = 0;
        }


    }
   
    public void HelicopterHover()
    {
        bladeRotation.BladeRotate();
        float upFoce = EnginePower * rigidbodyHelicopter.mass;
        if(rigidbodyHelicopter.transform.position.y>= effectiveHeight)
        {
            upFoce = 0;

        }
        rigidbodyHelicopter.AddRelativeForce(Vector3.up * upFoce);
    }
    public void HelicopterMovement()
    {
        if (Movement.y > 0)
        {
            rigidbodyHelicopter.AddRelativeForce(Vector3.forward*Mathf.Max(0f,Movement.y*ForwardFoce*rigidbodyHelicopter.mass));
        }
        else if(Movement.y < 0)
        {
            rigidbodyHelicopter.AddRelativeForce(Vector3.back * Mathf.Max(0f, -Movement.y * ForwardFoce * rigidbodyHelicopter.mass));
        }
        if (Movement.x > 0)
        {
            rigidbodyHelicopter.AddRelativeForce(Vector3.right * Mathf.Max(0f, Movement.x * ForwardFoce * rigidbodyHelicopter.mass));
        }
        else if (Movement.x < 0)
        {
            rigidbodyHelicopter.AddRelativeForce(Vector3.left * Mathf.Max(0f, -Movement.x * ForwardFoce * rigidbodyHelicopter.mass));
        }

        if (!onSurface)
        {
            rigidbodyHelicopter.transform.rotation = Quaternion.RotateTowards(rigidbodyHelicopter.transform.rotation, Quaternion.Euler(new Vector3(0f, Camera.main.transform.rotation.eulerAngles.y, 0f)), 50f * Time.deltaTime);
        }
    }
    public void HelicopterTilting()
    {
        Tilting.y = Mathf.Lerp(Tilting.y, Movement.y * ForwardtiltFoce, Time.deltaTime);
        Tilting.x = Mathf.Lerp(Tilting.x, Movement.x* TurntiltFoce, Time.deltaTime);
        rigidbodyHelicopter.transform.localRotation = Quaternion.Euler(Tilting.y, rigidbodyHelicopter.transform.localEulerAngles.y, -Tilting.x);
       
    }
}
