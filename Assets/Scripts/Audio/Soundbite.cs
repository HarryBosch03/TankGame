using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio/Soundbite")]
public class Soundbite : ScriptableObject
{
    public AudioClip clip;
    public ParticleSystem.MinMaxCurve pitch = new ParticleSystem.MinMaxCurve(1.0f);
    public ParticleSystem.MinMaxCurve volume = new ParticleSystem.MinMaxCurve(1.0f);

    public void PlaySound ()
    {
        AudioSource player = new GameObject("[TEMP] Soundbite Audiosource").AddComponent<AudioSource>();
        DontDestroyOnLoad(player.gameObject);
        Destroy(player.gameObject, clip.length + 1.0f);

        PlaySound(player);
    }

    public void PlaySound(AudioSource player)
    {
        player.clip = clip;
        player.pitch = pitch.Evaluate(Random.value);
        player.volume = volume.Evaluate(Random.value);

        player.Play();
    }
}
