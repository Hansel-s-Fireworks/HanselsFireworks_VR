using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.AI;

namespace VR
{
    public class ShieldedEnemy : Enemy, IMonster
    {
        [Header("Move Speed")]
        public float pursuitSpeed;
        public float runSpeed;

        [Header("Info")]
        [SerializeField] private float attackRange;
        [SerializeField] private float recognitionRange;            // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)

        [Header("Audio Clips")]
        [SerializeField] private AudioClip audioClipWalk;
        [SerializeField] private AudioClip audioClipRun;
        [SerializeField] private AudioClip audioClipDie;
        [SerializeField] private AudioClip audioClipShieldBreak;
        [SerializeField] private AudioClip audioClipAttack;

        public int shieldScore;
        [SerializeField] private Player target;                           // 적의 공격 대상(플레이어)

        [Header("Effect")]
        public GameObject shieldScoreEffect;
        public GameObject scoreEffect;

        private EnemyState enemyState;    // 현재 적 행동
        public GameObject shield;

        [Header("Component For Debug")]
        [SerializeField] NavMeshAgent nav;
        [SerializeField] Rigidbody rb;
        [SerializeField] Animator animator;
        [SerializeField] BoxCollider candyCane;
        [SerializeField] private CapsuleCollider collider;


        private DissolveEnemy dissoveEffect;
        private GameObject[] spawnPoints;
        private int spawnIndex;
        public float distance;

        [SerializeField] Target temp;

        // Start is called before the first frame update
        void Start()
        {
            enemyState = EnemyState.None;
            target = FindObjectOfType<Player>();        // 플레이어 인식
            animator = GetComponent<Animator>();
            animator.SetInteger("HP", currentHP);
            nav = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            dissoveEffect = GetComponent<DissolveEnemy>();
            collider = GetComponent<CapsuleCollider>();
            audioSource = GetComponent<AudioSource>();
            //navMeshAgent = GetComponent<NavMeshAgent>();
            // ChangeState(EnemyState.Idle);
            temp = GetComponent<Target>();
        }
        public void Spawn(int index)
        {
            Debug.Log("SheildedEnemySpawn!");
            if (VR.GameManager.Instance.currentStage == 1)
                spawnPoints = GameObject.FindGameObjectsWithTag("1stFloorSP");
            else if (VR.GameManager.Instance.currentStage == 3)
                spawnPoints = GameObject.FindGameObjectsWithTag("3rdFloorSP");

            spawnIndex = index % spawnPoints.Length;

            Transform selectedSpawnPoint = spawnPoints[spawnIndex].transform;
            transform.position = selectedSpawnPoint.position;
            nav.enabled = true;
            ChangeState(EnemyState.Idle);
        }

        public override void TakeScore()
        {
            if (currentHP >= 1)
            {
                // 방패 점수
                VR.GameManager.Instance.score += shieldScore;
                Debug.Log("Shielded_Gingerbread Damaged_1");
                // 이미지 변경            
                temp.effect = shieldScoreEffect;
            }
            else if (currentHP == 0)
            {
                VR.GameManager.Instance.score += score;
                Debug.Log("Shielded_Gingerbread Damaged_2");
                temp.effect = scoreEffect;
            }

        }

        public override void TakeDamage(int damage)
        {
            // Debug.Log("Shielded_Gingerbread Damaged");
            bool isDie = DecreaseHP(damage);
            animator.SetInteger("HP", currentHP);
            nav.enabled = false;

            if (isDie == false)
            {
                PlaySound(audioClipShieldBreak);
                animator.SetTrigger("Hit");
                if (currentHP == 1)
                {
                    shield.SetActive(false);
                }
            }
            else
            {
                PlaySound(audioClipDie);

                dissoveEffect.StartDissolve();      // 몬스터 효과 재생
                                                    // 모든 코루틴 스탑 => 중간에 공격모션시 소리나는 에러때문에 
                StopAllCoroutines();
                // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
                collider.enabled = false;
                nav.enabled = false;       // 얘까지 꺼야 땅에 꺼진다. 

                // GameManager.Instance.leftMonster--;

                // WaveSpawner.Instance.;
                Debug.Log("Shielded_Gingerbread Dead");
            }
        }

        public void DeActivate()
        {
            StopAllCoroutines();
            audioSource.mute = true;
            nav.enabled = false;
            animator.speed = 0;
        }


        void FreezeVelocity()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        private void SetStatebyDistance()
        {
            distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < attackRange)
            {
                animator.SetBool("Pursuit", false);
                ChangeState(EnemyState.Attack);
            }
            else if (distance > attackRange && distance <= recognitionRange)
            {
                animator.SetBool("Pursuit", true);
                ChangeState(EnemyState.Pursuit);
            }
            else if (distance > recognitionRange)
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Pursuit", false);
                ChangeState(EnemyState.Idle);
            }
        }

        public void ChangeState(EnemyState newState)
        {
            // 현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기 때문에 return
            if (enemyState == newState) return;
            StopCoroutine(enemyState.ToString());   // 이전에 재생중이던 상태 종료   
            enemyState = newState;                  // 현재 적의 상태를 newState로 설정        
            StartCoroutine(enemyState.ToString());  // 새로운 상태 재생
        }

        private IEnumerator Idle()
        {
            audioSource.Stop();
            nav.speed = 0;
            while (true)
            {
                // 대기상태일 때, 하는 행동
                // 타겟과의 거리에 따라 행동 선태개(배회, 추격, 원거리 공격)
                candyCane.enabled = false;
                nav.enabled = true;
                SetStatebyDistance();

                yield return null;
            }
        }

        private IEnumerator Pursuit()
        {
            PlaySound(audioClipRun);
            while (true)
            {
                animator.SetBool("Attack", false);
                // LookRotationToTarget();         // 타겟 방향을 계속 주시
                // MoveToTarget();                 // 타겟 방향을 계속 이동
                candyCane.enabled = false;
                nav.enabled = true;
                nav.speed = pursuitSpeed;
                nav.SetDestination(target.transform.position);
                SetStatebyDistance();
                yield return null;
            }
        }

        private IEnumerator Attack()
        {
            while (true)
            {
                nav.enabled = false;
                // FreezeVelocity();
                LookRotationToTarget();                 // 타겟 방향을 계속 주시
                animator.SetBool("Attack", true);

                PlaySound(audioClipAttack);
                SetStatebyDistance();
                // ----------------------------------
                // DeActivateCane();
                // yield return new WaitForSeconds(0.2f / 2f);
                ActivateCane();
                yield return new WaitForSeconds(0.75f / 2f);
                DeActivateCane();
                yield return new WaitForSeconds(0.75f / 2f);
            }
        }

        private void ActivateCane() => candyCane.enabled = true;
        private void DeActivateCane() => candyCane.enabled = false;

        private void LookRotationToTarget()
        {
            Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // 내 위치        
            transform.rotation = Quaternion.LookRotation(to - from);            // 바로 돌기
        }

        // private void MoveToTarget()
        // {
        //     Vector3 to = target.transform.position; // 목표 위치
        //     Vector3 from = transform.position;      // 내 위치
        //     moveDirection = (to - from).normalized;
        //     transform.position += moveDirection * moveSpeed * Time.deltaTime;
        // }

        private void OnDrawGizmos()
        {
            // 목표 인식 및 공격 범위
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // 추적 범위
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, recognitionRange);
        }



    }
}
