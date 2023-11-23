using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    static ChallengeManager instance = null;
    HashSet<string> completeChallenges = new HashSet<string>();
    private AudioSource completeSound;

    public static ChallengeManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            completeSound = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
                Destroy(this.gameObject);
        }
    }

    public HashSet<string> getCompleteChallenges()
    {
        return completeChallenges;
    }
    
    public void CompleteKHUChallenge()
    {
        Debug.Log("KHU 도전과제 완료");
        completeChallenges.Add("KHU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }

    public void CompleteKUChallenge()
    {
        Debug.Log("KU 도전과제 완료");
        completeChallenges.Add("KU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }

    public void CompletePCUChallenge()
    {
        Debug.Log("PCU 도전과제 완료");
        completeChallenges.Add("PCU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }

    public void CompleteCAUChallenge()
    {
        Debug.Log("CAU 도전과제 완료");
        completeChallenges.Add("CAU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }
    public void CompleteKWUChallenge()
    {
        Debug.Log("KWU 도전과제 완료");
        completeChallenges.Add("KWU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }

    public void CompleteJJUChallenge()
    {
        Debug.Log("JJU 도전과제 완료");
        completeChallenges.Add("JJU");
        CompleteChallenge();
        IsCompleteAllChallenge();
    }

    private void CompleteChallenge()
    {
        // 사운드 재생
        completeSound.Play();
        // 이펙트

        // 체크박스

        // UI
    }

    public bool IsCompleteAllChallenge()
    {
        Debug.Log("달성한 도전 과제 수: " + completeChallenges.Count);
        if (completeChallenges.Count == 6)
        {
            return true;
        }
        return false;
    }

}
