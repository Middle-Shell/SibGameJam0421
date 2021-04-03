using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private Vector3 scaleChange;

    void Awake()
    {
        scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Update()
    {
        transform.localScale -= scaleChange;
        Invoke("toSpawn", 4f);
    }
    void Minimize()
    {
        scaleChange -= -scaleChange;
    }
}
