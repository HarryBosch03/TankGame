using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TankEngineSounds : MonoBehaviour
{
    public TankMovement movement;
    new public Rigidbody2D rigidbody;

    [Space]
    public AnimationCurve pop;
    public int cylinders;
    public ParticleSystem.MinMaxCurve popBaseDuration;

    [Space]
    public ParticleSystem.MinMaxCurve noiseFreq;
    public int noiseOctaves;

    [Space]
    public float volume;
    public float falloffOffset;
    public float falloffExponent;
    public ParticleSystem.MinMaxCurve volumeCurve;

    [Space]
    public float speedInfluence;
    public float throttleInfluence;


    System.Random random;
    float popTick;
    float noiseTick;
    float sampleRate;

    AudioListener listener;
    Vector2 listenerPreviousPosition;
    Vector2 listenerVelocity;
    Vector2 listenerDirection;

    float distanceToSource;

    Vector2 velocity;

    private void Start()
    {
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
        if (!movement) movement = GetComponent<TankMovement>();
        listener = FindObjectOfType<AudioListener>();

        sampleRate = AudioSettings.outputSampleRate;
        random = new System.Random();
    }

    private void FixedUpdate()
    {
        velocity = rigidbody.velocity;
        distanceToSource = (listener.transform.position - transform.position).magnitude;

        listenerVelocity = (Vector2)listener.transform.position - listenerPreviousPosition;
        listenerPreviousPosition = listener.transform.position;

        listenerDirection = (listener.transform.position - transform.position).normalized;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        float effort = velocity.magnitude * speedInfluence + movement.ThrottleInput * throttleInfluence;

        int dataLength = data.Length / channels;
        for (int i = 0; i < dataLength; i++)
        {
            float sample = 0.0f;
            for (int c = 0; c < cylinders; c++)
            {
                double t = popTick + c / cylinders;

                float noise = SampleNoise(noiseTick);
                sample += pop.Evaluate((float)t) * noise;
            }

            sample *= volumeCurve.Evaluate(effort) * volume * Mathf.Min(Mathf.Exp(falloffOffset - distanceToSource * falloffExponent), 1.0f);

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] += sample;
            }

            popTick += popBaseDuration.Evaluate(effort) / sampleRate;
            noiseTick += noiseFreq.Evaluate(effort) / sampleRate;
        }
    }

    private float SampleNoise (float t)
    {
        float val = 0.0f;
        float max = 0.0f;

        for (int i = 0; i < noiseOctaves; i++)
        {
            float frequency = Mathf.Pow(2.0f, i);
            float amplitude = Mathf.Pow(0.5f, i);

            max += amplitude;
            val += (Mathf.PerlinNoise(t * frequency, 0.5f) * 2.0f - 1.0f) * amplitude;
        }

        val /= max;
        return val;
    }
}
