using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class StageManager : MonoBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bomb"))
            {
                GameManager.Instance.currentStage++;
                GameManager.Instance.StartStage();
            }
        }
    }
}