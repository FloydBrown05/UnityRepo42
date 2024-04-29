using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{

    [SerializeField] string _volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] UnityEngine.UI.Slider _slider;
    [SerializeField] float _multiplier = 30f;
    [SerializeField] Toggle _toggle;
    bool _disableToggleEvent;

    void Awake()

    {
        {
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
            _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
        }
    }

    void HandleToggleValueChanged(bool enableSound)
    {
        if (_disableToggleEvent)
            return;

        if (enableSound)
            _slider.value = _slider.maxValue;
        else
            _slider.value = _slider.minValue;

    }

    void OnDisable()
    {
        
        {
            PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
        }
    }

    void HandleSliderValueChanged(float value)
    {

        _mixer.SetFloat(_volumeParameter, value:Mathf.Log10(value) * _multiplier);
        _disableToggleEvent = true;
        _toggle.isOn = _slider.value > _slider.minValue;
        _disableToggleEvent = false;
    }

    void Start()
    {

        _slider.value = PlayerPrefs.GetFloat(_volumeParameter, _slider.value);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
