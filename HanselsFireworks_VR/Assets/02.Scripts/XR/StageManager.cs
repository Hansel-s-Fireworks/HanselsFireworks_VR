using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private int repeatCount;
        [SerializeField] private int maxRepeatCount;
        [SerializeField] private SpawnManager spawnManager;

        private void Start()
        {
            repeatCount = 0;
            maxRepeatCount = 5;

            StartCoroutine(RepeatSpawn());
            spawnManager.currentPhase = 0;
        }

        IEnumerator RepeatSpawn()
        {
            while (repeatCount < maxRepeatCount)
            {
                yield return new WaitForSeconds(5f); // 5초 대기
                Debug.Log("마지막꺼 소환");
                spawnManager.SpawnPhaseFinal();

                repeatCount++;
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Bomb"))
        //    {
        //        GameManager.Instance.currentStage++;
        //        GameManager.Instance.StartStage();
        //    }
        //}
    }
}