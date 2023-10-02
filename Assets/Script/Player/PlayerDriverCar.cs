using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriverCar : MonoBehaviour
{
    public VehiclesControl vehiclesControl;
    public Car car;

    public void GetInCar(Transform enterFormPos)
    {
        car.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        while (Player.ins.gameObject.transform.eulerAngles != car.transform.eulerAngles + new Vector3(0f, 90f, 0f) && Player.ins.gameObject.transform.position != enterFormPos.position)
        {

            Player.ins.gameObject.transform.eulerAngles = car.transform.eulerAngles + new Vector3(0f, 90f, 0f);
            Player.ins.gameObject.transform.position = enterFormPos.position;
        }
        Player.ins.animator.Play("GetCarLeft");
        car.animOpenDoor.SetBool("Open", true);
        car.animOpenDoor.SetBool("Close", false);           
        CameraManager.ins.ChangeCam(1, car.camtarget);
    }
    public void FinishGetInCar()
    {
        car.animOpenDoor.SetBool("Open", false);
        car.animOpenDoor.SetBool("Close", true);
    }
    void Update()
    {


        car.MoveVehicle(vehiclesControl.Vertical,car.maxspeed);
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
