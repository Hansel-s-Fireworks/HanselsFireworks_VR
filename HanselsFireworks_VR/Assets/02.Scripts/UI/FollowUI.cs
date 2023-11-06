using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine.InputSystem;

public class FollowUI : MonoBehaviour
{
    public float spawnDistance = 0.5f;
    public GameObject menu;
    public InputActionProperty showButton;

    private Transform head;    
    private XROrigin xrOrigin;
    
    // Start is called before the first frame update
    void Start()
    {
        if (xrOrigin == null) xrOrigin = FindObjectOfType<XROrigin>();
        head = xrOrigin.Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            // 메뉴 활성화
            Debug.Log("Menu");
            menu.SetActive(!menu.activeSelf);
            // 플레이어 앞에 띄우기
            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }

        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1;
    }
}
