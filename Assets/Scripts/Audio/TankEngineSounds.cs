using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TankEngineSounds : MonoBehaviour
{
    [SerializeField] AnimationCurve[] waves;

    [Space]
    [SerializeField] float idleFrequency;
    [SerializeField] float effortFrequency;

    [Space]
    [SerializeField] float volume;
    [SerializeField][Range(0.0f, 10.0f)] float falloff;
    [SerializeField][Range(0.0f, 1.0f)] float noiseAmp;
    [SerializeField] float noiseFreq;
    [SerializeField] int octaves;
    [SerializeField] float laqunarity;
    [SerializeField] float persistance;

    [Space]
    [SerializeField] float maxSpeed;
    [SerializeField] float smoothtime;

    new public Rigidbody2D rigidbody;

    System.Random random;
    AudioListener listener;
    int sampleRate;
    float time;

    float effort;
    float teffort;
    float effortVel;
    float expDistance;

    private void Awake()
    {
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
        listener = FindObjectOfType<AudioListener>();

        sampleRate = AudioSettings.outputSampleRate;
        random = new System.Random();
    }

    private void Update()
    {
        teffort = rigidbody.velocity.magnitude / maxSpeed;
        expDistance = Mathf.Exp(-falloff * ((Vector2)(Camera.main.transform.position - transform.position)).magnitude);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        int dataLength = data.Length / channels;
        for (int i = 0; i < dataLength; i++)
        {
            float sample = 0.0f;
            effort = Mathf.SmoothDamp(effort, teffort, ref effortVel, smoothtime, float.MaxValue, 1.0f / sampleRate);
            
            foreach (var wave in waves)
            {
                sample += wave.Evaluate(time);
            }

            sample += sample * (MultiLayerNoise(time * noiseFreq) * 2.0f - 1.0f) * noiseAmp;

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] += sample * volume * expDistance;
            }

            time += Mathf.Lerp(idleFrequency, effortFrequency, effort) / sampleRate;

            if (float.IsNaN(time)) time = 0.0f;
            if (time > 100.0f) time %= 100.0f;
        }
    }

    private float MultiLayerNoise(float t)
    {
        float val = 0.0f;
        for (int i = 0; i < octaves; i++)
        {
            float freq = noiseFreq * Mathf.Pow(laqunarity, i);
            float amp = noiseFreq * Mathf.Pow(persistance, i);

            val += Noise(t * freq) * amp;
        }

        return val / 2.0f * noiseAmp;
    }

    private float Noise(float t)
    {
        int i = (int)t;
        float p = t - i;

        float r1 = Random(i) / (float)long.MaxValue;
        float r2 = Random(i + 1) / (float)long.MaxValue;

        return r1 + (r2 - r1) * p;
    }

    public long Random (long n)
    {
        long n1 = 0xb5297a4d;
        long n2 = 0x68e31da4;
        long n3 = 0x1b56c4e9;

        n *= n1;
        n ^= n >> 8;
        n += n2;
        n ^= n << 8;
        n *= n3;
        n ^= n >> 8;
        return n;
    }
}
