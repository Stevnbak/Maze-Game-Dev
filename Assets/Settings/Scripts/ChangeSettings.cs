using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeSettings : MonoBehaviour
{
    public Slider graphicSlider;
    public TextMeshProUGUI graphicsText;
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;

    void Start()
    {
        graphicSlider.value = PlayerPrefs.GetFloat("graphics");
        if (graphicSlider.value == 0) graphicsText.text = "Low"; else if (graphicSlider.value == 1) graphicsText.text = "Medium"; else if (graphicSlider.value == 2) graphicsText.text = "High";
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
        sensitivityText.text = sensitivitySlider.value.ToString();
    }


    void Update()
    {
        if(graphicSlider.value != PlayerPrefs.GetFloat("graphics"))
        {
            PlayerPrefs.SetFloat("graphics", graphicSlider.value);
            if (graphicSlider.value == 0) graphicsText.text = "Low"; else if (graphicSlider.value == 1) graphicsText.text = "Medium"; else if (graphicSlider.value == 2) graphicsText.text = "High";
            QualitySettings.SetQualityLevel((int) graphicSlider.value, true);
        }
        if (sensitivitySlider.value != PlayerPrefs.GetFloat("sensitivity"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
            sensitivityText.text = sensitivitySlider.value.ToString();
            if (player != null) player.GetComponent<MovementController>().sensUpdate();
        }
    }
}
