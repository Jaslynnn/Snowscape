using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicTracks;
    private AudioSource audioSource;

    private void Awake()
    {
        // If there is already a music manager in the scene, destroy this new instance.
        if (FindObjectsOfType<BackgroundMusicManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Persist between scenes
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public void PlayMusic(int trackIndex, float volume = 1f)
    {
        if (musicTracks == null || musicTracks.Length == 0)
        {
            Debug.LogWarning("No music tracks assigned!");
            return;
        }

        if (trackIndex < 0 || trackIndex >= musicTracks.Length)
        {
            Debug.LogWarning("Track index out of range!");
            return;
        }

        audioSource.clip = musicTracks[trackIndex];
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void ChangeMusic(int newTrackIndex, float volume = 1f)
    {
        StopMusic();
        PlayMusic(newTrackIndex, volume);
    }
}
