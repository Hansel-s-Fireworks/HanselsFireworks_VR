using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class Recoil : MonoBehaviour
    {
        [SerializeField] private Vector3 currentRotation;
        [SerializeField] private Vector3 targetRotation;
        [SerializeField] private Vector3 previousRotation;

        [SerializeField] private float recoilX;
        [SerializeField] private float recoilY;
        [SerializeField] private float recoilZ;

        [SerializeField] private float snappiness;
        [SerializeField] private float returnSpeed;


        // Start is called before the first frame update
        void Start()
        {
            // StartCoroutine(OnRecover());
        }

        // Update is called once per frame
        void Update()
        {
            if (targetRotation.magnitude > 0.1f)
            {
                targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
                currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
            }
            else
            {
                targetRotation = new Vector3(0,0,0);
                currentRotation = new Vector3(0, 0, 0);
            }
            // // 제자리로 돌아오기
            // if(currentRotation.magnitude > 0.1f)
            // {
            //     // 총 기준 제자리
            //     transform.rotation = Quaternion.Euler(currentRotation);
            // }
        }

        float QuaternionAngleDifference(Quaternion a, Quaternion b)
        {
            float angle = Quaternion.Angle(a, b);
            return angle;
        }

        IEnumerator OnRecover()
        {
            while (true)
            {
                Vector3 recoverRotation = Vector3.Slerp(currentRotation, previousRotation, snappiness * Time.deltaTime);
                transform.rotation *= Quaternion.Euler(recoverRotation);
                yield return null;
            }
        }

        public float attackRate = 0.1f;
        public float lastAttackTime = 0;

        IEnumerator OnRecoil()
        {
            // 복귀 할때까지 코루틴 활성화
            while (true)
            {
                if(Time.time - lastAttackTime > attackRate)
                {
                    lastAttackTime = Time.time;

                    transform.rotation *= Quaternion.Euler(currentRotation);        // 반동 반영
                    break;
                }
                // transform.rotation = previousRotation;
                yield return null;
            }
        }

        public void RecoilFire()
        {
            //previousRotation = transform.localEulerAngles;      // 쐈을 때, 회전값
            targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
            StartCoroutine(OnRecoil());
        }
    }
}