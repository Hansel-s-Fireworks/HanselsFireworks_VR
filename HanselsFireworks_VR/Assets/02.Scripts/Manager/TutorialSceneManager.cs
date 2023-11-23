using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSceneManager : MonoBehaviour
{    
    public int maxPlayer;
    public Image loadingImage;
    public string mainSceneName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveToRoom());
        }
    }

    IEnumerator MoveToRoom()
    {
        yield return StartCoroutine(FadeOutScene());        // 로딩 씬
    }

    IEnumerator FadeOutScene()
    {
        float timer = 0;
        float delay = 2f;
        float percent = 25f;
        Color targetColor = new Color(0, 0, 0, 1); // 목표 알파 값

        while (timer < delay)
        {
            timer += Time.deltaTime;
            float lerpValue = Mathf.Clamp01(timer / percent);       // 타이머가 얼마나 진행되었는지 비율로 계산
            loadingImage.color = Color.Lerp(loadingImage.color, targetColor, lerpValue);    // 색상의 알파 값을 서서히 변경
            yield return null;
        }
    }


}