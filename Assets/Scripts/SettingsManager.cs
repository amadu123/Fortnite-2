using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public MouseLook mouseLook;
    public Slider senseSlider;
    public Text senseText;

    public void UpdateSensitivity()
    {
        mouseLook.mouseSpeed = senseSlider.value;
        senseText.text = senseSlider.value.ToString();
    }
}
