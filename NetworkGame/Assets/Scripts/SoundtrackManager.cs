using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Soundtrack
{
    Lobby,
    Recruit,
    Prep,
    Battle,
    None
}

public class SoundtrackManager : MonoBehaviour
{
    private static SoundtrackManager i;
    
    public List<Track> tracks;
    private List<AudioSource> sources = new();
    
    [Serializable]
    public class Track
    {
        public AudioClip clip;
        public bool isLooping;
    }
    
    private void Awake()
    {
        i = this;
        foreach (var track in tracks)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            sources.Add(audioSource);
            audioSource.clip = track.clip;
            audioSource.loop = track.isLooping;
        }
    }


    public static void PlayMusic(Soundtrack track)
    {
        if(track == Soundtrack.None)
            return;

        for (int n = 0; n < i.tracks.Count; n++)
        {
            if ((int)track == n)
                i.sources[n].Play();
            else
            {
                i.sources[n].Stop();
            }
        }
    }

    public static void StopMusic(Soundtrack track)
    {
        for (int n = 0; n < i.tracks.Count; n++)
        {
            if ((int)track == n)
                i.sources[n].Stop();
        }
    }
    
}