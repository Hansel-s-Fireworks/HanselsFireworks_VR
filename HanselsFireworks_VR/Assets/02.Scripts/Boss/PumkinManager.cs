using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinManager : MonoBehaviour, IMonster
{
    public static PumkinManager Instance;
    private GameObject target;

    public int completeScore;

    [SerializeField]
    private List<GameObject> pumkins;
    private GameObject[] spawnPoints;
    [SerializeField] private GameObject barrier;
    [SerializeField] private int spawnIndex;
    [SerializeField] ScoreEffect scoreEffect;       // 자식중 마지막 호박의 위치로 
    

    public GameObject completeScoreEffect;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        // scoreEffect = GetComponent<ScoreEffect>();
        StartCoroutine(CheckPumpkin()); 
    }

    //private void Update()
    //{
    //    Vector3 directionToPlayer = target.transform.position - barrier.transform.position;
    //    Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
    //    barrier.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    //    if (pumkins.Count == 0)
    //    {
    //        barrier.SetActive(false);
    //    }
    //}

    IEnumerator CheckPumpkin()
    {
        while (true)
        {
            if (pumkins.Count == 0)
            {
                VR.GameManager.Instance.score += completeScore;
                Debug.Log("모든 호박 제거 성공");
                // scoreEffect.OnSpawnImpactComplete();
                completeScoreEffect.SetActive(true);
                Destroy(this.gameObject, 1f);
                break;
            }
            yield return null;
        }       
    }

    public void Spawn(int index)
    {
        Debug.Log("PumkinSpawn!");
        // 마시멜로우에 붙어있는 스폰 포인트 중 랜덤으로 하나 설정
        spawnPoints = GameObject.FindGameObjectsWithTag("MarshmallowSP");

        spawnIndex = index % spawnPoints.Length;

        Transform selectedSpawnPoint = spawnPoints[spawnIndex].transform;
        transform.position = selectedSpawnPoint.position;
        Invoke("Attack", 20f);
    }

    public void addPumkin(GameObject pumkin)
    {
        pumkins.Add(pumkin);
    }

    public void DeletePumkin(GameObject pumkin)
    {
        pumkins.Remove(pumkin);
    }
    
    public void DeleteAllPumkin()
    {
        pumkins.Clear();
    }

    public List<GameObject> GetPumkinList()
    {
        return pumkins;
    }

    public void Attack()
    {
        foreach (GameObject pumkin in pumkins)
        {
            if(pumkin.activeSelf) pumkin.GetComponent<PumkinEnemy>().Attack();
        }
    }


    public void DeActivate()
    {
        pumkins.Clear();
    }

}