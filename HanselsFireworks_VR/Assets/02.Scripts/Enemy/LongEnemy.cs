using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;
using VR;

public class LongEnemy : Enemy, IMonster
{
    [Header("Pursuit")]
    [SerializeField] private float attackRange;            // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackDelay;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipDie;
    [SerializeField] private AudioClip audioClipAttack;

    public Animator animator;
    private CapsuleCollider collider;
    private MemoryPool memoryPool;
    private EnemyState enemyState;
    private GameObject[] spawnPoints;
    private int spawnIndex;

    [SerializeField] private XROrigin target;

    private DissolveEnemy dissoveEffect;

    private void Awake()
    {
        memoryPool = new MemoryPool(projectilePrefab);
    }
    private void OnApplicationQuit()
    {
        memoryPool.DestroyObjects();
    }
    public void Spawn(int index)
    {
        //Debug.Log("LongEnemySpawn!");
        if (VR.GameManager.Instance.currentStage == 1)
            spawnPoints = GameObject.FindGameObjectsWithTag("1stFloorSP_Range");
        else if (VR.GameManager.Instance.currentStage == 2)
            spawnPoints = GameObject.FindGameObjectsWithTag("2ndFloorSP");

        spawnIndex = index % spawnPoints.Length;

        Transform selectedSpawnPoint = spawnPoints[spawnIndex].transform;
        transform.position = selectedSpawnPoint.position;
        LookRotationToTarget();
    }

    public override void TakeScore()
    {
        VR.GameManager.Instance.score += score;
    }

    public override void TakeDamage(int damage)
    {
        // bool isDead;
        Debug.Log("LongEnemy Damaged");
        bool isDie = DecreaseHP(damage);
        if(isDie)
        {
            PlaySound(audioClipDie);
            dissoveEffect.StartDissolve();
            // 모든 코루틴 스탑 => 중간에 공격모션시 소리나는 에러때문에 
            StopAllCoroutines();
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            collider.enabled = false;
            // GameManager.Instance.leftMonster--;         // 남은 몬스터 수 줄기

            Debug.Log("Shielded_Gingerbread Dead");
        }
    }

    public void DeActivate()
    {
        StopAllCoroutines();
        audioSource.mute = true;
        animator.speed = 0;
    }

    private void Start()
    {
        enemyState = EnemyState.None;
        target = FindObjectOfType<XROrigin>();        // 플레이어 인식
        animator = GetComponent<Animator>();
        dissoveEffect = GetComponent<DissolveEnemy>();
        collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Attack());
        //ChangeState(EnemyState.Idle);
    }

    //private void CalculateDistanceToTargetAndSelectState()
    //{
    //    float distance = Vector3.Distance(target.transform.position, transform.position);

    //    if (distance <= attackRange)        // 공격하기
    //    {
            
    //        animator.SetBool("Attack", true);
    //        ChangeState(EnemyState.Attack);
    //    }        
    //    else 
    //    { 
    //        animator.SetBool("Attack", false);
    //        ChangeState(EnemyState.Idle);
    //    }
    //}



    //public void ChangeState(EnemyState newState)
    //{
    //    // 현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기 때문에 return
    //    if (enemyState == newState) return;        
    //    StopCoroutine(enemyState.ToString());   // 이전에 재생중이던 상태 종료   
    //    enemyState = newState;                  // 현재 적의 상태를 newState로 설정        
    //    StartCoroutine(enemyState.ToString());  // 새로운 상태 재생
    //}

    private IEnumerator Idle()
    {
        while (true)
        {
            // 대기상태일 때, 하는 행동
            // 타겟과의 거리에 따라 행동 선태개(배회, 추격, 원거리 공격)
            // CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    // 애니메이션 이벤트와 연결
    // 참조 없는 것처럼 보이지만 던지는것과 관련있다. 
    private void ThrowCandyball()
    {
        PlaySound(audioClipAttack);
        // Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        // 메모리 풀을 이용해서 총알 생성
        GameObject clone = memoryPool.ActivatePoolItem();

        clone.transform.position = projectileSpawnPoint.position;
        clone.transform.rotation = projectileSpawnPoint.rotation;
        clone.GetComponent<VR.EnemyProjectile>().moveDirection = CalculateVectorToTarget();

        clone.GetComponent<VR.EnemyProjectile>().Setup(memoryPool);
    }

    private IEnumerator Attack()
    {
        // LookRotationToTarget();
        while (true)
        {
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            animator.SetBool("Attack", false);
            Debug.Log("5초 기다리기 전");
            yield return new WaitForSeconds(attackDelay);
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            // 타겟과의 거리에 따라 행동 선택 (원거리 공격 / 정지)
            //CalculateDistanceToTargetAndSelectState();
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {        
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);              // 내 위치   
        transform.rotation = Quaternion.LookRotation(to - from);                                // 바로 돌기
    }

    private Vector3 CalculateVectorToTarget()
    {
        Vector3 to = target.transform.position; // 목표 위치
        Vector3 from = transform.position;      // 내 위치  
        Vector3 moveDirection = (to - from).normalized;
        return moveDirection;
    }

    private void OnDrawGizmos()
    {
        // 목표 인식 및 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}