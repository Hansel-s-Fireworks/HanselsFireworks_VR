using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine.InputSystem;
namespace VR
{
    public class Marshmallow : MonoBehaviour
    {
        public float loadTime;
        public float growSpeed;
        public float currentHeight;
        public float spawnDuration;
        public float nextSpawnHeight;

        [SerializeField] private XROrigin player;
        [SerializeField] private float timer;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private GameObject spawnPoints;

        // Start is called before the first frame update
        void Start()
        {
            growSpeed = 1;
            spawnDuration = 2;
            nextSpawnHeight = 0.1f;
            player = FindObjectOfType<XROrigin>();
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Ascend(10, 20));
            }
            currentHeight = gameObject.transform.position.y;

            float heightTolerance = 0.01f;

            if (Mathf.Abs(transform.position.y - nextSpawnHeight) < heightTolerance)
            {
                Debug.Log("spawnPhase!");
                spawnManager.SpawnPhase();
                nextSpawnHeight += spawnDuration;
            }
        }

        public void StartStage()
        {
            switch (GameManager.Instance.currentStage)
            {
                case 1:
                    StartCoroutine(Ascend(0, 10));
                    break;
                case 2:
                    StartCoroutine(Ascend(10, 20));
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        public void Descend()
        {
            // PlaySound(audioClipHurt);
            Debug.Log("Marshmallow Damaged");
            float nextSpeed = growSpeed - 0.2f;
            if (nextSpeed > 0) { growSpeed -= 0.2f; Invoke("RecoverSpeed", 2f); }
            else { growSpeed = 0; }
            
        }

        private void RecoverSpeed() => growSpeed += 0.2f;

        IEnumerator Ascend(int startHeight, int topHeight)
        {
            timer = 0;
            float fakeLoadingDuration = 1f / loadTime;

            while (true)
            {
                // if(speed <= 1) { speed += Time.deltaTime; }
                timer += Time.deltaTime * growSpeed * fakeLoadingDuration;
                float value = Mathf.Lerp(startHeight, topHeight, timer);
                player.transform.position = new Vector3(0, value * 2, 0);
                spawnPoints.transform.position = new Vector3(0, value * 2, 0);
                transform.position = new Vector3(0, value, 0);
                transform.localScale = new Vector3(2, value, 2);

                if (timer >= 1)
                {
                    Debug.Log("Next Floor");
                    // 수류탄 소환
                    yield break;
                }
                yield return null;
            }
        }
    }
}