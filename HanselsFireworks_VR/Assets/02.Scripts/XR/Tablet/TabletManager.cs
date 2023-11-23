using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
public class TabletManager : MonoBehaviour
{
    private GameObject XRPlayer;
    private XRBaseInteractable interactable;
    private Vector3 originalScale;
    private CapsuleCollider capsuleCollider;
    
    public GameObject pocket;
    public GameObject menu;
    public XRDirectInteractor rightHand;
    public XRDirectInteractor leftHand;
    public List<GameObject> panels = new List<GameObject>();
    
    
    
    
    private void Start()
    {
        XRPlayer = FindObjectOfType<XROrigin>().gameObject;
        capsuleCollider = GetComponent<CapsuleCollider>();
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        
        originalScale = transform.localScale;           // 원래 스케일 저장
        
        grabbable.selectEntered.AddListener(ShowUI);
        grabbable.selectEntered.AddListener(setoffOppositeHand);
        grabbable.selectExited.AddListener(HideUI);
    }
    public void ShowUI(SelectEnterEventArgs arg)
    {
        // isGrab = !isGrab;
        menu.SetActive(true);
        capsuleCollider.isTrigger = true;
        transform.SetParent(null);
        transform.localScale = originalScale; // 원래 스케일로 설정
    }
    public void setoffOppositeHand(SelectEnterEventArgs arg)
    {
        // 왼손이 잡으면 오른손의 XRDirect컴포넌트를 false로 한다. 
        if (arg.interactorObject.transform.CompareTag("Left Hand"))
        {
            // 오른손의 XRDirect컴포넌트를 false로 한다. 
            rightHand.enabled = false;
        }
        if (arg.interactorObject.transform.CompareTag("Right Hand"))
        {
            leftHand.enabled = false;
        }
    }
    
    public void HideUI(SelectExitEventArgs arg)
    {
        // isGrab = !isGrab;
        menu.SetActive(false);
        capsuleCollider.isTrigger = false;
        transform.position = pocket.transform.position;
        transform.rotation = pocket.transform.rotation;
        transform.SetParent(pocket.transform);
        rightHand.enabled = true;
        leftHand.enabled = true;
    }
    public void setOnOpositeHand(SelectExitEventArgs arg)
    {
        // 왼손이 잡으면 오른손의 XRDirect컴포넌트를 false로 한다. 
        if (arg.interactorObject.transform.CompareTag("Left Hand"))
        {
            // 오른손의 XRDirect컴포넌트를 false로 한다. 
            rightHand.enabled = true;
        }
        if (arg.interactorObject.transform.CompareTag("Right Hand"))
        {
            leftHand.enabled = true;
        }
    }
    
    // 패널 모두 비활성화하는 함수 
    private void setoffPanels()
    {
        foreach (var VARIABLE in panels)
        {
            VARIABLE.SetActive(false);
        }
    }
    
    public void PressBtn(int idx)
    {
        setoffPanels();
        panels[idx].SetActive(true);
    }
    
    public void TeleportToBuildingForward(GameObject SpawnSpot)
    {
        XRPlayer.transform.position = SpawnSpot.transform.position;
    }
}
