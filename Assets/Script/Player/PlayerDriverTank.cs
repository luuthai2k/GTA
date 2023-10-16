using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriverTank : MonoBehaviour
{
    public VehiclesControl vehiclesControl;
    public Tank tank;
    public void GetInTank()
    {
        //while (Player.ins.player.transform.eulerAngles != tank.transform.eulerAngles && player.transform.position != enterFormPos[side].position)
        //{

        //    Player.ins.player.transform.eulerAngles = tank.transform.eulerAngles;
        //    Player.ins.player.transform.position = enterFormPos[side].position;
        //}
        Player.ins.gameObject.transform.position = tank.driverSitPos.position;
        //Player.ins.player.SetActive(false);
        CameraManager.ins.ChangeCam(2, tank.camtarget);


    }
    private void Update()
    {
        tank.MoveVehicle(vehiclesControl.Vertical);
        tank.HorizontalMove(vehiclesControl.Horizontal);
        tank.UpdateVehicleSteering();
        tank.MoveTurret();
        if (vehiclesControl.Horizontal != 0 && vehiclesControl.Vertical == 0)
        {
            tank.MoveVehicle(0.2f);
        }
    }
    public void GetOutTank()
    {

        Player.ins.gameObject.transform.position = tank.exitPos.position;
        Player.ins.player.SetActive(true);
        Player.ins.ChangeControl(0);
        Player.ins.characterController.enabled = true;
    }
}
