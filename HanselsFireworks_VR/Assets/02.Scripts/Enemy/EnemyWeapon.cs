using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR;
public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Player"))
        // {
        //     other.GetComponent<Player>().TakeScore();
        // }
        if (other.CompareTag("Marshmallow"))
        {
            other.GetComponentInParent<Marshmallow>().Descend();
        }
    }
}
