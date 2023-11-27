using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VR
{
    public class LazorItem : Item
    {
        [SerializeField] private PlayerController player;

        // Start is called before the first frame update 
        void Start()
        {
            // 초기 위치 저장
            originalPosition = transform.position;

            // 코루틴 시작
            StartCoroutine(MoveUpdownCoroutine());
            StartCoroutine(RotateCoroutine());
            player = FindObjectOfType<PlayerController>();
        }

        public override void GetItem()
        {
            player.GetLaserItem();
            Destroy(gameObject);
        }
    }
}