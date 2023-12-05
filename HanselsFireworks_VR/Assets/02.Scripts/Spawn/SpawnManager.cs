using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
        public Bridge bridge;

        [SerializeField] private int repeatCount;
        [SerializeField] private int maxRepeatCount;
        [SerializeField] private int itemSpawnDuration;
        public ItemManager itemMng;
        public Volume globalVolume;

        private GameObject[] lastEnemies;


        // Start is called before the first frame update
        void Start()
        {
            CreatePhases();
            currentPhase = 0;

            repeatCount = 0;
        }



        public void Spawn()
        {
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
            while (currentPhase < phasesInStageFinal.Length)
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
                }
                yield return new WaitForSeconds(5f);
                currentPhase++;

                if (currentPhase == phasesInStageFinal.Length)
                {
                    // 다리 부수기 이벤트 출력
                    yield return StartCoroutine(BrokeBridge());

                }

            }
        }

        public IEnumerator BrokeBridge()
        {
            yield return new WaitForSeconds(3f);
            // 화면 효과
            globalVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
            colorAdjustments.active = true;
            colorAdjustments.saturation.Override(-100f);
            // 시간 느려짐
            // UI 띄우기

            bridge.setCanDestroy();
            GameObject[] finalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(var enemy in finalEnemies)
            {
                NavMeshAgent navMesh = enemy.GetComponent<NavMeshAgent>();
                if (navMesh != null)
                {
                    enemy.GetComponent<Enemy>().DeActivate();
                }
            }
        }

        public void SpawnPhase()
        {
            StartCoroutine(SpawnPhaseWithDelay());
        }

        private IEnumerator SpawnPhaseWithDelay()
        {
            yield return new WaitForSeconds(1.0f);
            foreach (var monsterInfo in phasesInStage[currentPhase].monsterData)
            {
                string monsterType = monsterInfo.Item1;
                int count = monsterInfo.Item2;

                for (int i = 0; i < count; i++)
                {
                    GameObject spawnedMonster = Instantiate(monsterPrefabs[GetMonsterIndex(monsterType)], firstSpawnPoint.position, Quaternion.identity);
                    
                    if (spawnIndex % itemSpawnDuration == 1)
                    {
                        AddItemToMonster(spawnedMonster, itemMng.GetCurrentItem());
                    }

                    IMonster monster = spawnedMonster.GetComponent<IMonster>();
                    yield return new WaitForSeconds(0.2f);
                    monster.Spawn(spawnIndex);
                    spawnIndex++;
                }
            }
            if (currentPhase < phasesInStage.Length)
                currentPhase++;
        }

        private void AddItemToMonster(GameObject monster, GameObject item_)
        {
            ItemSpawn itemSpawn = monster.AddComponent<ItemSpawn>();
            itemSpawn.item = item_;
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
            else if (monsterType == "GrandSheildEnemy")
                return 7;
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
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 2),
                //                                                 Tuple.Create("SheildEnemy_3", 3)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 4),
                //                                                 Tuple.Create("Ghost", 1)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 2),
                //                                                 Tuple.Create("SheildEnemy_3", 2),
                //                                                 Tuple.Create("Ghost", 1)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("ShortEnemy_3", 4),
                //                                                 Tuple.Create("Ghost", 1)}
                //},
                //new SpawnPhaseInfo
                //{
                //    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 3)}
                //},
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("GrandSheildEnemy", 3)
                }
                }
            };

            //Stage1,2
            phasesInStage = new SpawnPhaseInfo[]
            {
                // 1
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 2),
                                                                 Tuple.Create("ShortEnemy", 2)}
                },
                // 3
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("SheildEnemy", 2)}
                },
                // 5
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("SheildEnemy", 0),
                                                                 Tuple.Create("Ghost", 1)}
                },
                // 7
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 2)}
                },
                // 9
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("Ghost", 1),
                                                                 Tuple.Create("Pumkin", 1)}
                },
                // 11
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 1)}
                },
                // 13
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Ghost", 1)}
                },
                // 15
                new SpawnPhaseInfo
                {
                    monsterData = new List<Tuple<string, int>> { Tuple.Create("LongEnemy", 2),
                                                                 Tuple.Create("Pumkin", 1)}
                }

            };
        }
    }
}