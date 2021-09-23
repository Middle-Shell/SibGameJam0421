using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Vector3 scaleChange;
    [SerializeField] GameObject player;
    

    void Start()
    {
        // Вытягиваем игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player");
        
        InvokeRepeating("Hit", 0f, 1f);
    }
    void Awake()
    {
        scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Hit()
    {
        transform.localScale -= scaleChange;
        if (transform.localScale.x < 0.5f)
        {
            player.GetComponent<MovePlayer>().health -= 0.05f;
        }
    }
    void Hill()
    {
        
        if (transform.localScale.x < 1.25f)
        {
            transform.localScale += scaleChange;
        }
    }
}
