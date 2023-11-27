using System.Collections;
using UnityEngine;

public class Impact : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public float delay;
    private MemoryPool memoryPool;

    private void Awake()
    {
        // particle = GetComponent<ParticleSystem>();
        // animator = GetComponent<Animator>();  // Animator 초기화
    }

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }


    private void OnEnable()
    {
        StartCoroutine(StartAnimation());
    }
    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("효과 비활성화");
        memoryPool.DeactivatePoolItem(gameObject);        
    }

}
