using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                if (null == instance)
                {
                    return null;
                }
                return instance;
            }
        }
        private void Awake()
        {
            Debug.Log("game manager");
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        [Header("Info")]
        public int[] stageScores;
        public int score;
        public int totalScore;
        public int currentStage;
        public int leftMonster;
        public int leftCase;
        public float elapseTime;
        public float limitedTime;
        public float spawnDuration;
        public float nextSpawnHeight;

        public float hp;

        [Header("Manager")]
        [SerializeField] private Marshmallow marshmallow;
        [SerializeField] private SpawnManager spawnManager;

        // Start is called before the first frame update
        void Start()
        {
            marshmallow = FindObjectOfType<Marshmallow>();
            spawnManager = FindObjectOfType<SpawnManager>();
            limitedTime = 5;
            elapseTime = 0;
            nextSpawnHeight = 0.3f;
            spawnDuration = 2;
            StartStage();
            Init();
        }

        private void Init()
        {
            hp = 100;
        }

        public void PlayMainBGM()
        {

        }

        public void StartStage()
        {
            marshmallow.StartStage();
            //currentStage = 1;
            StartCoroutine(CheckObjective());
        }
        public void ControlStoppedTime()
        {
            // Debug.Log("ControlStoppedTime");
            if (marshmallow.growSpeed <= 0.2f)
            {
                elapseTime += Time.deltaTime;
            }
            else
            {
                if (elapseTime <= 0) return;
                elapseTime -= Time.deltaTime;
            }
        }

        IEnumerator CheckObjective()
        {
            while (true)
            {
                ControlStoppedTime();
                // if (marshmallow.currentHeight <= 0)
                // {
                //     Debug.Log("Lose");
                //     yield break;
                // }

                if (elapseTime / limitedTime >= 1)
                {
                    Debug.Log("Lose");
                    yield break;
                }
                yield return null;
            }
        }

        IEnumerator CheckObjectiveInStage3()
        {
            while (true)
            {
                // hp바 체력이 조건
                if (true)
                {
                    Debug.Log("Lose");
                    yield break;
                }
                yield return null;
            }
        }
    }
}
