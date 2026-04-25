using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------- Audio Source -----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("-------- Audio Source -----------")]
    public AudioClip background;
    public AudioClip bush;
    public AudioClip walking;
    public AudioClip growling;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}