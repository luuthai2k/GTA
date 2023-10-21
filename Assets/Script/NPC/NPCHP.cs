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

    public void HitDame(float dame, Vector3 pos, float powerRagdoll = 0,bool isRagdoll = false)
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
            

            OnRagdoll(Vector3.zero, powerRagdoll,true);
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


    public void OnRagdoll(Vector3 pos, float power = 0, bool isDie = false)
    {

        NPC.SetActive(false);


        ragdollNPCNow = Instantiate(ragdollNPC, transform.parent.position, transform.parent.rotation);

        //if (gameObject.GetComponent<CharacterController>() != null)
        //{
        //    gameObject.GetComponent<CharacterController>().enabled = false;
        //}

        ragdollNPCNow.GetComponent<RagDoll>().OnRagDoll(transform.parent.gameObject, pos * power,isDie);
    }


}
