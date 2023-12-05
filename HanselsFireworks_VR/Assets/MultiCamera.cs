using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MultiCamera : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;

    // Start is called before the first frame update
    void Start()
    {
        camera1.clearFlags = CameraClearFlags.SolidColor;
        camera1.backgroundColor = Color.blue;

        camera2.clearFlags = CameraClearFlags.Depth;
        camera2.backgroundColor = Color.clear;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
