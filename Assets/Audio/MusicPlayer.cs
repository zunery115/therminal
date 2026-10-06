using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip song;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float volume = 0.4f;

    private static MusicPlayer instance;
    private AudioSource source;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.clip = song;
        source.outputAudioMixerGroup = musicGroup;
        source.loop = true;
        source.playOnAwake = false;
        source.Play();

        SetVolume(volume);
    }

    public void SetVolume(float value)
    {
        if (mixer == null) return;

        float db = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        mixer.SetFloat("MusicVolume", db);
    }
}