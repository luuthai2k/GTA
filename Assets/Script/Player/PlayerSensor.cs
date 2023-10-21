using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public static PlayerSensor ins;
    [Header("Vehicles")]
    public GameObject VehiclesCollision;
    public GameObject enemy;
    public LayerMask enemylayerMask;
    public CharacterControl characterControl;
    public PlayerControl playerControl;
    private void Awake()
    {
        ins = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vehicles"))
        {

            Debug.Log("Enter_Car");
            ControlsManager.ins.Control[0].GetComponent<CharacterControl>().getInVehicles.SetActive(true);
            VehiclesCollision = other.gameObject;

        }
        if (enemylayerMask == (enemylayerMask | (1 << other.gameObject.layer)))
        {
            if (enemy == null)
            {
                enemy = other.gameObject;
            }
           
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {      
            characterControl.Swimming();

            Player.ins.animator.SetBool("OnGround", false);
            Player.ins.animator.Play("Swimming");
            //Player.ins.transform.position = new Vector3(Player.ins.transform.position.x,)

            Invoke("DameWater", 0.5f);

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vehicles"))
        {
         
            ControlsManager.ins.Control[0].GetComponent<CharacterControl>().getInVehicles.SetActive(false);
        }
        if (enemylayerMask == (enemylayerMask | (1 << other.gameObject.layer)))
        {

            enemy = null;

        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            characterControl.EndSwimming();
            Player.ins.animator.SetInteger("IsSwimming", 0);
            playerControl.ChangeState(PlayerState.Move);
        }
    }

    public void DameWater()
    {
        Player.ins.playerHP.OnHit(HitDameState.Water, false, 0.25f, Vector3.zero);
        if (Player.ins.playerControl.playerState == PlayerState.Swimming)
        {       
            Invoke("DameWater", Time.deltaTime);
        }
        
    }
}

