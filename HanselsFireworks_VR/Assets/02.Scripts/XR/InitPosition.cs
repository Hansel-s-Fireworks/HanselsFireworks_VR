using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VR;

public class InitPosition : MonoBehaviour
{
    private Vector3 originalScale;

    public GameObject pocket;
    public XRDirectInteractor rightHand;
    public XRDirectInteractor leftHand;


    private void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        originalScale = transform.localScale;           // 원래 스케일 저장
        grabbable.selectEntered.AddListener(ShowUI);
        grabbable.selectExited.AddListener(ReturnPocket);
    }

    public void ShowUI(SelectEnterEventArgs arg)
    {
        transform.SetParent(null);
        transform.localScale = originalScale; // 원래 스케일로 설정
    }

    public void ReturnPocket(SelectExitEventArgs arg)
    {
        transform.position = pocket.transform.position;
        transform.rotation = pocket.transform.rotation;
        transform.SetParent(pocket.transform);
        rightHand.enabled = true;
        leftHand.enabled = true;
    }
}
