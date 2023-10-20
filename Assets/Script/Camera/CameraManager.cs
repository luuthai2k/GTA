using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager ins;
    public GameObject camGround;
    public GameObject camThirdPesonMotorandCar;
    public GameObject camThirdPesonTank;
    public GameObject camThirdPesonHelicopter;
    public GameObject camAir;
    void Awake()
    {
        ins = this;
    }

    public void ChangeCam(int index,Transform target)
    {
        if (index == 0)
        {
            camGround.SetActive(true);
            camThirdPesonMotorandCar.SetActive(false);
            camThirdPesonTank.SetActive(false);
            camThirdPesonHelicopter.SetActive(false);
            camAir.SetActive(false);

        }
        else if (index == 1)
        {
            camThirdPesonMotorandCar.SetActive(true);     
             camGround.SetActive(true);
            camThirdPesonMotorandCar.SetActive(false);
            camThirdPesonTank.SetActive(false);
            camThirdPesonHelicopter.SetActive(false);
            camAir.SetActive(false);
            camThirdPesonMotorandCar.GetComponent<CameraThirdPersonVehicles>().TargetCam(target);
        }
        else if (index == 2)
        {
            camThirdPesonTank.SetActive(true);
            camThirdPesonTank.GetComponent<CameraThirdPersonVehicles>().TargetCam(target);
        }
        else if (index == 3)
        {
            camThirdPesonHelicopter.SetActive(true);
            camThirdPesonHelicopter.GetComponent<CameraThirdPersonVehicles>().TargetCam(target);
        }
        else if (index == 4)
        {
            camThirdPesonHelicopter.SetActive(true);
            camThirdPesonHelicopter.GetComponent<CameraThirdPersonVehicles>().TargetCam(target);
        }
    }
}
