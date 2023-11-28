using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace VR
{
    public class PlayerBullet : MonoBehaviour
    {
        public float speed;
        public float time;
        private MemoryPool memoryPool;
        [SerializeField] private ImpactMemoryPool impactMemoryPool;
        [SerializeField] private ScoreEffectMemoryPool scoreEffectMemoryPool;

        [Header("Debug")]
        [SerializeField] Vector3 pre;
        [SerializeField] Vector3 cur;
        [SerializeField] private Vector3 direction;

        // [SerializeField] private Mode mode;
        // Start is called before the first frame update
        void Start()
        {
            impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
            // scoreEffectMemoryPool = FindObjectOfType<ScoreEffectMemoryPool>();
            transform.SetParent(null);

            // speed = 10;
        }
        public void Setup(MemoryPool pool)
        {
            memoryPool = pool;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            transform.Translate(0, -speed * Time.fixedDeltaTime, 0);

            if (time > 3)
            {
                time = 0;
                memoryPool.DeactivatePoolItem(gameObject);      // 비활성화
            }
        }

        public void GetShootPoint(Vector3 position)
        {
            pre = position;        // 총구 위치
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                cur = transform.position;        // 맞은 위치
                direction = (cur - pre).normalized;     // (맞은 위치 - 총구 위치).정규화
                other.GetComponent<Enemy>().TakeDamage(1);
                other.GetComponent<Enemy>().TakeScore();
                // 방향 벡터를 바탕으로 평면의 법선 벡터를 얻음
                Vector3 normal = direction;
                // ItemSpawn itemSpawnComponent = other.GetComponent<ItemSpawn>();
                // if (itemSpawnComponent != null)
                // {
                //     itemSpawnComponent.Spawn();
                // }

                // 평면을 정의하기 위한 Quaternion 생성
                Quaternion rotation = Quaternion.LookRotation(normal);

                impactMemoryPool.OnSpawnImpact(other, transform.position, rotation);
                // bool 변수 하나 변화주면 부모 스크립트에서 메모리 풀 실행
                // 이펙트 플레이 끝나고서 메모리 풀 해제
                memoryPool.DeactivatePoolItem(gameObject);
            }
            else if (other.CompareTag("Wall") || other.CompareTag("Floor"))
            {
                memoryPool.DeactivatePoolItem(gameObject);
            }
            else if (other.CompareTag("Interactable"))
            {
                // Debug.Log("Interactable");
                other.GetComponent<InteractableObject>().TakeDamage(1);
                other.GetComponent<InteractableObject>().TakeScore();
                memoryPool.DeactivatePoolItem(gameObject);
            }
            else if (other.CompareTag("Item"))
            {
                other.GetComponent<Item>().GetItem();
                memoryPool.DeactivatePoolItem(gameObject);
            }
            else if(other.CompareTag("target"))
            {
                other.GetComponent<Target>().TakeDamage();
                memoryPool.DeactivatePoolItem(gameObject);
            }
        }
    }
}