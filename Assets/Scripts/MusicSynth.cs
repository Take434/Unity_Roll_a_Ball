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

    private float _time = 0.0f;

    void Awake()
    {
        _sampleRate = AudioSettings.outputSampleRate;
        Debug.Log("Sample Rate: " + _sampleRate);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        double phaseIncrement = _frequency / _sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            float value  = 0;

            if(_time < 10f) {
                value = Mathf.Sin((float)_phase * 2 * Mathf.PI) * amplitude;
            }

            if(_time > 20f) {
                _time = 0f;
            }

            _phase = (_phase + phaseIncrement) % 1;

            for(int c = 0; c < channels; c++) {
                data[i + c] = value;
            }
        }

        _time += (float)(data.Length / _sampleRate / 2);
    }
}
