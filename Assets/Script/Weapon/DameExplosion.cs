using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DameExplosion : MonoBehaviour
{
    [SerializeField]
    private int dameExplosion;
    [SerializeField]
    private float powerRagDoll;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Vector3 pos = gameObject.transform.position - other.gameObject.transform.position;

            Player.ins.playerHP.OnHit(HitDameState.Weapon, true, dameExplosion,pos,powerRagDoll);

        }
    }
}
