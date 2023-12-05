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

        [Header("Debug")]
        [SerializeField] Vector3 pre;
        [SerializeField] Vector3 cur;
        [SerializeField] private Vector3 direction;

        // [SerializeField] private Mode mode;
        // Start is called before the first frame update
        void Start()
        {
            // impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
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

        // [SerializeField] ItemSpawn itemSpawnComponent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                cur = transform.position;        // 맞은 위치
                direction = (cur - pre).normalized;     // (맞은 위치 - 총구 위치).정규화
                // 방향 벡터를 바탕으로 평면의 법선 벡터를 얻음
                Vector3 normal = direction;
                Quaternion rotation = Quaternion.LookRotation(normal);

                other.GetComponent<Enemy>().TakeDamage(1);


                ItemSpawn itemSpawnComponent = other.GetComponent<ItemSpawn>();
                if (itemSpawnComponent != null && itemSpawnComponent.enabled)
                {
                    Debug.Log("아이템 스폰");
                    itemSpawnComponent.Spawn();
                }

                other.GetComponent<Enemy>().TakeScore();

                // 평면을 정의하기 위한 Quaternion 생성
                // 메모리 풀 안씀
                other.GetComponent<ScoreEffect>().OnSpawnImpact(transform.position, rotation);

                // impactMemoryPool.OnSpawnImpact(other, transform.position, rotation);
                // 임펙트가 소환되는데 구분하고 싶다.
                
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
                other.GetComponent<ScoreEffect>().TakeDamage();
                memoryPool.DeactivatePoolItem(gameObject);
            }
        }
    }
}