using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VR
{
    public class GunUI : MonoBehaviour
    {
        public TextMeshProUGUI tScore;
        public TextMeshProUGUI tLeftMonster;
        public TextMeshProUGUI tLeftCase;
        public Slider s_Velocity;
        public Slider s_StoppedTime;
        [SerializeField] private Marshmallow marshmallow;

        // Start is called before the first frame update
        void Start()
        {
            marshmallow = FindObjectOfType<Marshmallow>();
            StartCoroutine(UpdateUI());
        }

        IEnumerator UpdateUI()
        {
            while (true)
            {
                tScore.text = GameManager.Instance.score.ToString();
                tLeftCase.text = GameManager.Instance.leftCase.ToString();
                s_Velocity.value = marshmallow.growSpeed;
                s_StoppedTime.value = GameManager.Instance.elapseTime 
                    / GameManager.Instance.limitedTime;

                yield return null;
            }
        }
    }
}

 