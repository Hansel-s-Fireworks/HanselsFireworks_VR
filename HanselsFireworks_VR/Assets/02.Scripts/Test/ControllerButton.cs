using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerButton : MonoBehaviour
{
    public GameObject airballoon;
    public GameObject BallPrefab;
    public Transform XROrigin; 
    public Transform spawnspot;
    public InputActionProperty PressX;
    public InputActionProperty PressY;
    public InputActionProperty PressA;
    public InputActionProperty PressB;

    void Update()
    {
        if (PressX.action.WasPressedThisFrame())
        {
            Debug.Log("Pressed X");
        }

        if (PressY.action.WasPressedThisFrame())
        {
            Debug.Log("Pressed Y");
        }

        if (PressA.action.WasPressedThisFrame())
        {
            Debug.Log("Pressed A");
        }

        if (PressB.action.WasPressedThisFrame())
        {
            Debug.Log("Pressed B");
        }
    }
}
