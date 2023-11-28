using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : MonoBehaviour
{
    [SerializeField]
    public GameObject cage, key;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OpenCage()
    {
        StartCoroutine(OpenCageWithDelay());
    }

    private IEnumerator OpenCageWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        cage.SetActive(false);
        key.SetActive(false);
    }
}
