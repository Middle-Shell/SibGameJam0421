using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringUp : MonoBehaviour
{
    //private Rigidbody2D rb2D;
    [SerializeField] GameObject bubble;
    private Vector3 scaleChange;
    [SerializeField] float CoofOxygen = 5f;//количество добавляемого О2

    void Start()
    {
        //rb2D = this.GetComponent<Rigidbody2D>();
        bubble = GameObject.FindGameObjectWithTag("Bubble");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            scaleChange = new Vector3(CoofOxygen, CoofOxygen, CoofOxygen);
            bubble.transform.localScale += scaleChange;
            Debug.Log("UpOxygen");
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        //rb2D.AddForce(Vector2.up * 5f); Попытка сделать пузыри на пружинках не увенчалась успехом, оно не работает
        //rb2D.AddForce(new Vector2(2, -5), ForceMode2D.Impulse);
    }
}
