using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinAnimation : MonoBehaviour
{
    public Transform centerPoint;
    public float radius = 2.3f;

    private GameObject target;
    public float angle;

    public float angularSpeed = 30.0f; // 각도 증가 속도 (1초에 몇 도씩 회전할지)
    public float rotationSpeed = 5f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // 각도를 증가시킵니다.
        if (angle <= 360)
            angle += angularSpeed * Time.deltaTime;
        else
            angle = 0;

        // 각도를 라디안으로 변환합니다.
        float radians = Mathf.Deg2Rad * angle;

        // 원의 둘레 좌표 계산
        float x = centerPoint.position.x + radius * Mathf.Cos(radians);
        float y = centerPoint.position.y + radius * Mathf.Sin(radians);

        // 물체의 위치를 설정합니다.
        transform.position = new Vector3(x, y, centerPoint.position.z);

        Vector3 directionToPlayer = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
