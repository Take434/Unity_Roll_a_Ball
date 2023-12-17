using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private float _sampleRate;
    private float amplitude = 0.5f;
    private float _time = 0.0f;

    float[,] notes = new float[41,2] {
        {659.25f, 0.5f}, {493.88f, 0.25f}, {523.25f, 0.25f}, {587.33f, 0.5f},  {523.25f, 0.25f}, {493.88f, 0.25f},
        {440.00f, 0.5f}, {440.00f, 0.25f}, {523.25f, 0.25f}, {659.25f, 0.5f},  {587.33f, 0.25f}, {523.25f, 0.25f},
        {493.88f, 0.5f}, {493.88f, 0.25f}, {523.25f, 0.25f}, {587.33f, 0.5f},  {659.25f, 0.5f},
        {523.25f, 0.5f}, {440.00f, 0.5f},  {440.00f, 0.5f},  {0f, 0.5f},
        {0f, 0.25f},     {587.33f, 0.5f},  {698.46f, 0.25f}, {880.00f, 0.5f},  {783.99f, 0.25f}, {698.46f, 0.25f},
        {659.25f, 0.75f},{523.25f, 0.25f}, {659.25f, 0.5f},  {587.33f, 0.25f}, {523.25f, 0.25f},
        {493.88f, 0.5f}, {493.88f, 0.25f}, {523.25f, 0.25f}, {587.33f, 0.5f},  {659.25f, 0.5f},
        {523.25f, 0.5f}, {440.00f, 0.5f},  {440.00f, 0.5f},  {0f, 0.5f},
    };
    const float totalDuration = 16;


    void Awake()
    {
        _sampleRate = AudioSettings.outputSampleRate;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        float timeInc = 1 / _sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            float value = 0f;

            float t = _time;
            for(int j = 0; j < notes.GetLength(0); j++) {
                float freq = notes[j,0];
                float duration = notes[j,1];

                if(t < duration) {
                    value = synth(freq, _time);
                    break;
                    // value = adsr(value, _time, duration);
                }

                t -= duration;
            }


            _time = (_time + timeInc) % totalDuration;

            for(int c = 0; c < channels; c++) {
                data[i + c] = value;
            }
        }
    }

    float synth(float freq, float t) {
        // float output = phase % 1 - 0.5f;
        // output += phase % 1 * 1.005f - 0.5f;
        // output += phase % 1 * 0.995f - 0.5f;
        float output = (float)Math.Pow(Math.Sin(2 * Math.PI * freq * t), 5);
        
        return output / 4;
    }

    float lerp(float x, float x1, float x2, float y1, float y2) {
        return y1 + (x - x1) * (y2 - y1) / (x2 - x1);
    }

    float adsr(float sample, float t, float duration) {
        float attackTime = 0.05f;
        float decayTime = 0.1f;
        float sustainGain = 0.7f;
        float releaseTime = 0.1f;
        
        if(t < attackTime) {
            sample *= lerp(t, 0, attackTime, 0, 1);
        } else if (t < attackTime + decayTime) {
            sample *= lerp(t, attackTime, attackTime + decayTime, 1, sustainGain);
        } else if (t < duration - releaseTime) {
            sample *= sustainGain;
        } else {
            sample *= lerp(t, duration - releaseTime, duration, sustainGain, 0);
        }
        
        return sample;
    }
}
