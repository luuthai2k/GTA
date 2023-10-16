using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShoot : MonoBehaviour
{
    public Rigidbody rigibody;
    public ParticleSystem FX;
    public float dame;
    public GameObject bullet;
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.tag != "Helicopter")
        {
            FX.Play();
            //gameObject.SetActive(false);
            rigibody.isKinematic = true;
            Invoke("EndEffect", 1f);
        }

       
    }

    private void EndEffect()
    {
        gameObject.SetActive(false);
    }
   
}
