using UnityEngine;

public class ChaseAudioManager : MonoBehaviour
{
    public static ChaseAudioManager Instance { get; private set; }

    [System.Serializable]
    public class ChaseLayer
    {
        public string name;       
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [Header("Audio Layers")]
    public ChaseLayer[] layers;
    public bool loopChaseAudio = true;

    private AudioSource[] audioSources;

    void Awake()
    {
        Instance = this;

        audioSources = new AudioSource[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].clip = layers[i].clip;
            audioSources[i].volume = layers[i].volume;
            audioSources[i].loop = loopChaseAudio;
            audioSources[i].playOnAwake = false;
        }
    }

    public void StartChaseAudio()
    {
        double startTime = AudioSettings.dspTime + 0.1;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].volume = layers[i].volume; 
                audioSources[i].PlayScheduled(startTime);
            }
        }
    }

    // Adjust a layer's volume
    public void SetLayerVolume(string layerName, float volume)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].name == layerName)
            {
                layers[i].volume = Mathf.Clamp01(volume);
                audioSources[i].volume = layers[i].volume;
                return;
            }
        }
        Debug.LogWarning($"Chase layer '{layerName}' not found.");
    }

   
    public void SetLayerVolume(int index, float volume)
    {
        if (index < 0 || index >= layers.Length) return;
        layers[index].volume = Mathf.Clamp01(volume);
        audioSources[index].volume = layers[index].volume;
    }

    public void StopChaseAudio()
    {
        foreach (var source in audioSources)
            source.Stop();
    }

    public void StopChaseAudioFade(float duration = 1f)
    {
        StartCoroutine(FadeOut(duration));
    }

    private System.Collections.IEnumerator FadeOut(float duration)
    {
        float[] startVolumes = new float[audioSources.Length];
        for (int i = 0; i < audioSources.Length; i++)
            startVolumes[i] = audioSources[i].volume;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            for (int i = 0; i < audioSources.Length; i++)
                audioSources[i].volume = Mathf.Lerp(startVolumes[i], 0f, elapsed / duration);
            yield return null;
        }

        foreach (var source in audioSources)
            source.Stop();

        // Restore volumes for next chase
        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i].volume = startVolumes[i];
    }
}