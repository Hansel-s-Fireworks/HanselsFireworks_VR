using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VR
{
    public class LazorItem : Item
    {
        private FireGun[] fireGun;

        // Start is called before the first frame update
        void Start()
        {
            fireGun = FindObjectsOfType<FireGun>();
        }

        public override void GetItem()
        {
            foreach (var item in fireGun)
            {
                item.AttachLaser();
            }
            Destroy(gameObject);
        }
    }
}