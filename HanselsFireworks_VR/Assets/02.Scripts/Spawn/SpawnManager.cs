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
        public SpawnPhaseInfo[] phasesInStage;
        public SpawnPhaseInfo[] phasesInStageFinal;
        public GameObject[] monsterPrefabs;
        public int currentPhase;
        private int spawnIndex;
        public Transform firstSpawnPoint;

        [SerializeField] private int repeatCount;
        [SerializeField] private int maxRepeatCount;

        // Start is called before the first frame update
        void Start()
        {
            CreatePhases();
            currentPhase = 0;

            repeatCount = 0;
            maxRepeatCount = 5;
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

        /*public void SpawnPhaseFinal()
        {
            foreach (var monsterInfo in phasesInStageFinal[currentPhase].monsterData)
            {
                string monsterType = monsterInfo.Item1;
                int count = monsterInfo.Item2;

                for (int i = 0; i < count; i++)
                {
                    GameObject spawnedMonster = Instantiate(monsterPrefabs[GetMonsterIndex(monsterType)], firstSpawnPoint.position, Quaternion.identity);
                    IMonster monster = spawnedMonster.GetComponent<IMonster>();
                    monster.Spawn(spawnIndex);
                    spawnIndex++;
                }
            }
            currentPhase++;
        }*/
        
        public IEnumerator SpawnPhaseFinal_()
        {
            while (repeatCount < maxRepeatCount)
            {
                foreach (var monsterInfo in phasesInStageFinal[currentPhase].monsterData)
                {
                    string monsterType = monsterInfo.Item1;
                    int count = monsterInfo.Item2;

                    for (int i = 0; i < count; i++)
                    {
                        GameObject spawnedMonster = Instantiate(monsterPrefabs[GetMonsterIndex(monsterType)], firstSpawnPoint.position, Quaternion.identity);
                        IMonster monster = spawnedMonster.GetComponent<IMonster>();
                        yield return new WaitForSeconds(0.5f);
                        monster.Spawn(spawnIndex);
                        spawnIndex++;
                    }
                    yield return new WaitForSeconds(5f);
                    currentPhase++;
                }
                repeatCount++;
            }
        }

        public void SpawnPhase()
        {
            StartCoroutine(SpawnPhaseWithDelay());
        }

        private IEnumerator SpawnPhaseWithDelay()
        {
            foreach (var monsterInfo in phasesInStage[currentPhase].monsterData)
            {
                string monsterType = monsterInfo.Item1;
                int count = monsterInfo.Item2;

                for (int i = 0; i < count; i++)
                {
                    GameObject spawnedMonster = Instantiate(monsterPrefabs[GetMonsterIndex(monsterType)], firstSpawnPoint.position, Quaternion.identity);
                    IMonster monster = spawnedMonster.GetComponent<IMonster>();
                    yield return new WaitForSeconds(0.5f);
                    monster.Spawn(spawnIndex);
                    spawnIndex++;
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
            else if (monsterType == "ShortEnemy_3")
                return 5;
            else if (monsterType == "SheildEnemy_3")
                return 6;
            else
            {
                Debug.Log("Wrong monsterType!");
                return -1;
            }
        }

        // Phase 만들기
        private void CreatePhases()
        {
            // Stage3
            phasesInStageFinal = new SpawnPhaseInfo[]
            {
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 2),
                                                                 Tuple.Create("SheildEnemy_3", 3)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 6),
                                                                 Tuple.Create("Ghost", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 5),
                                                                 Tuple.Create("SheildEnemy_3", 2),
                                                                 Tuple.Create("Ghost", 2)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 20),
                                                                 Tuple.Create("Ghost", 1)}
                }

            };

            //Stage1,2
            phasesInStage = new SpawnPhaseInfo[]
            {
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("ShortEnemy", 0)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("ShortEnemy", 0)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("SheildEnemy", 0),
                                                                 Tuple.Create("Ghost", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 2)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 1),
                                                                 Tuple.Create("Pumkin", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Pumkin", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Pumkin", 1)}
                },
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Pumkin", 1)}
                }

            };
        }
    }
}