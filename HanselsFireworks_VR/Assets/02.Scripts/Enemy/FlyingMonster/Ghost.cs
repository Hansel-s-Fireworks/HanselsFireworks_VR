using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR;

public class Ghost : Enemy
{
    [SerializeField]
    private GameObject bullet;
    private GameObject target;

    public float rotationSpeed = 5f;
    public Transform bulletSpawner;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SpawnBullet", 0f, 2f);
    }

    void SpawnBullet()
    {
        Debug.Log("spawnBullet");
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
