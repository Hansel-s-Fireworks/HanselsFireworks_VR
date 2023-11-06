using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Image tooltip;
    public Sprite nextTooltip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeTooltipUI()
    {
        tooltip.sprite = nextTooltip;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
