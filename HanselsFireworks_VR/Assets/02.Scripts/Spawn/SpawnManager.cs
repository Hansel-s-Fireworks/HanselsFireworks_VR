using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR
{
    // 페이즈 정보를 담고있음 (몬스터 이름과 마리수)
    public class SpawnPhaseInfo
    {
        public List<Tuple<string, int>> monsterData = new List<Tuple<string, int>>(); 
    }

    public class SpawnManager : MonoBehaviour
    {
        public SpawnPhaseInfo[] phasesInStage1;
        public SpawnPhaseInfo[] phasesInStage2;
        public GameObject[] monsterPrefabs;
        private int currentPhase;
        public Transform firstSpawnPoint;

        // Start is called before the first frame update
        void Start()
        {
            CreatePhases();
            currentPhase = 0;
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

        public void SpawnPhase()
        {
            foreach (var monsterInfo in phasesInStage1[currentPhase].monsterData)
            {
                string monsterType = monsterInfo.Item1;
                int count = monsterInfo.Item2;

                for (int i = 0; i < count; i++)
                {
                    IMonster monster = Instantiate(monsterPrefabs[GetMonsterIndex(monsterType)], firstSpawnPoint.position, Quaternion.identity).GetComponent<IMonster>();
                    monster.Spawn();
                }
            }
            currentPhase++;
        }

        int GetMonsterIndex(string monsterType)
        {
            if (monsterType == "LongEnemy")
                return 0;
            else if (monsterType == "ShortEnemy")
                return 1;
            else if (monsterType == "SheildEnemy")
                return 2;
            else if (monsterType == "Ghost")
                return 3;
            else if (monsterType == "Pumkin")
                return 4;
            else
            {
                Debug.Log("Wrong monsterType!");
                return -1;
            }
        }

        // Phase 만들기
        private void CreatePhases()
        {
            // Stage1
            phasesInStage1 = new SpawnPhaseInfo[]
            {
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 10)}
                }
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemey", 10)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 6),
                //                                                 Tuple.Create("Ghost", 1)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 3),
                //                                                 Tuple.Create("Ghost", 2)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 3),
                //                                                 Tuple.Create("Pumkin", 1)}
                //}

            };
        }
    }
}