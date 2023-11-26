using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR;

public interface IMonster
{
    void Spawn();
}

public class Ghost : Enemy, IMonster
{
    [SerializeField]
    private GameObject bullet;
    private GameObject target;
    private GameObject[] spawnPoints;

    public float rotationSpeed = 5f;
    public Transform bulletSpawner;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnBullet", 0f, 2f);
    }

    public void Spawn()
    {
        Debug.Log("GhostSpawn!");
        // 마시멜로우에 붙어있는 스폰 포인트 중 랜덤으로 하나 설정
        spawnPoints = GameObject.FindGameObjectsWithTag("MarshmallowSP");

        int randomIndex = Random.Range(0, spawnPoints.Length);

        Transform selectedSpawnPoint = spawnPoints[randomIndex].transform;
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
        GameManager.Instance.score += this.score * GameManager.Instance.combo;

    }

    public override void TakeDamage(int damage)
    {
        bool isDie = DecreaseHP(damage);
        //animator.SetInteger("HP", currentHP);

        if (isDie)
        {
            //PlaySound(audioClipDie);
            // animator.SetTrigger("Hit");
            StopAllCoroutines();
            // 콜라이더도 제거. 안그러면 dissolve하는 동안 쿠키를 밀고 감
            // collider.enabled = false;
            GameManager.Instance.leftMonster--;         // 남은 몬스터 수 줄기
        }
    }

}
