using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    public List<Animator> bridgeAnimator;
    [SerializeField] private AudioSource bridgeAudio;

    public void PlayBridgeAnimation()
    {
        StartCoroutine(PlayBridgeAnimationWithDelay());
    }

    public IEnumerator PlayBridgeAnimationWithDelay()
    {
        foreach (var animator in bridgeAnimator)
        {
            yield return new WaitForSeconds(1.5f);
            animator.enabled = true;
            bridgeAudio.Play();
        }
    }

}
