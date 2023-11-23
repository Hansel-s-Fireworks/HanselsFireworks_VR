using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TicketGate : MonoBehaviour
{
    public GameObject leftGate;
    public GameObject rightGate;
    public float temp;
    private Vector3 l_initPos;
    private Vector3 r_initPos;
    public float delay;
    public bool isTaged;
    public GameObject toolTip;
    
    private void Start()
    {
        temp = 0;
        delay = 5;
        isTaged = false;
        l_initPos = leftGate.transform.position;
        r_initPos = rightGate.transform.position;
        if (transform.childCount > 0)
        {
            toolTip = transform.GetChild(0).gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tablet") && isTaged == false)
        {
            isTaged = true;     // 한번만 태그하게 하기
            OpenGate();
            PlayerPrefs.SetInt("달성한 도전과제 수", 1);
        }
    }

    public void ActiveToolTip()
    {
        toolTip.SetActive(true);
    }
    public void OpenGate()
    {
        temp = 0;
        StartCoroutine(AnimateOpenGate());
        toolTip.SetActive(false);
        // StopAllCoroutines();
    }

    private void CloseGate()
    {
        temp = 0;
        StartCoroutine(AnimateCloseGate());
    }
    
    IEnumerator AnimateOpenGate()
    {
        float speed = 10;
        float openDuration = delay; // 문이 열려 있는 시간 (초)
        while (temp < 1)
        {
            temp += Time.deltaTime * speed;
            // 닫혔을 때만
            leftGate.transform.position = Vector3.Lerp(l_initPos, l_initPos - new Vector3(0.25f, 0, 0), temp);
            rightGate.transform.position = Vector3.Lerp(r_initPos, r_initPos + new Vector3(0.25f, 0, 0), temp);
            
            print("문 열림");

            yield return null;
        }
        yield return new WaitForSeconds(openDuration);

        // CloseGate();
    }
    IEnumerator AnimateCloseGate()
    {
        float speed = 10;
        
        while (temp < 1)
        {
            temp += Time.deltaTime * speed;
            // 열렸을 때만 실행
            // if (leftGate.transform.position != l_initPos && rightGate.transform.position != r_initPos)
            // {
                leftGate.transform.position = Vector3.Lerp(l_initPos - new Vector3(0.25f, 0, 0),l_initPos, temp);
                rightGate.transform.position = Vector3.Lerp(r_initPos + new Vector3(0.25f, 0, 0), r_initPos, temp);
            // }
            print("문 닫힘");
            isTaged = false;
            yield return null;
        }
    }
    
}
