using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private double _phase;
    private double _sampleRate;
    public float _frequency { get; set; }//middle C
    private float amplitude = 0.5f;

    void Awake()
    {
        _sampleRate = AudioSettings.outputSampleRate;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        double phaseIncrement = _frequency / _sampleRate;
        float noteLength = 0.5f;
        float totalTime = noteLength * 2;
        float currentTime = DateTime.Now.Second % totalTime;

        for (int i = 0; i < data.Length; i += channels)
        {
            float value  = 0;

            if(_phase >= 50.0f) {
                value = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;
            }

            if(_phase >= 100.0f) {
                _phase = 0.0f;
            }

            _phase = _phase + phaseIncrement;

            for(int c = 0; c < channels; c++) {
                data[i + c] = value;
            }
        }
    }
}
