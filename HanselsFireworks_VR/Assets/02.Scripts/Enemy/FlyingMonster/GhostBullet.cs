using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class GhostBullet : MonoBehaviour
    {
        private Animator animator;

        [SerializeField]
        private GameObject target;
        public int damage;
        private float approachTime = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(ApproachTarget());
        }

        IEnumerator ApproachTarget()
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = target.transform.position;

            float elapsedTime = 0f;

            while (elapsedTime < approachTime)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / approachTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<VR.Player>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else Destroy(gameObject);
        }

    }

}
