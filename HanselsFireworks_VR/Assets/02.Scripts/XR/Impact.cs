using UnityEngine;

namespace VR
{
    public class Impact : MonoBehaviour
    {
        public ParticleSystem particle;
        private MemoryPool memoryPool;

        public void Setup(MemoryPool pool)
        {
            memoryPool = pool;
        }

        private void Update()
        {
            // 파티클이 재생중이 아니면 삭제
            if (particle.isPlaying == false)
            {
                transform.SetParent(null);          // 쿠키에서 떨어지기
                memoryPool.DeactivatePoolItem(gameObject);
            }
        }
    }


}