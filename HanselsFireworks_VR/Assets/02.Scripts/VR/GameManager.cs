using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    public class GameManager : MonoBehaviour
    {
        public int[] stageScores;
        public int score;
        public int totalScore;
        public int currentStage;
        public int leftMonster;
        public int leftCase;
        public float elapseTime;
        private float limitedTime;

        public Marshmallow marshmallow;
        // Start is called before the first frame update
        void Start()
        {
            marshmallow = FindObjectOfType<Marshmallow>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
