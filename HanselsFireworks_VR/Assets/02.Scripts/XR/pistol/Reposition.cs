using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR;

public class Reposition : MonoBehaviour
{
    private Vector3 originalScale;

    public Transform originalTransform;
    public XRDirectInteractor rightHand;
    public XRDirectInteractor leftHand;


    private void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        originalScale = transform.localScale;           // 원래 스케일 저장
        grabbable.selectEntered.AddListener(ShowUI);
        //grabbable.selectExited.AddListener(ReturnPocket);
    }

    public void ShowUI(SelectEnterEventArgs arg)
    {
        transform.SetParent(null);
        transform.localScale = originalScale; // 원래 스케일로 설정
    }

    public void ReturnPocket(SelectExitEventArgs arg)
    {
        transform.position = originalTransform.localPosition;
        transform.rotation = originalTransform.localRotation;
        transform.SetParent(originalTransform);
        rightHand.enabled = true;
        leftHand.enabled = true;
    }
}
