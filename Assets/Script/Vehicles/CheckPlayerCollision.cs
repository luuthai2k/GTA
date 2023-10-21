using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollision : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigidbody;
    public void OnCollisionEnter(Collision collision)
    {
       
        if(collision.gameObject.tag == "Player")
        {
            rigidbody.isKinematic = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigidbody.isKinematic = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {     
            rigidbody.isKinematic = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rigidbody.isKinematic = false;
        }
    }

}
