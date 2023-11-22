using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using VR;
public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public float time;
    private MemoryPool memoryPool;
    [SerializeField] private ImpactMemoryPool impactMemoryPool;
    [SerializeField] private Mode mode;
    // Start is called before the first frame update
    void Start()
    {
        impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {            
            other.GetComponent<Enemy>().TakeDamage(1);
            other.GetComponent<Enemy>().TakeScore();
            // 플레이어 스크립트에 있는 Impact
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            // bool 변수 하나 변화주면 부모 스크립트에서 메모리 풀 실행
            // 이펙트 플레이 끝나고서 메모리 풀 해제
            memoryPool.DeactivatePoolItem(gameObject);
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Interactable"))
        {
            Debug.Log("Interactable");
            other.GetComponent<InteractableObject>().TakeDamage(1);
            other.GetComponent<InteractableObject>().TakeScore();
            
            impactMemoryPool.OnSpawnImpact(other, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Item"))
        {
            other.GetComponent<Item>().GetItem();
        }
    }
}