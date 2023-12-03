using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class BurstItem : Item
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject burstEffectPrefab;

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
            // BurstMode();
            player.GetRifleItem();
            Instantiate(burstEffectPrefab, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}