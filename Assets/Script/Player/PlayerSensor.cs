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
            Debug.Log("Enter_Human");

        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vehicles"))
        {
            Debug.Log("Exit_Car");
            ControlsManager.ins.Control[0].GetComponent<CharacterControl>().getInVehicles.SetActive(false);
        }
        if (enemylayerMask == (enemylayerMask | (1 << other.gameObject.layer)))
        {

            enemy = null;


        }
    }
}
