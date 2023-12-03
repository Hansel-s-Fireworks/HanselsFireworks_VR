using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VR
{
    public class GunUI : MonoBehaviour
    {
        public Slider s_Hp;
        [SerializeField] private Status status;      // 플레이어 상태 정보 + 이벤트

        private void Awake()
        {
            status.onHPEvent.AddListener(UpdateHPUI);
        }

        // Start is called before the first frame update
        void Start()
        {
            // StartCoroutine(UpdateUI());
        }


        private void UpdateHPUI(int previous, int current)
        {
            // 체력이 증가했을 때는 화면에 빨간색 이미지를 출력하지 않도록 return
            if (previous <= current) return;
            
            if (previous - current > 0)
            {
                s_Hp.value = current; //  GameManager.Instance.hp;
            }
        }
    }

}
