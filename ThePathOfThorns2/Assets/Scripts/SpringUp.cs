using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SpringUp : MonoBehaviour, IInteractable
{
    public AudioClip impact; // наш звук
    AudioSource audio;
    //private Rigidbody2D rb2D;
    [SerializeField] GameObject bubble;
    private Vector3 scaleChange;
    [SerializeField] float CoofOxygen = 0.2f;//количество добавляемого О2

    void Start()
    {
        //rb2D = this.GetComponent<Rigidbody2D>();
        bubble = GameObject.FindGameObjectWithTag("Bubble");
        audio = GetComponent<AudioSource>();

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            scaleChange = new Vector3(CoofOxygen, CoofOxygen, CoofOxygen);
            bubble.transform.localScale += scaleChange;
            Debug.Log("UpOxygen");
            audio.PlayOneShot(impact, 0.7F);
            Destroy(gameObject, 0.3f);
        }
    }
    void FixedUpdate()
    {
        //rb2D.AddForce(Vector2.up * 5f); Попытка сделать пузыри на пружинках не увенчалась успехом, оно не работает
        //rb2D.AddForce(new Vector2(2, -5), ForceMode2D.Impulse);
    }
}
