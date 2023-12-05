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
    private GameObject particle;

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
        ItemSpawn itemSpawnComponent = GetComponent<ItemSpawn>();
        if (itemSpawnComponent!=null)
        {
            Debug.Log("아이템 스폰");
            itemSpawnComponent.enabled = false;
        }

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

    public override void DeActivate()
    {

    }

    public override void TakeScore()
    {
        if(currentHP > 1)
        {
            temp.effect = hideEffect;
        }
        else if (currentHP == 1)
        {
            this.gameObject.GetComponent<ItemSpawn>().enabled = true; // 이자식 바꿔야 함
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

        collisionSound.Play();
        visual.SetActive(false);
        GameObject ghostParticle = Instantiate(particle, this.transform);
        ghostParticle.transform.SetParent(null);
        
        bool isDie = DecreaseHP(damage);

        if (isDie)
        {
            //PlaySound(audioClipDie);
            StopAllCoroutines();
            gameObject.SetActive(false);
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            // collider.enabled = false;
            VR.GameManager.Instance.leftMonster--;         // 남은 몬스터 수 줄기
            return;
        }
        MoveMonster();
    }

    private void MoveMonster()
    {
        visual.SetActive(true);
        transform.position += transform.right * 4f;
    }

}
