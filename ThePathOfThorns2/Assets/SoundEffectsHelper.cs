using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour
{

    public static SoundEffectsHelper Instance;

    public AudioClip[] audioClips;
    public AudioSource AS;
    private SoundEffectsHelper[] SoundHelpers;

    private void Awake()
    {
        SoundHelpers = GameObject.FindObjectsOfType<SoundEffectsHelper>();
        if (SoundHelpers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        AS = gameObject.GetComponent<AudioSource>();
        this.GetComponent<AudioSource>().enabled = true;
    }

    public void On_Beach_Music()
    {
        AS.clip = audioClips[0];
        AS.Play();
    }

    public void On_Deep_Music()
    {
        AS.clip = audioClips[1];
        AS.Play();
    }

    public void On_Underground_Music()
    {
        AS.clip = audioClips[2];
        AS.Play();
    }
    public void On_Loose_Music()
    {
        AS.clip = audioClips[3];
        AS.Play();
    }
    public void On_Win_Music()
    {
        AS.clip = audioClips[4];
        AS.Play();
    }
    public void On_Bite_Music()
    {
        AS.clip = audioClips[5];
        AS.Play();
    }
}