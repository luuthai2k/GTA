using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHP : MonoBehaviour
{
    public NPCState npcState;
    public GameObject NPC;
    float hit;
    public float hp = 100;

    public void hitDame(float dame)
    {
        if (npcState.currentState != SelectState.Attack)
        {
            npcState.ChangeState(SelectState.Attack);
        }
        hp -= dame;
        if (hp <= 0)
        {
            NPCManager.ins.npcPooling.ReturnPool(NPC, 0.5f);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
