using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineMixingCamera mixingCamera;
    public float step;
    public bool switchToFPS, switchToADS;

    void FixedUpdate()
    {
        if(switchToFPS)
        {
            switchToADS = false;
            mixingCamera.m_Weight0 += step;
            mixingCamera.m_Weight1 -= step;
            if (mixingCamera.m_Weight0 >= 1)
            {
                switchToFPS = false;
                mixingCamera.m_Weight1 = 0;
                mixingCamera.m_Weight0 = 1;
            }
        }
        if (switchToADS)
        {
            switchToFPS = false;
            mixingCamera.m_Weight0 -= step;
            mixingCamera.m_Weight1 += step;
            if(mixingCamera.m_Weight1 >= 1)
            {
                switchToADS = false;
                mixingCamera.m_Weight1 = 1;
                mixingCamera.m_Weight0 = 0;
            }
        }
    }
}
