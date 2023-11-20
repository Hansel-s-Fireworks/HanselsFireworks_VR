using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class TutoManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // 시작 버튼에 이벤트로 등록 
        public void StartGame()
        {
            GameManager.Instance.StartStage();
            GameManager.Instance.PlayMainBGM();
        }
    }
}