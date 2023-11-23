using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreEffectMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject shieldMonsterScoreEffect;
    [SerializeField] private GameObject shortMonsterScoreEffect;
    [SerializeField] private GameObject longMonsterScoreEffect;
    [SerializeField] private GameObject pumpkinMonsterScoreEffect;
    [SerializeField] private GameObject ghostMonsterScoreEffect;

    private MemoryPool shieldMonsterScoreMemoryPool;
    private MemoryPool shortMonsterScoreMemoryPool;
    private void Awake()
    {
        // shieldMonsterScoreMemoryPool = new MemoryPool(shieldMonsterScoreEffect);
        shortMonsterScoreMemoryPool = new MemoryPool(shortMonsterScoreEffect);
    }

    private void OnApplicationQuit()
    {
        // shieldMonsterScoreMemoryPool.DestroyObjects();
        shortMonsterScoreMemoryPool.DestroyObjects();
    }

    // 얘를 총알에서 호출해야 한다. 
    public void OnSpawnImpact(Collider other, Vector3 position, Quaternion rotation)
    {
        Debug.Log("ShortEnemy");
        // 맞힌 위치로 이동
        GameObject item = shortMonsterScoreMemoryPool.ActivatePoolItem();

        item.transform.position = position;
        // item.transform.rotation = rotation;       
        item.transform.SetParent(null);      // 쿠키에 붙어 다니기

        item.GetComponent<Impact>().Setup(shortMonsterScoreMemoryPool);

        /*if (other.GetComponent<ShortEnemy>() != null)
        {
            Debug.Log("ShortEnemy");
            // 맞힌 위치로 이동
            GameObject item = shortMonsterScoreMemoryPool.ActivatePoolItem();

            item.transform.position = position;
            // item.transform.rotation = rotation;       
            item.transform.SetParent(null);      // 쿠키에 붙어 다니기

            item.GetComponent<Impact>().Setup(shortMonsterScoreMemoryPool);

        }*/
        /*else if (other.GetComponent<ShieldedEnemy>() != null)
        {
            // 맞힌 위치로 이동
            GameObject item = shieldMonsterScoreMemoryPool.ActivatePoolItem();

            item.transform.position = position;
            // item.transform.rotation = rotation;       
            item.transform.SetParent(null);      // 쿠키에 붙어 다니기

            item.GetComponent<Impact>().Setup(shieldMonsterScoreMemoryPool);
        }*/
    }
}
