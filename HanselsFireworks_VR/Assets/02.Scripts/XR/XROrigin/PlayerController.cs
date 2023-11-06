using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool onPortal;
    private Vector3 previousControllerPosition;
    public float throwForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TutorialPortal"))
        {
            onPortal = true;
        }
    }
}
