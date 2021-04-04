using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMobs : MonoBehaviour
{
    [SerializeField] float speedmoveX = 1f;
    [SerializeField] float speedmoveY = 0f;
    [SerializeField] float speedmoveZ = 0f;
    void Update()
    {

        transform.position += new Vector3(speedmoveX * Time.deltaTime, speedmoveY * Time.deltaTime, speedmoveZ * Time.deltaTime);
    }
}
