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
        Vector3 pos = gameObject.transform.position - other.gameObject.transform.position;
        if (other.gameObject.layer == 9)
        {

            Player.ins.playerHP.OnHit(HitDameState.Weapon, true, dameExplosion,pos,powerRagDoll);

        }
        if(other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<VehiclesHp>().DameVehicles(dameExplosion);
        }

        if(other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<NPCControl>().npcHp.HitDame(dameExplosion, pos,powerRagDoll,true);
        }
    }
}
