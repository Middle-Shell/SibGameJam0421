using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]

public class Enemy : MonoBehaviour
{
    //дистанция от которой он начинает видеть игрока
    public float seeDistance = 10f;
    //дистанция до атаки
    public float attackDistance = 2f;
    //скорость енеми
    public float speed = 6;
    //игрок
    private Transform target;
    public SkeletonAnimation skeletonAnimation;

    public AudioClip impact; // наш звук
    AudioSource audio;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < seeDistance)
        {

            if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
            {
                transform.right = target.transform.position - transform.position;
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

            }
            else
            {
                //audioData.Play(0);
                
                skeletonAnimation.AnimationName = "Killed2";
                InvokeRepeating("rep", 0f, 5f);
                
                //skeletonAnimation.state.SetAnimation(0, "Killed2", true);

                //AudioSystem("Fish_bite");
            }
        }
    }
    void rep()
    {
        audio.PlayOneShot(impact, 0.7F);
        StartCoroutine(toSpawn(1f));
    }
    public IEnumerator toSpawn(float delayTime)
    {
        
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //void AudioSystem(string nameOfClip)
    //{
    //    GetComponents<AudioSource>().FirstOrDefault(s => s.clip.name == nameOfClip)?.Play();
    //}
}
