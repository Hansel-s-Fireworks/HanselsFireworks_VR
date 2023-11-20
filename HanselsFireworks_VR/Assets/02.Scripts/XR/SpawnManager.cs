using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class SpawnManager : MonoBehaviour
    {
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
            Debug.Log("Spawn Enemy");
            switch (GameManager.Instance.currentStage)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
    }
}