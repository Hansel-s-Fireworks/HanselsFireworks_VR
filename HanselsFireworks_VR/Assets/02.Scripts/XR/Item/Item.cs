using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VR
{
    public abstract class Item : MonoBehaviour
    {
        [Header("Rotate Item")]
        [SerializeField] protected GameObject innerItem;
        [SerializeField] protected float rotationSpeed;

        [Header("Move Updown")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float moveDistance; // 이동 거리 (위아래)
        protected Vector3 originalPosition;

        public abstract void GetItem();

        private void Start()
        {
            
        }

        protected IEnumerator MoveUpdownCoroutine()
        {
            float startTime = Time.time;

            while (true) // 계속해서 이동하도록 무한 루프 사용
            {
                // 현재 시간에 따른 이동 값 계산
                float t = (Time.time - startTime) * moveSpeed;
                float newY = Mathf.Sin(t) * moveDistance;

                // 새로운 위치 설정
                Vector3 newPosition = originalPosition + new Vector3(0, newY, 0);
                transform.position = newPosition;

                // 한 프레임 대기
                yield return null;
            }
        }

        protected IEnumerator RotateCoroutine()
        {
            while (true) // 계속해서 회전하도록 무한 루프를 사용
            {
                // 현재 오브젝트의 회전값을 가져옴
                Vector3 currentRotation = innerItem.transform.rotation.eulerAngles;

                // 회전값을 업데이트 (예: y축을 중심으로 회전)
                currentRotation.y += rotationSpeed * Time.deltaTime;

                // 회전을 적용
                innerItem.transform.rotation = Quaternion.Euler(currentRotation);

                // 한 프레임 대기
                yield return null;
            }
        }

    }

}
