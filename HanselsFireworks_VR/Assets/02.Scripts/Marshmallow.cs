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
        [SerializeField] private XROrigin player;
        [SerializeField] private float speed;
        [SerializeField] private float timer;

        // Start is called before the first frame update
        void Start()
        {
            speed = 1;
            player = FindObjectOfType<XROrigin>();
            StartStageOne();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Ascend(10, 20));
            }
        }

        public void StartStageOne()
        {
            StartCoroutine(Ascend(0, 10));
        }

        public void Descend()
        {
            // PlaySound(audioClipHurt);
            Debug.Log("Marshmallow Damaged");
            speed -= 0.02f;
        }


        IEnumerator Ascend(int startHeight, int topHeight)
        {
            timer = 0;
            float fakeLoadingDuration = 1f / loadTime;

            while (true)
            {
                timer += Time.deltaTime * speed * fakeLoadingDuration;
                float value = Mathf.Lerp(startHeight, topHeight, timer);
                player.transform.position = new Vector3(0, value * 2, 0);
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