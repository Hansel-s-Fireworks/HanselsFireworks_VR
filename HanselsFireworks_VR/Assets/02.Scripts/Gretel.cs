using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gretel : MonoBehaviour
{
    [SerializeField] private GameObject cage;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject endingUI;

    // Start is called before the first frame update
    void Start()
    {
        endingUI.SetActive(false);
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
        yield return new WaitForSeconds(1f);    // 애니메이션 재생 완료 후 UI 생성
        endingUI.SetActive(true);
    }

    public void RestartToTutorial()
    {
        SceneManager.LoadScene(0);
    }

}
