using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriverMotor : MonoBehaviour
{
    public VehiclesControlWithJoystick  vehiclesControlWithJoystick;
    public Motor  motor;
    public void GetInMotor(Transform enterFormPos,int side)
    {
        while (Player.ins.gameObject.transform.eulerAngles != motor.transform.eulerAngles && Player.ins.gameObject.transform.position != enterFormPos.position)
        {

            Player.ins.gameObject.transform.eulerAngles = motor.transform.eulerAngles;
            Player.ins.gameObject.transform.position = enterFormPos.position;
        }
        CameraManager.ins.ChangeCam(1, motor.GetComponent<Motor>().camtarget);

        if (side == 0)
        {
            Player.ins.animator.Play("GetInMotorBikeRight");
        }
        if (side == 1)
        {

            Player.ins.animator.Play("GetInMotorBikeLeft");
        }
    }
    void Update()
    {
        motor.VerticalMove(vehiclesControlWithJoystick.Vertical,15f);
        Debug.Log(vehiclesControlWithJoystick.Vertical);
        motor.HorizontalMove(vehiclesControlWithJoystick.joystick.Horizontal);
        motor.TiltingToMotorcycle(vehiclesControlWithJoystick.joystick.Vertical, vehiclesControlWithJoystick.joystick.Horizontal);
    }
    public void GetOutMotor()
    {
        Player.ins.animator.SetTrigger("GetOut");
        Player.ins.ChangeControl(0);
        Player.ins.characterController.enabled = true;
    }
}
