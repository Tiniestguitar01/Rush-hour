using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> clips;

    public AudioSource player;
    public float time;

    Settings settings;

    void Start()
    {
        player = GetComponent<AudioSource>();
        settings = InstanceCreator.GetSettings();
        GetARandomClip();
    }

    void Update()
    {
        if(player.clip.length > time)
        {
            time += Time.deltaTime;
        }
        else
        {
            player.Stop();
            time = 0;
            GetARandomClip();
        }

        player.volume = settings.volume;
    }

    public void GetARandomClip()
    {
        player.clip = clips[Random.Range(0, clips.Count)];
        player.Play();
    }
}
