using UnityEngine;

public class Impact : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Animator animator;  // Animator 추가
    private MemoryPool memoryPool;

    private void Awake()
    {
        // particle = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();  // Animator 초기화
    }

    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }

    private void Update()
    {
        // 애니메이션 재생중이 아니면 삭제
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ScorePopup")) // 애니메이션 클립 이름으로 변경
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
