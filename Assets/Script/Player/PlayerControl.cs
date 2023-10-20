using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public PlayerState playerState;
    public PlayerRigControl playerRigControl;
    public CharacterControl characterControl;
    public PlayerMove playerMove;
    public PlayerClimb playerClimb;
    public PlayerSwing playerSwing;
    public PlayerRope playerRope;
    public PlayerAttack playerAttack;
    public PlayerShootRoket playerShootRoket;
    public PlayerShootLaser playerShootLaser;
    public PlayerSwim playerSwim;
    [Header("On Ground ")]
    public bool onSurface;
    public bool top;
    public bool bot;
    [SerializeField] Transform surfaceCheck;
    [SerializeField] float surfaceDistance = 1.2f;
    public bool isSwing;
    private float timedelay;



    void Update()
    {
        if (playerState != PlayerState.Dead)
        {
            HandleInput();
            if (playerState != PlayerState.Swimming)
            {
                CheckOnGround();
            }
        }

        HandleState();
    }
    public void CheckOnGround()
    {
        onSurface = Physics.Raycast(surfaceCheck.position, Vector3.down, surfaceDistance, GameManager.ins.layerData.Surface);
        Player.ins.animator.SetBool("OnGround", onSurface);
        if (onSurface)
        {
            playerRigControl.HeadLookAt();
        }
        else
        {
            playerRigControl.ReturnHeadLookAt();
            if (!characterControl.isLaser) return;
            characterControl.isLaser = false;
        }
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
                playerRope.Rope(characterControl);
                break;
            case PlayerState.Attack:
                playerAttack.Attack(characterControl.isAttack);
                break;
            case PlayerState.Rocket:
                playerShootRoket.ShootRoket();
                break;
            case PlayerState.Laser:
                playerShootLaser.ShootLaser();
                break;
            case PlayerState.Swimming:
                playerSwim.Swim(characterControl);
                break;
            case PlayerState.Dead:

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
            
            ChangeState(PlayerState.Rope);
            return;
        }
        if (characterControl.isSwing || Input.GetKey(KeyCode.LeftControl))
        {

            if (playerClimb.CheckNearWall() && !onSurface)
            {
                ChangeState(PlayerState.Climb);
                return;
            }
            if (playerState == PlayerState.Climb)
            {
                timedelay += Time.deltaTime;
                if (timedelay >= 0.5f)
                {
                    timedelay = 0;
                    ChangeState(PlayerState.Swing);
                    isSwing = true;

                }
                return;
            }
            ChangeState(PlayerState.Swing);
            isSwing = true;
            return;
        }
        else if (characterControl.isSwimming )
        {
            playerState = PlayerState.Swimming;
        }
        else
        {
            isSwing = false;

            if (!onSurface)
            {
                if (playerState == PlayerState.Climb) return;
                if (playerClimb.CheckNearWall())
                {
                    ChangeState(PlayerState.Climb);
                    return;
                }
            }
            else
            {
                if (characterControl.isAttack || Input.GetKey(KeyCode.A))
                {
                    ChangeState(PlayerState.Attack);
                    return;
                }
                if (characterControl.isRocket || Input.GetKey(KeyCode.N))
                {
                    ChangeState(PlayerState.Rocket);
                    return;
                }
                if (characterControl.isLaser || Input.GetKey(KeyCode.L))
                {
                    ChangeState(PlayerState.Laser);
                    return;
                }
                ChangeState(PlayerState.Move);

            }

        }

    }
   
    public void ChangeState(PlayerState _playerState)
    {
        if (playerState == _playerState) return;
        FinishState();
         if (_playerState == PlayerState.Laser)
        {
            //FreeLookCameraControl.ins.TargetHeading(true);
        }
        else if (_playerState == PlayerState.Swing)
        {
            FreeLookCameraControl.ins.TargetHeading(true, 2);
        }
        else if (_playerState == PlayerState.Climb)
        {
            Player.ins.animator.SetBool("NearWall", true);
        }
        else if (_playerState == PlayerState.Rocket)
        {
            //if (playerShootRoket.isShoot) return;
        }
        else if (_playerState == PlayerState.Move)
        {
            FreeLookCameraControl.ins.TargetHeading(false);
        }
        else if (_playerState == PlayerState.Rope)
        {
            playerRope.PrepareShoot();
        }

        playerState = _playerState;
    }
    public void FinishState()
    {
        if (playerState == PlayerState.Attack)
        {
            playerAttack.FinishActack(0f);
        }
        else if (playerState == PlayerState.Laser)
        {
            playerShootLaser.FinishShootLaser();
           
        }
        else if (playerState == PlayerState.Swing)
        {
            playerSwing.FinishSwing(0);
            playerSwing.fall = 1;
            Player.ins.animator.SetBool("IsSwing", false);
        }
        else if (playerState == PlayerState.Climb)
        {
            return;

        }
        else if (playerState == PlayerState.Rocket)
        {
            if (playerShootRoket.isShoot) return;
        }
        else if (playerState == PlayerState.Rope)
        {
           playerRope.FinishRope();
        }

    }
}
public enum PlayerState
{
    Move,
    Climb,
    Swing,
    Rope,
    Fall,
    Attack,
    Rocket,
    Laser,
    Swimming,
    Dead

}
