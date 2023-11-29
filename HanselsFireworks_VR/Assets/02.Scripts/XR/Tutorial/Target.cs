using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        GameObject clone = Instantiate(effect,transform);
        clone.transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public void OnSpawnImpact(Vector3 position, Quaternion rotation)
    {
        // 맞힌 위치로 이동
        GameObject item = Instantiate(effect);
        // item.transform.SetParent(null);

        item.transform.position = position;
        item.transform.rotation = rotation;
        // gameObject.SetActive(false);
    }

}
