using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> bridges;
    public List<Animator> brigeAnimators;
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

    public void setCanDestroy()
    {
        foreach (var bridge in bridges)
        {
            pointsUI.SetActive(true);
        }
    }

}
