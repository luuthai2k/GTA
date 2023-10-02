using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadLookAt : MonoBehaviour
{

    public MultiAimConstraint multiAimConstraint;
    public Transform player;  
   
    void Update()
    {
        Vector3 cameraForward = Camera.main.transform.forward;    
        Vector3 playerForward = player.forward;
        float dotProduct = Vector3.Dot(cameraForward.normalized, playerForward.normalized);
        if (dotProduct > 0)
        {
            multiAimConstraint.weight = dotProduct + 0.5f;
        }
        else
        {
            multiAimConstraint.weight = dotProduct + 0.5f;
        }

      

    }
}


