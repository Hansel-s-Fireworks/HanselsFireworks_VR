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

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(UpdateUI());
        }
        IEnumerator UpdateUI()
        {
            while (true)
            {
                s_Hp.value = GameManager.Instance.hp;

                yield return null;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
