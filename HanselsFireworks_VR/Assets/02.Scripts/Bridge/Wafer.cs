using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wafer : InteractableObject
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipBreak;

    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        
    }

    public override void TakeScore()
    {
        // GameManager.Instance.score += this.score;
    }

    public override void TakeDamage(int damage)
    {
        bool isDie = DecreaseHP(damage);
        if (isDie)
        {
            // PlaySound(audioClipBreak);
            // audioSource.Play();
            gameObject.SetActive(false);                // 비활성화
            GetComponent<BreakFruit>().Run();
            Debug.Log("Wafer Breaked");
        }
    }
}
