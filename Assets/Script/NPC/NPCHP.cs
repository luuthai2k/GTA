using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHP : MonoBehaviour
{
    public NPCState npcState;
    public GameObject NPC;
    public NPCControl npcControl;

    public float hp = 100;

    public bool isPolice;

    public bool isNpcQuest;

    public bool isdead;

    public int bonus,wanterPoint;

    [SerializeField]
    private GameObject ragdollNPC,ragdollNPCNow;

    [SerializeField]
    private Rigidbody rb;

    public void HitDame(int dame, Vector3 pos, float powerRagdoll = 0,bool isRagdoll = false)
    {
        if (hp >= 0)
        {
            if (npcState.currentState != SelectState.Attack)
            {
                npcState.ChangeState(SelectState.Attack);
            }
            hp -= dame;
            
        }
        else if(isdead == false)
        {
            npcControl.animator.enabled = false;

            PoliceStarManager.ins.ChangeWanterPoint(wanterPoint);
            UpgradeXpManager.ins.CheckLevel(Random.Range(150, 250));

            OnRagdoll(Vector3.zero, powerRagdoll);
            Invoke("NPCDead", 2.5f);
            isdead = true;
        }
        if (isRagdoll)
        {
            OnRagdoll(Vector3.zero, powerRagdoll);
        }
    }
    public void NPCDead()
    {
        if (isPolice)
        {
            NPCManager.ins.npcPooling.ResetNPCStar();
        }
        else
        {
            NPCManager.ins.npcPooling.ReturnPool(NPC, 0.5f);
        }

        float x = Random.Range(transform.position.x - 1, transform.position.x + 1);
        float z = Random.Range(transform.position.z - 1, transform.position.z + 1);

        Vector3 pos = new Vector3(x, transform.position.y, z);

        NPCManager.ins.npcPooling.SpawnPickUp(pos);
        NPCManager.ins.npcPooling.SpawnPickUp(pos,true);
    }


    public void OnRagdoll(Vector3 pos, float power = 0)
    {
        NPC.SetActive(false);

        ragdollNPCNow = Instantiate(ragdollNPC, transform.position, transform.rotation);
        gameObject.GetComponent<CharacterController>().enabled = false;

        ragdollNPCNow.GetComponent<BodyPhysicsController>().Fall();

        ragdollNPCNow.GetComponent<Rigidbody>().AddForce(pos * power);

        Invoke("EndRagdoll", 1.25f);
    }


    public void EndRagdoll()
    {
        transform.position = new Vector3(ragdollNPCNow.transform.position.x, transform.position.y, ragdollNPCNow.transform.position.z);

        NPC.SetActive(true);

        gameObject.GetComponent<CharacterController>().enabled = true;
        Destroy(ragdollNPCNow);
    }

}
