using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]

public class Enemy : MonoBehaviour
{
    public float attackDistance = 2f;
    private Transform target;
    public SkeletonAnimation skeletonAnimation;

    public AudioClip impact; // наш звук
    public new AudioSource audio;
    public MovePlayer MP;


    private bool FishBite; 
    void Start()
    {
        MP = GameObject.FindObjectOfType<MovePlayer>();
        target = GameObject.FindWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        if ((Vector2.Distance(transform.position, target.transform.position) <= attackDistance) && FishBite!=true)
        {
            FishBite = true;
            skeletonAnimation.AnimationName = "Killed";
            SoundEffectsHelper.Instance.AS.PlayOneShot(SoundEffectsHelper.Instance.audioClips[5]);
            MP.health = 0;
            EnemyAI.Instance.ReloadScene();
        }
    }
    /*public IEnumerator toSpawn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
}
