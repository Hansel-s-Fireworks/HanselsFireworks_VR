using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VR
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> itemList;
        private int currentItemIndex = 0;

        public GameObject GetCurrentItem()
        {
            return itemList[currentItemIndex++];
        }
    }

}
