using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public PlayerState playerState;
    public CharacterControl characterControl;
    public PlayerMove playerMove;
    public PlayerClimb playerClimb;
    public PlayerSwing playerSwing;
    public PlayerRope playerRope;
    public PlayerAttack playerAttack;
    [Header("On Ground ")]
    public LayerMask surfaceMask;
    public bool onSurface;
    public bool top;
    public bool bot;
    [SerializeField] Transform surfaceCheck;
    [SerializeField] float surfaceDistance = 1.2f;
   
 
    void Update()
    {
        HandleInput();
        CheckOnGround();
    }
    private void FixedUpdate()
    {
        HandleState();
    }
    public void CheckOnGround()
    {
        onSurface = Physics.Raycast(surfaceCheck.position, Vector3.down, surfaceDistance, surfaceMask);
        Player.ins.animator.SetBool("OnGround", onSurface);
    }
    private void HandleState()
    {
        switch (playerState)
        {
            case PlayerState.Move:
                playerMove.Move(characterControl);
                break;
            case PlayerState.Climb:
                playerClimb.Climb(characterControl);
                break;
            case PlayerState.Swing:
                playerSwing.Swing(characterControl);
                break;
            case PlayerState.Rope:
                //npcControl.DoFallAction();
                break;
            case PlayerState.Attack:
                playerAttack.Attack(characterControl.isAttack);
                break;
            //case SelectState.Driver:
            //    npcControl.DoDriver();
            //    break;
            default:
                break;
        }
    }
    public void HandleInput()
    {
       
        if (characterControl.isRope || Input.GetKey(KeyCode.R))
        {
            playerState = PlayerState.Rope;
            return;
        }
        if (characterControl.isSwing || Input.GetKey(KeyCode.LeftControl))
        {
            ////playerSwing.StartSwing();
            //if (playerState == PlayerState.Move)
            //{
            //    playerMove.ChangeToSwing();
            //    //playerState = PlayerState.Swing;
            //}
            playerState = PlayerState.Swing;
            return;
        }
        else
        {

            //playerMove.HandleInput(characterControl);
            if (!onSurface)
            {
                if (playerClimb.CheckNearWall())
                {
                    Player.ins.animator.SetBool("NearWall", true);
                    playerState = PlayerState.Climb;
                    return;
                }
                else
                {
                    Player.ins.animator.SetBool("NearWall", false);
                }
                //if (playerState == PlayerState.Swing)
                //{
                //    playerSwing.ChangeToFall();
                //    return;
                //}
                //playerState = PlayerState.Move;
            }
            else
            {
                if (characterControl.isAttack || Input.GetKey(KeyCode.A))
                {
                    playerState = PlayerState.Attack;
                    return;
                }
                else if (playerState == PlayerState.Attack)
                {
                    playerAttack.FinishActack(0f);
                    playerState = PlayerState.Move;
                    return;
                }
                playerState = PlayerState.Move;
            }
            //if (playerState == PlayerState.Swing)
            //{
            //    playerSwing.ChangeToFall();
            //    return;
            //}
            //else
            //{
            //    playerState = PlayerState.Move;
            //}

            
        }
        
    }
    public void ChangeState(PlayerState _playerState)
    {
        if (playerState == PlayerState.Attack)
        {
            playerAttack.FinishActack(0f);
        }
        //if (playerState == PlayerState.Swing)
        //{
        //    playerSwing.ChangeToFall();
        //    return;
        //}
        playerState = _playerState;
    }
}
public enum PlayerState
{
    Move,
    Climb,
    Swing,
    Rope,
    Fall,
    Attack



}
