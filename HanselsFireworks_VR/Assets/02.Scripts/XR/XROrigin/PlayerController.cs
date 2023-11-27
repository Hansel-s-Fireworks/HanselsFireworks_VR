using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class PlayerController : MonoBehaviour
    {
        public Pistol tuto_pistol;
        public Pistol r_doublePistol;
        public Pistol l_doublePistol;
        public Rifle rifle;

        public Gun[] currentGun;

        // Start is called before the first frame update
        void Start()
        {
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
            tuto_pistol.gameObject.SetActive(false);
            r_doublePistol.laser.SetActive(false);
            l_doublePistol.laser.SetActive(false);
            r_doublePistol.gameObject.SetActive(false);
            l_doublePistol.gameObject.SetActive(false);            
            rifle.gameObject.SetActive(true);
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


    }


}