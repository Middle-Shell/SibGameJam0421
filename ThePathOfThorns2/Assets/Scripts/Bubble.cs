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
        scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void Hit()
    {
        transform.localScale -= scaleChange;
        if (transform.localScale.x < 14f)
        {
            Debug.Log("HIT Oxygen");
            player.GetComponent<MovePlayer>().health -= 0.2f;
        }
}
}
