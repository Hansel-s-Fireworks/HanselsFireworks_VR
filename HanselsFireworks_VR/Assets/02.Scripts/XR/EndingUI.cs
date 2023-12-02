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
        tTime.text = VR.GameManager.Instance.takenTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
