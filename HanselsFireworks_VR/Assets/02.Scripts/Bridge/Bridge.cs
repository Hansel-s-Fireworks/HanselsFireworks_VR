using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public List<Wafer> bridges;
    public List<Animator> brigeAnimators;
    public List<GameObject> pointsUIs;
    public GameObject pointsUI;

    [SerializeField] private AudioSource bridgeAudio;

    public void PlayBridgeAnimation()
    {
        StartCoroutine(PlayBridgeAnimationWithDelay());
    }

    public IEnumerator PlayBridgeAnimationWithDelay()
    {
        foreach (var bridge in brigeAnimators)
        {
            yield return new WaitForSeconds(1.5f);
            bridge.enabled = true;
            bridgeAudio.Play();
        }
    }


    public void SetCanDestroy()
    {
        foreach (var bridge in bridges)
        {
            bridge.enabled = true;            
        }
        pointsUI.SetActive(true);
        /*foreach (var item in pointsUIs)
        {
            item.SetActive(true);
        }*/
        /*for (int i = 0; i < bridges.Count; i++)
        {
            pointsUI[i].SetActive(true);
            bridges[i].enabled = true;
        }*/

    }
    public bool CheckWaferBroken()
    {
        // for (int i = 0; i < bridges.Count; i++)
        // {
        //     if (bridges[i].CurrentHP != 0)
        //     {
        //         pointsUI[i].SetActive(false);
        //         return false;
        //     }
        // }
        // return true;
        foreach (var bridge in bridges)
        {
            if (bridge.CurrentHP != 0)
            {
                return false;
            }            
        }
        return true;
    }


}
