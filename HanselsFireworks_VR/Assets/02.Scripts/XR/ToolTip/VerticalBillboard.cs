using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class VerticalBillboard : MonoBehaviour
{    
    private Transform head;
    private XROrigin xrOrigin;    
    // public Transform target;    

    private void Start()
    {
        if (xrOrigin == null) xrOrigin = FindObjectOfType<XROrigin>();
        head = xrOrigin.Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        // menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        // menu.transform.forward *= -1;
        transform.LookAt(head, Vector3.up);
    }
}
