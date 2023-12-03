using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
public class FollowUI : MonoBehaviour
{
    public Transform head;
    private GameObject XRPlayer;
    // Start is called before the first frame update
    void Start()
    {
        XRPlayer = FindObjectOfType<XROrigin>().gameObject;
        head = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1;
    }
}
