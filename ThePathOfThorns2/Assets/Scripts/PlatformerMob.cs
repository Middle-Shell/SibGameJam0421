using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMob : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(DeadPlatforma(2f));
        }
    }
    public IEnumerator DeadPlatforma(float delayTime)
    {
        Debug.Log("Platforma");
        yield return new WaitForSeconds(delayTime);
        rb.isKinematic = false;
        Destroy(gameObject, 4);
    }
}
