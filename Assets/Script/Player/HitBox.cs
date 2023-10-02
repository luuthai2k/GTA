using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public LayerMask enemylayerMask;
    public PlayerAttack playerAttack;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enemylayerMask == (enemylayerMask | (1 << other.gameObject.layer)))
        {
            playerAttack.ishit = true;
            if (other.GetComponent<NPCHP>() != null)
            {
                playerAttack.OffHitBox();
                other.GetComponent<NPCHP>().hitDame(10);
                Debug.Log("hit");
            }
           
        }
    }
}
