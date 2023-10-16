using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingSenSor : MonoBehaviour
{
    [SerializeField] Transform pos;
   
    public bool topRayCast;
    public bool botRayCast;
    public bool leftRayCast;
    public bool rightRayCast;
    public float h_distance;
    public float v_distance;
    public float raydistance;
    public float distance;
    public LayerMask wallMask;
   
    private void Update()
    {
        topRayCast=CheckTop();
        botRayCast=CheckBot();
    }
    public bool CheckTop()
    {
        Ray ray = new Ray(pos.position+Vector3.up*h_distance, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * raydistance, Color.green);
        if (Physics.Raycast(ray, raydistance, wallMask))
        {
            return true;
        }
        else return false;
    }
    public bool CheckBot()
    {
        Ray ray = new Ray(pos.position+Vector3.down*h_distance, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * raydistance, Color.green);
        if (Physics.Raycast(ray, raydistance, wallMask))
        {
            return true;
        }
        else return false;
    }
    public bool CheckLeft()
    {
        Vector3 origin = pos.position -transform.right*v_distance;
        Vector3 direction =( pos.position+ transform.forward*distance)-origin;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * raydistance, Color.green);
        if (Physics.Raycast(ray, raydistance, wallMask))
        {
            return true;
        }
        else return false;
    }
    public bool CheckRight()
    {
        Vector3 origin = pos.position + transform.right * v_distance;
        Vector3 direction = (pos.position + transform.forward*distance) - origin;
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * raydistance, Color.green);
        if (Physics.Raycast(ray, raydistance, wallMask))
        {
            return true;
        }
        else return false;
    }

}
