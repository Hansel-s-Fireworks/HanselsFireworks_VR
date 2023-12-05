using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
        [SerializeField] private GameObject stageMng;

        // Start is called before the first frame update
        void Start()
        {
            growSpeed = 1;
            spawnDuration = 2;
            nextSpawnHeight = 1f;
            player = FindObjectOfType<XROrigin>();
        }


        private void Update()
        {
            currentHeight = gameObject.transform.position.y;
            if (currentHeight < 10) { VR.GameManager.Instance.currentStage = 1; }
            else if (currentHeight < 20) { VR.GameManager.Instance.currentStage = 2; }
            else 
            { 
                VR.GameManager.Instance.currentStage = 3; 
                stageMng.SetActive(true);
            }

            if(growSpeed < 1)
            {
                // StartCoroutine(OnActivateWarning());
                VR.GameManager.Instance.warningScreen.SetActive(true);
            }
            else
            {
                VR.GameManager.Instance.warningScreen.SetActive(false);
                // StopCoroutine(OnActivateWarning());
            }

            float heightTolerance = 0.01f;

            if (Mathf.Abs(currentHeight - nextSpawnHeight) < heightTolerance && currentHeight < 16)
            {
                Debug.Log("spawnPhase!");
                spawnManager.SpawnPhase();
                nextSpawnHeight += spawnDuration;
            }
        }

        public void StartStage()
        {
            StartCoroutine(Ascend(0, 20));
        }

        public void Descend()
        {
            // PlaySound(audioClipHurt);
            Debug.Log("Marshmallow Damaged");
            float nextSpeed = growSpeed - 0.2f;
            if (nextSpeed > 0) { growSpeed -= 0.2f; Invoke("RecoverSpeed", 2f); }
            else { growSpeed = 0; Invoke("RecoverSpeed", 2f); }
            
        }
        public void StopGrowing()
        {
            StopAllCoroutines();
        }
        IEnumerator OnActivateWarning()
        {
            VR.GameManager.Instance.warningScreen.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            VR.GameManager.Instance.warningScreen.SetActive(false);
            yield return new WaitForSeconds(0.5f);
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