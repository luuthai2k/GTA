using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraThirdPersonVehicles : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachinePOV aimPOV;
    public float verticalAxisValue;
    public float horizontalAxisValue;
    

    private void OnEnable()
    {
        aimPOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        aimPOV.m_VerticalAxis.Value= verticalAxisValue;
        aimPOV.m_HorizontalAxis.Value = horizontalAxisValue;
       
    }
   public void TargetCam(Transform target)
    {
        virtualCamera.Follow= target;
        virtualCamera.LookAt = target;

    }
}
