using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinEnemy : Enemy
{
    private Animator animator;

    [SerializeField] private GameObject target;
    [SerializeField] private AudioSource damageSound;
    public int damage;
    [SerializeField] ScoreEffect scoreEffect;

    private float approachTime = 1f;

    private bool canTakeDamage = false;
    public float rotationSpeed = 5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsAttack", false);
    }

    private void Start()
    {
        animator.SetTrigger("IsAppear");
        // PumkinManager.Instance.addPumkin(this.gameObject);
        target = GameObject.FindGameObjectWithTag("Player");
        scoreEffect = GetComponent<ScoreEffect>();
        canTakeDamage = true;
    }
    public override void DeActivate()
    {

    }
    public override void TakeScore()
    {
        if (canTakeDamage)
        {
            VR.GameManager.Instance.score += score;
            // scoreEffect.effect = ;
        }
    }

    public override void TakeDamage(int damage)
    {
        damageSound.Play();
        if (canTakeDamage)
        {
            animator.SetTrigger("IsDamage");
            bool isDie = DecreaseHP(damage);
            if (isDie)
            {
                PumkinManager.Instance.DeletePumkin(this.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        canTakeDamage = false;
        animator.SetTrigger("IsAttack");
        StartCoroutine(ApproachTarget());   // 아마 여기 오류뜰텐데 bullet에 닿으면 비활성화되지만
                                            // PumkinManager의 리스트에는 남아있어서
                                            // 비활성화된 object의 함수를 호출할 수 없어서 생긴 오류. 무시해도 될듯
    }

    IEnumerator ApproachTarget()
    {
        yield return new WaitForSeconds(1.0f);

        animator.enabled = false;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < approachTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / approachTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        // PumkinManager.Instance.DeletePumkin(this.gameObject);
        // Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<VR.Player>().TakeDamage(damage);
        }
    }
}
