using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCControl : MonoBehaviour
{ 
    public static NPCControl ins;
    public NavMeshAgent navMeshAgent;
    public NPCState npcState;
    public ChacractorData chacractorData;
    public LayerMask obstacleMask;
    public GameObject enemy;
    public Animator animator;
    public NPCMovement npcMovement;
    public Transform pointtarget;
    public Transform lastpoint;
    public NPCAttack npcAttack;
    public NPCDriver npcDriver;
    public float timedelay;
    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            npcState.ChangeState(SelectState.Attack);
        }
    }
    public void DoIdleAction()
    {
        npcMovement.canmove = false;
        npcMovement.Idle();
    }

    public void DoMoveAction()
    {
       
        timedelay = 1f;
        npcMovement.canmove = true;
        npcMovement.Move(0.5f);
    }

    public void DoRunAction()
    {
        // Implement running behavior
    }

    public void DoFallAction()
    {
        // Implement falling behavior
    }

    public void DoAttackAction()
    {
        if (enemy == null)
        {
            enemy = Player.ins.gameObject;
        }
        npcAttack.isAttack = true;
        npcMovement.canmove = false;
        npcAttack.Attack(chacractorData.meleeWeapons, chacractorData.rangedWeapons);

    }
    public void DoDriver()
    {
        npcDriver.candriver = true;
        npcDriver.DriverVehicle();
    }
}
