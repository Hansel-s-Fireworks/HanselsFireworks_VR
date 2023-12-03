using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace VR
{
    public class HeartItem : Item
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject hpEffectPrefab;
        [SerializeField] private int increasHP = 50;

        // Start is called before the first frame update
        void Start()
        {
            // 초기 위치 저장
            originalPosition = transform.position;

            // 코루틴 시작
            StartCoroutine(MoveUpdownCoroutine());
            StartCoroutine(RotateCoroutine());
            player = FindObjectOfType<Player>();
        }

        public override void GetItem()
        {
            Debug.Log("Get Heal Item");
            player.GetComponent<Status>().IncreaseHP(increasHP);
            Instantiate(hpEffectPrefab, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
