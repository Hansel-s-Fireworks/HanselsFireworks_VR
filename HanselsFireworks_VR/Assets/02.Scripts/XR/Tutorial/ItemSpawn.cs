using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class ItemSpawn : MonoBehaviour
    {
        public GameObject item;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Spawn()
        {
            Debug.Log("Spawn Item");
            GameObject clone = Instantiate(item, transform);
            clone.transform.SetParent(null);
            clone.transform.localScale = new Vector3(2, 2, 2);
            // gameObject.SetActive(false);
        }
    }

}