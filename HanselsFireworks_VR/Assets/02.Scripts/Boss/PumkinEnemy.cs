using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinEnemy : Enemy
{
    private Animator animator;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private AudioSource damageSound;

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
        PumkinManager.Instance.addPumkin(this.gameObject);
        target = GameObject.FindGameObjectWithTag("Player");
        canTakeDamage = true;
    }

    public override void TakeScore()
    {
        if (canTakeDamage)
        {
            VR.GameManager.Instance.score += this.score;            
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
        Debug.Log("Attack!");
        canTakeDamage = false;
        animator.SetTrigger("IsAttack");
        StartCoroutine(ApproachTarget());
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
        PumkinManager.Instance.DeletePumkin(this.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeScore();
        }
    }
}
