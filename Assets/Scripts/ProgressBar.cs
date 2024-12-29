using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _progressBar;

    public void SetMaxValue(float maxValue) { _progressBar.maxValue = maxValue; }

    public void SetCurrentValue(float currentValue) { _progressBar.value = currentValue; }

    public void SetMinValue(float minValue) { _progressBar.minValue = minValue; }
}