using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ghost;
    [SerializeField]
    private List<Transform> ghostTransform;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayGhostSpawnAnimation());
    }

    IEnumerator PlayGhostSpawnAnimation()
    {
        foreach (Transform trns in ghostTransform)
        {
        }
        yield return new WaitForSeconds(0.5f);
    }

}
