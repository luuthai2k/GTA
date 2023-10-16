using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerDriverCar : MonoBehaviour
{
    public VehiclesControl vehiclesControl;
    public Car car;

    public Transform enterFormPos;

    private Vector3 positionPos;

    public void GetInCar(Transform _enterFormPos)
    {
  
        enterFormPos = _enterFormPos;
        car.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        CheckPositionInCar();
        positionPos = Player.ins.gameObject.transform.position;
       
    }

    public void CheckPositionInCar()
    {
        while (Player.ins.gameObject.transform.eulerAngles != car.transform.eulerAngles + new Vector3(0f, 90f, 0f) && Player.ins.gameObject.transform.position != enterFormPos.position)
        {
           Player.ins.gameObject.transform.eulerAngles = car.transform.eulerAngles + new Vector3(0f, 90f, 0f);
           Player.ins.gameObject.transform.position = enterFormPos.position;
            
        }

        if(Mathf.Abs(Player.ins.gameObject.transform.position.x - enterFormPos.position.x) <= 0.2f && Mathf.Abs(Player.ins.gameObject.transform.position.z - enterFormPos.position.z) <= 0.2f)
        {
            
            if (PlayerSensor.ins.VehiclesCollision.GetComponent<Car>().npcDrive != null)
            {

                NPCControl npc = PlayerSensor.ins.VehiclesCollision.GetComponent<Car>().npcDrive;

                npc.gameObject.transform.parent = null;

                npc.animator.SetBool("DeadOutVehicle", true);

                //npc.npcState.ChangeState(SelectState.Attack);

                npc.npcDriver.GetOutVehicle();


            }
            Player.ins.animator.Play("GetCarLeft");
            car.animOpenDoor.SetBool("Open", true);
            car.animOpenDoor.SetBool("Close", false);
            CameraManager.ins.ChangeCam(1, car.camtarget);
            Player.ins.animator.SetBool("IsWalkInCar", false);

              
        }

    }
    public void FinishGetInCar()
    {
        car.animOpenDoor.SetBool("Open", false);
        car.animOpenDoor.SetBool("Close", true);

    }
    void Update()
    {
        car.MoveVehicle(vehiclesControl.Vertical, car.maxspeed);
        car.VehicleSteering(vehiclesControl.Horizontal);
        car.UpdateVehicleSteering();

        if (Input.GetKeyDown(KeyCode.A))
        {
            car.VehicleSteering(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            car.VehicleSteering(1);
        }
    }
    public void GetOutCar()
    {
        car.animOpenDoor.SetBool("Open", true);
        car.animOpenDoor.SetBool("Close", false);
        Player.ins.animator.SetTrigger("GetOut");
    }
    public void FinishGetOutCar()
    {
        Player.ins.ChangeControl(0);
        car.animOpenDoor.SetBool("Open", false);
        car.animOpenDoor.SetBool("Close", true);
        Player.ins.characterController.enabled = true;
    }
}
