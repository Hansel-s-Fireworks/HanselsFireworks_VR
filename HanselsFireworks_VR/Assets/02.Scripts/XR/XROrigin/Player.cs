using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class Player : MonoBehaviour
    {
        [Header("Gun")]
        public Pistol tuto_pistol;
        public Pistol r_doublePistol;
        public Pistol l_doublePistol;
        public Rifle rifle;
        public Gun[] currentGun;

        [SerializeField] private Status status;      // 이동속도 등의 플레이어 정보
        [SerializeField] private GameObject r_grabRay;
        [SerializeField] private GameObject l_grabRay;


        // Start is called before the first frame update
        void Start()
        {
            status = GetComponent<Status>();
            currentGun = new Gun[2];
        }

        // Update is called once per frame
        void Update()
        {
            if (rifle.isActiveAndEnabled) { ClearCurrentGun();  currentGun[0] = rifle; }
            else if (r_doublePistol.isActiveAndEnabled && l_doublePistol.isActiveAndEnabled)
            {
                ClearCurrentGun();
                currentGun[0] = r_doublePistol;
                currentGun[1] = l_doublePistol;
            }
            else if (tuto_pistol.isActiveAndEnabled)
            {
                ClearCurrentGun();
                currentGun[0] = tuto_pistol;
            }
        }

        public void GetDoublePistolItem()
        {
            tuto_pistol.gameObject.SetActive(false);            
            r_doublePistol.gameObject.SetActive(true);
            l_doublePistol.gameObject.SetActive(true);
            rifle.laser.SetActive(false);
            rifle.gameObject.SetActive(false);
        }

        public void GetRifleItem()
        {
            /*tuto_pistol.gameObject.SetActive(false);
            r_doublePistol.laser.SetActive(false);
            l_doublePistol.laser.SetActive(false);
            r_doublePistol.gameObject.SetActive(false);
            l_doublePistol.gameObject.SetActive(false);            
            rifle.gameObject.SetActive(true);*/
            currentGun[0].BurstMode();
            if (currentGun[1] != null) currentGun[1].BurstMode();

        }

        public void ActivateGrabRay()
        {
            r_grabRay.SetActive(true);
            l_grabRay.SetActive(true);
        }

        public void TakeOffGun()
        {
            currentGun[0].gameObject.SetActive(false);
            if (currentGun[1] != null) currentGun[1].gameObject.SetActive(false);
        }

        public void GetLaserItem()
        {
            currentGun[0].AttachLaser();
            if(currentGun[1] != null) currentGun[1].AttachLaser();
        }
        private void ClearCurrentGun()
        {
            for (int i = 0; i < currentGun.Length; i++)
            {
                currentGun[i] = null;
            }
        }

        private void DeActive()
        {

        }
        public void GetHP()
        {
            
            // GameManager.Instance.hp += 10;
        }


        public void TakeDamage(int damage)
        {
            bool isDie = status.DecreaseHP(damage);
            Debug.Log("Player Damaged");
            if (isDie == true)
            {
                // 죽으면 호출할 거 여기에 하기
                // 1. 마시멜로 성장 멈춤
                // 2. 플레이어 조작 멈추기
                // 3. 화면 빨개지기 + GameOver화면 만들기
                // 4. Retry 버튼 만들기
                Debug.Log("Dead");
            }            
            // GameManager.Instance.hp -= 10;
        }

    }


}