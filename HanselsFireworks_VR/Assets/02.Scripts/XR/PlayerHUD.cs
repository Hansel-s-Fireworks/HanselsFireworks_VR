using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VR
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("HP & BloodScreen UI")]
        [SerializeField] private Image imageBloodScreen;
        [SerializeField] private AnimationCurve curveBloodScreen;
        [SerializeField] private Status status;      // 플레이어 상태 정보 + 이벤트


        private void Awake()
        {
            status.onHPEvent.AddListener(UpdateHPHUD);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void UpdateHPHUD(int previous, int current)
        {
            // 체력이 증가했을 때는 화면에 빨간색 이미지를 출력하지 않도록 return
            if (previous <= current) return;

            if (previous - current > 0)
            {
                StopCoroutine("OnBloodScreen");
                StartCoroutine("OnBloodScreen");
            }
        }

        private IEnumerator OnBloodScreen()
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime;

                Color color = imageBloodScreen.color;
                color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
                imageBloodScreen.color = color;

                yield return null;
            }
        }
    }

}