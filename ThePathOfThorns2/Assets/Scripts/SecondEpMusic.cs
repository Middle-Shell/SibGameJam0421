using UnityEngine;

public class SecondEpMusic : MonoBehaviour
{
    [SerializeField] GameObject MusicFolder;

    GameObject nowPlay;
    GameObject toPlay;
    bool changingAudio;
    bool transitioning;

    // Start is called before the first frame update
    void Start()
    { 
        nowPlay = MusicFolder.transform.GetChild(0).gameObject;
        nowPlay.GetComponent<AudioSource>().volume = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < 7.4f)
        {
            TurnOnSing("Main_Theame_1");
        }
        else if (transform.position.x >= 7.4f && transform.position.x < 33f)
        {
            TurnOnSing("Main_Theame_2");
        }
        else if (transform.position.x >= 33f && transform.position.x < 51.5f)
        {
            TurnOnSing("Cave_sound_dripping");
        }
        else if (transform.position.x >= 51.5f)
        {
            TurnOnSing("Main_Theame_2");
        }

        if (transitioning)
        {
            Transition();
        }
    }

    void TurnOnSing(string name)
    {
        for(int i = 0; i < MusicFolder.transform.childCount; i++)
        {
            if (MusicFolder.transform.GetChild(i).gameObject.name == name && toPlay != MusicFolder.transform.GetChild(i).gameObject)
            {
                toPlay = MusicFolder.transform.GetChild(i).gameObject;

                changingAudio = true;
                transitioning = true;
            }
        }

    }

    void Transition()
    {
        //if (changingAudio)
        //{
        //    if (nowPlay.GetComponent<AudioSource>().volume != 0)
        //    {
        //        nowPlay.GetComponent<AudioSource>().volume = Mathf.Lerp(nowPlay.GetComponent<AudioSource>().volume, 0, Time.deltaTime * 2);
        //    }
        //    if (nowPlay.GetComponent<AudioSource>().volume <= 0.001f)
        //    {
        //        nowPlay.GetComponent<AudioSource>().Stop();

        //        nowPlay = toPlay;
        //        changingAudio = false;

        //        nowPlay.GetComponent<AudioSource>().volume = 0;
        //        nowPlay.GetComponent<AudioSource>().loop = true;
        //        nowPlay.GetComponent<AudioSource>().Play();
        //    }
        //}
        //else
        //{
        //    if(nowPlay.GetComponent<AudioSource>().volume != 1)
        //    {
        //        nowPlay.GetComponent<AudioSource>().volume = Mathf.Lerp(nowPlay.GetComponent<AudioSource>().volume, 1, Time.deltaTime * 2);
        //    }
        //    if(nowPlay.GetComponent<AudioSource>().volume >= 1 - .1f)
        //    {
        //        nowPlay.GetComponent<AudioSource>().volume = 1;
        //        transitioning = false;
        //    }
        //}

        if(nowPlay != toPlay)
        {
            AudioSource nowPlayAS = nowPlay.GetComponent<AudioSource>();
            AudioSource toPlayAS = toPlay.GetComponent<AudioSource>();
            
            if (!toPlayAS.isPlaying)
            {
                toPlayAS.volume = 0;
                toPlayAS.Play();
            }

            if (nowPlayAS.volume != 0)
            {
                nowPlayAS.volume = Mathf.Lerp(nowPlayAS.volume, 0, Time.deltaTime * 5);

                toPlayAS.volume = Mathf.Lerp(toPlayAS.volume, .2f, Time.deltaTime * 5);
            }
            if (nowPlayAS.volume <= 0.005f)
            {
                nowPlayAS.Stop();
                nowPlayAS.volume = 0;
                toPlayAS.volume = .2f;

                nowPlay = toPlay;
                transitioning = false;
            }
        }
    }
}
