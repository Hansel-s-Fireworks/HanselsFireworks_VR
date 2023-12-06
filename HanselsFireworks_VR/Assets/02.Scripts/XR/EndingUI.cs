using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    public TextMeshProUGUI tScore;
    public TextMeshProUGUI tTime;

    // Start is called before the first frame update
    void Start()
    {
        tScore.text = VR.GameManager.Instance.score.ToString();
        float takenTime = VR.GameManager.Instance.takenTime;
        tTime.text = ConvertToTimeString(takenTime);
    }

    string ConvertToTimeString(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        string timeString = string.Format("{0}m {1}s", minutes, seconds);
        return timeString;
    }
}
