using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveHUDElement : MonoBehaviour
{
    [Header("Values")]
    public string objName;
    public float goal;
    public float count;
    public bool completed;
    [Header("Children")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI subText;

    void Update()
    {
        title.text = objName;
        subText.text = "Progress: " + count + "/" + goal + " (" + Mathf.Round((count/goal*100)) + "%)";
        if(completed)
        {
            subText.color = Color.green;
            title.color = Color.green;
        }
    }
}
