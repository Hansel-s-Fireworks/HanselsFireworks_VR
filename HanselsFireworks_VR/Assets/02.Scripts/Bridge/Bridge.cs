using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> bridges;
    [SerializeField] private AudioSource bridgeAudio;

    public void PlayBridgeAnimation()
    {
        StartCoroutine(PlayBridgeAnimationWithDelay());
    }

    public IEnumerator PlayBridgeAnimationWithDelay()
    {
        foreach (var bridge in bridges)
        {
            yield return new WaitForSeconds(1.5f);
            bridge.GetComponent<Animator>().enabled = true;
            bridgeAudio.Play();
        }
    }

    public void setCanDestroy()
    {
        foreach (var bridge in bridges)
        {
            bridge.GetComponent<Animator>().enabled = true;
        }
    }

}
