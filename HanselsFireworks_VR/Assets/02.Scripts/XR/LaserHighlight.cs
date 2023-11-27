using UnityEngine;

namespace VR
{
    public class LaserHighlight : MonoBehaviour
    {
        // public Transform laserTransform; // 레이저를 발사하는 지점
        [SerializeField] private GameObject highlightCircle;

        void Update()
        {           
            ShootLaser();
        }

        void ShootLaser()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // 레이캐스트 발사
            if (Physics.Raycast(ray, out hit))
            {
                // 부딪힌 물체의 태그를 검사하여 특정 태그를 가진 오브젝트에게만 강조 효과 활성화
                if (hit.collider.CompareTag("Enemy"))
                {
                    HighlightHitPoint(hit.point, hit.normal);
                }
                else
                {
                    // 특정 태그를 가지지 않은 경우 비활성화
                    highlightCircle.SetActive(false);
                }
            }
            else 
            {
                // 비활성화 
                highlightCircle.SetActive(false); 
            }
        }

        void HighlightHitPoint(Vector3 hitPoint, Vector3 normal)
        {
            highlightCircle.SetActive(true);
            highlightCircle.transform.position = hitPoint;
            highlightCircle.transform.rotation = Quaternion.LookRotation(normal);
        }
    }
}
