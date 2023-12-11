using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public Image barImage;
    

    public void UpdateBar(float value, float maxValue)
    {
        slider.value = value / maxValue;
    }
}
