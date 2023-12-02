using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR;

public class Ghost : Enemy, IMonster
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField] private GameObject visual;
    private GameObject target;
    private GameObject[] spawnPoints;
    private Animator animator;
    private int spawnIndex;

    [SerializeField]
    private AudioSource collisionSound;
    [SerializeField]
    private ParticleSystem particle;

    public float rotationSpeed = 5f;
    public Transform bulletSpawner;

    [Header("Effect")]
    public GameObject hideEffect;
    public GameObject scoreEffect;
    [SerializeField] ScoreEffect temp;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnBullet", 0f, 4f);
        temp = GetComponent<ScoreEffect>();
    }

    public void Spawn(int index)
    {
        Debug.Log("GhostSpawn!");
        // 마시멜로우에 붙어있는 스폰 포인트 중 랜덤으로 하나 설정
        spawnPoints = GameObject.FindGameObjectsWithTag("MarshmallowSP");

        spawnIndex = index % spawnPoints.Length;

        Transform selectedSpawnPoint = spawnPoints[spawnIndex].transform;
        transform.position = selectedSpawnPoint.position;
    }

    void SpawnBullet()
    {
        Instantiate(bullet, bulletSpawner);
    }

    private void Update()
    {
        Vector3 directionToPlayer = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public override void TakeScore()
    {
        if (currentHP >= 1)
        {
            Debug.Log("ghost 피함");
            // 이미지 변경            
            // temp.effect = effect;
        }
        else if (currentHP == 0)
        {
            VR.GameManager.Instance.score += score;
            Debug.Log("Ghost Score!");
            temp.effect = scoreEffect;
        }

    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("ghost damage!");

        collisionSound.Play();
        visual.SetActive(false);
        particle.Play();
        Invoke("MoveMonster", 1f);

        bool isDie = DecreaseHP(damage);

        if (isDie)
        {
            //PlaySound(audioClipDie);
            StopAllCoroutines();
            gameObject.SetActive(false);
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            // collider.enabled = false;
            VR.GameManager.Instance.leftMonster--;         // 남은 몬스터 수 줄기
        }
    }

    private void MoveMonster()
    {
        visual.SetActive(true);
        transform.position += transform.right * 4f;
    }

}
