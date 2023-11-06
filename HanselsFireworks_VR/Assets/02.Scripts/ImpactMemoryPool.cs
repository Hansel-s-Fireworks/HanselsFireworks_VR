using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private MemoryPool impactMemoryPool;

    private void Awake()
    {
        impactMemoryPool = new MemoryPool(effect);
    }

    private void OnApplicationQuit()
    {
        impactMemoryPool.DestroyObjects();
    }

    // �긦 �Ѿ˿��� ȣ���ؾ� �Ѵ�. 
    public void OnSpawnImpact(Collider other,Vector3 position, Quaternion rotation)
    {
        // ���� ��ġ�� �̵�
        GameObject item = impactMemoryPool.ActivatePoolItem();

        item.transform.position = position;
        // item.transform.rotation = rotation;       
        item.transform.SetParent(other.transform);      // ��Ű�� �پ� �ٴϱ�

        item.GetComponent<Impact>().Setup(impactMemoryPool);
    }
}
