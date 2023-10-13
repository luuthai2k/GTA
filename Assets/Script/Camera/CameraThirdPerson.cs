using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class CameraThirdPerson : FreeLookCameraControl
{
    public float topheightbase;
    public float midheightbase;
    public float botheightbase;
    public float topradbase;
    public float midradbase;
    public float botradbase;
    private void Start()
    {
        topheightbase = freeLookCam.m_Orbits[2].m_Height;
        midheightbase = freeLookCam.m_Orbits[1].m_Height;
        botheightbase = freeLookCam.m_Orbits[0].m_Height;
        topradbase = freeLookCam.m_Orbits[2].m_Radius;
        midradbase = freeLookCam.m_Orbits[1].m_Radius;
        botradbase = freeLookCam.m_Orbits[0].m_Radius;
    }
   

}
