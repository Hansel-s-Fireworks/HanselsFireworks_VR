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
            player = FindObjectOfType<PlayerController>();
        }

        public override void GetItem()
        {
            player.GetRifleItem();
            Destroy(gameObject);
        }
    }
}