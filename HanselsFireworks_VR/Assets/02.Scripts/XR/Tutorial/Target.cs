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

}
